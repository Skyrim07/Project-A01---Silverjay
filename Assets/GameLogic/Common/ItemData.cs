using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class ItemData : Singleton<ItemData>
{
    public static Dictionary<int, InventoryItemInfo> InventoryItemInfo = new Dictionary<int, InventoryItemInfo>()
    {

    };
}

public class InventoryItem
{
    public InventoryItemInfo info;
    public int count, maxCount;
}
public class InventoryItemInfo
{
    public int id;
    public InventoryItemType type;
    public string name;
    public string description;
}

public enum InventoryItemType
{
    Consumable,
    Narrative,
    Treasure,
    Ingredient,
}
