using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColumnView : MonoBehaviour
{
    [SerializeField] private RectTransform _recTransform;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ColumnModel _columnModel;
    [SerializeField] private RectTransform _centerPosition;
    private int _columnsCount = 4;
    private int _elementsCount = 5;
    [SerializeField] private float _elementHeight;
    [SerializeField] private float _recTransformHeight;

    public void Initialize(ColumnModel columnModel)
    {
        _columnModel = columnModel;

        var randomizedConfigs = _columnModel.ItemConfigs.OrderBy(x => Random.value).ToList();
        _recTransformHeight = columnModel.Height;
        _elementHeight = columnModel.Height / _elementsCount;

        for (int i = 0; i < _columnModel.ElementCounts; i++)
        {
            var randomId = GetRandomElementID();
            var newItem = CreateNewElement(_columnModel.ItemConfigs[randomId]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_recTransform);

        for (int i = 0; i <= _elementsCount; i++)
        {
            var itemType = _columnModel.SlotItemModels[i].ItemType;
            var itemIcon = _columnModel.SlotItemModels[i].ItemIcon.sprite;
            var Cost = _columnModel.SlotItemModels[i].Cost;
            var slotItemConfig = new SlotItemConfig()
            {
                ItemType = itemType,
                ItemIcon = itemIcon,
                Cost = Cost
            };
            CreateNewElement(slotItemConfig);
        }
        _columnModel.PrizeItemModels = _columnModel.SlotItemModels
            .GetRange(_columnModel.SlotItemModels.Count - _elementsCount, _elementsCount);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_recTransform);
    }

    private int GetRandomElementID()
    {
        return Random.Range(0, _columnModel.ItemConfigs.Count);
    }

    private SlotItemModel CreateNewElement(SlotItemConfig config)
    {
        var newItem = Instantiate(_itemPrefab, _recTransform);
        var newItemModel = newItem.GetComponent<SlotItemModel>();
        newItemModel.ItemType = config.ItemType;
        newItemModel.ItemIcon.sprite = config.ItemIcon;
        newItemModel.Cost = config.Cost;
        var newItemRect = newItem.GetComponent<RectTransform>();

        newItemRect.sizeDelta = new Vector2(newItemRect.sizeDelta.x, _elementHeight);
        _columnModel.SlotItemModels.Add(newItemModel);
        return newItemModel;
    }

    public async UniTask ReplaceWithRandomElements()
    {
        int totalElements = _columnModel.SlotItemModels.Count;

        for (int i = 0; i < _elementsCount; i++)
        {
            int randomId = GetRandomElementID();
            var config = _columnModel.ItemConfigs[randomId];
            _columnModel.SlotItemModels[i].CopyFrom(config);
        }

        for (int i = 0; i < _elementsCount; i++)
        {
            var sourceItem = _columnModel.SlotItemModels[i];
            var targetItem = _columnModel.SlotItemModels[totalElements - _elementsCount + i];
            targetItem.CopyFrom(sourceItem);
        }

        await UniTask.CompletedTask;
    }

    public float GetPositionY() => _recTransform.anchoredPosition.y;

    public void SetPositionY(float positionY)
    {
        if (positionY <= -_recTransform.sizeDelta.y)
        {
            var distance = positionY - _recTransform.sizeDelta.y;
            positionY += _recTransform.sizeDelta.y;
        }

        _recTransform.anchoredPosition = new Vector2(_recTransform.anchoredPosition.x, positionY);
    }

    public float GetHeight() => _recTransform.sizeDelta.y;

    public float GetElementHeight() => _elementHeight;

    public (int Id, float DistanceToCenter) GetClosestToCenter()
    {
        float centerPosition = 0f;
        float minDistance = float.MaxValue;
        int closestElementId = -1;

        for (int i = 0; i < _columnModel.SlotItemModels.Count; i++)
        {
            float elementPosition = _columnModel.SlotItemModels[i].GetPositionY();
            float distance = Mathf.Abs(centerPosition - elementPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestElementId = _columnModel.SlotItemModels[i].Id;
            }
        }

        return (closestElementId, minDistance);
    }
}