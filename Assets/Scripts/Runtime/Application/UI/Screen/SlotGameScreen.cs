using Application.Services.UserData;
using Application.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SlotGameScreen : UiScreen
{
    private UserDataService _userDataService;
    private IUiService _uiService;

    [SerializeField] private Button _stopSpinButton;
    [SerializeField] private Image _stopSpinButtonImage;

    [SerializeField] private Button _optionsButton;

    [SerializeField] private RectTransform _slotsBoard;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private List<Column> _columns;
    [SerializeField] private GameObject _columnPrefab;
    [SerializeField] private int _columnsCount = 5;
    [SerializeField] private int _rowsCount = 5;
    [SerializeField] private int _spinSpeed;

    [SerializeField] private List<SlotItemConfig> ItemConfigs;
    [SerializeField] private List<Row> _rows = new List<Row>(5);

    public event Action OptionsButtonPressEvent;

    [Inject]
    public void Construct(UserDataService userDataService, IUiService uiService)
    {
        _userDataService = userDataService;
        _uiService = uiService;
    }

    private async void OnDestroy()
    {
        await StopAllColumnControllers();
    }

    public event Action<bool, int> GameOverEvent;

    public async void Initialize(int columnsCount, int spinSpeed)
    {
        GetUserData();
        _columns.Clear();
        _columnsCount = columnsCount;
        _spinSpeed = spinSpeed;

        for (int i = 0; i < _columnsCount; i++)
        {
            await CreateColumn();
        }

        _optionsButton.onClick.AddListener(OnOptionsButtonPress);
        _stopSpinButton.onClick.AddListener(() => StopSpinButtonPress());
    }

    public void GetUserData()
    {
        var money = _userDataService.GetUserData().Money;
        _moneyText.text = money.ToString();
        var buttonColor = _userDataService.GetUserData().UsedButtonColor;
        _stopSpinButtonImage.color = buttonColor;
    }

    public async UniTask CreateColumn()
    {
        var column = Instantiate(_columnPrefab, _slotsBoard);

        var columnModel = column.GetComponent<ColumnModel>();
        columnModel.ItemConfigs = ItemConfigs;
        columnModel.ColumnsCount = _columnsCount;
        columnModel.IsSpinning = true;
        columnModel.MaxSpinSpeed = _spinSpeed;
        columnModel.Height = _slotsBoard.rect.height;

        var columnView = column.GetComponent<ColumnView>();
        columnView.Initialize(columnModel);

        var columnController = new ColumnController(columnModel, columnView);
        _columns.Add(new Column(columnController, columnModel));
        await UniTask.CompletedTask;
        // await columnController.Run(default);
    }

    public async UniTask StopSpinButtonPress()
    {
        _stopSpinButton.interactable = false;
        _optionsButton.interactable = false;
        _rows.Clear();

        foreach (var column in _columns)
        {
            column.Controller.Run(default);
        }

        await UniTask.Delay(1000);

        var replaceTasks = new List<UniTask>();
        foreach (var column in _columns)
        {
            replaceTasks.Add(column.Controller.ReplaceRewards());
        }
        await UniTask.WhenAll(replaceTasks);

        foreach (var column in _columns)
        {
            await column.Controller.Stop();
            await UniTask.Delay(500);
        }

        await AddItemToRow();
        GameOver();
        _stopSpinButton.interactable = true;
        _optionsButton.interactable = true;
    }

    private async UniTask AddItemToRow()
    {
        _rows.Clear();
        _rows = new List<Row>(_rowsCount);

        for (int i = 0; i < 5; i++)
        {
            _rows.Add(new Row(i));
        }

        foreach (var column in _columns)
        {
            for (int i = 0; i < column.Model.PrizeItemModels.Count; i++)
            {
                _rows[i].SlotItemModels.Add(column.Model.PrizeItemModels[i]);
            }
        }
        await UniTask.CompletedTask;
    }

    private int CheckRowsForUniformity()
    {
        var score = 0;
        foreach (var row in _rows)
        {
            if (row.SlotItemModels.Count == 0)
            {
                Debug.Log($"Row {row.Id} is empty.");
                continue;
            }

            var firstItemType = row.SlotItemModels[0].ItemType;

            bool allSame = row.SlotItemModels.All(item => item.ItemType == firstItemType);

            if (allSame)
            {
                score += row.SlotItemModels[0].Cost;
            }
        }
        return score;
    }

    public bool AreAllColumnsStopped()
    {
        return _columns.TrueForAll(column => !column.Model.IsSpinning);
    }

    public void GameOver()
    {
        var score = 0;

        var isWin = true;
        score = CheckRowsForUniformity();

        if (score > 0)
        {
            isWin = true;

            _userDataService.GetUserData().Money += score;
            _userDataService.SaveUserData();
            var money = _userDataService.GetUserData().Money;
            _moneyText.text = money.ToString();

            GameOverEvent?.Invoke(isWin, score);
        }
        else
        {
            isWin = false;
        }
    }

    private void OnOptionsButtonPress()
    {
        OptionsButtonPressEvent?.Invoke();
    }

    public async UniTask StopAllColumnControllers()
    {
        foreach (var column in _columns)
        {
            await column.Controller.Stop();
        }
    }
}
[Serializable]
public struct Column
{
    public ColumnController Controller;
    public ColumnModel Model;

    public Column(ColumnController controller, ColumnModel model)
    {
        Controller = controller;
        Model = model;
    }
}
[Serializable]
public struct Row
{
    public int Id;
    public List<SlotItemModel> SlotItemModels;

    public Row(int id)
    {
        Id = id;
        SlotItemModels = new List<SlotItemModel>();
    }

    public string LogRow()
    {
        foreach (var slotItemModel in SlotItemModels)
        {
            Debug.Log($"row {Id} , item = {slotItemModel.ItemType}");
        }

        return base.ToString();
    }
}