using UnityEngine;
using UnityEngine.UI;

public class SlotItemModel : MonoBehaviour
{
    public int Id;
    public SlotItemType ItemType;
    public Image BackgroundImage;
    public Image ItemIcon;
    public int Cost;

    [SerializeField] private RectTransform _recTransform;

    private void OnEnable()
    {
        _recTransform = GetComponent<RectTransform>();
    }

    public float GetPositionY() => _recTransform.anchoredPosition.y;

    public void CopyFrom(SlotItemConfig other)
    {
        ItemType = other.ItemType;
        ItemIcon.sprite = other.ItemIcon;
        Cost = other.Cost;
    }
    public void CopyFrom(SlotItemModel other)
    {
        ItemType = other.ItemType;
        ItemIcon.sprite = other.ItemIcon.sprite;
        Cost = other.Cost;
    }
}

public enum SlotItemType
{
    None,
    Gold,
    Clover,
    Horseshoe,
    Hat,
    Ten,
    Jack,
    Queen,
    King,
    Ace,
    PotOfGold,
}