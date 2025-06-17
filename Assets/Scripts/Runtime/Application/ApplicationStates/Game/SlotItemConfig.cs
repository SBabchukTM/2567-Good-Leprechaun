using UnityEngine;

[CreateAssetMenu(fileName = "SlotItemConfig", menuName = "Config/SlotItemConfig")]
public class SlotItemConfig : ScriptableObject
{
    public SlotItemType ItemType;
    public Sprite ItemIcon;
    public int Cost;
}