using Application.Services.Audio;
using Core.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPopup : BasePopup
{
    private readonly string _loseTitle = "you lose";
    private readonly string _winTitle = "you won";

    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private Button _goToHomeButton;
    [SerializeField] private TextMeshProUGUI _costText;

    [SerializeField] private TextMeshProUGUI _titleText;

    [SerializeField] private Image _backgroundImage;

    public event Action RestartLevelPressEvent;
    public event Action GoToHomeButtonPressEvent;

    public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
    {
        var gameOverPopupData = data as GameOverPopupData;

        if (gameOverPopupData.IsWin)
        {
            _titleText.text = _winTitle;
            AudioService.PlaySound(ConstAudio.WinSound);
        }
        else
        {
            _titleText.text = _loseTitle;
            AudioService.PlaySound(ConstAudio.DisplaySound);
        }

        _costText.text = gameOverPopupData.WinCost.ToString();

        Initialize();
        return base.Show(data, cancellationToken);
    }

    public void Initialize()
    {
        _restartLevelButton.onClick.AddListener(OnRestartLevelButtonPress);
        _goToHomeButton.onClick.AddListener(OnGoToHomeButtonPress);
    }

    public override void DestroyPopup()
    {
        AudioService.PlaySound(ConstAudio.ClosePopupSound);
        _restartLevelButton.onClick.RemoveAllListeners();
        _goToHomeButton.onClick.RemoveAllListeners();
        base.DestroyPopup();
    }

    public void OnRestartLevelButtonPress()
    {
        AudioService.PlaySound(ConstAudio.PressButtonSound);
        RestartLevelPressEvent?.Invoke();
        Hide();
    }

    public void OnGoToHomeButtonPress()
    {
        AudioService.PlaySound(ConstAudio.PressButtonSound);
        GoToHomeButtonPressEvent?.Invoke();
        Hide();
    }
}