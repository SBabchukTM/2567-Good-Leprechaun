using System;
using System.Collections.Generic;
using UnityEngine;

public class ColumnModel : MonoBehaviour
{
    public List<SlotItemModel> SlotItemModels;
    public List<SlotItemModel> PrizeItemModels;
    public float MinSpinTime = 3;
    public float MaxSpinTime = 5;
    public int ElementCounts = 10;
    public List<SlotItemConfig> ItemConfigs;
    public float MaxSpinSpeed = 1000;
    public int ColumnsCount = 3;
    public bool IsSpinning = true;
    public SlotItemModel PrizeItem;
    public float Height;
    public Action OnSpinStarted;
    public Action OnSpinStopped;

}