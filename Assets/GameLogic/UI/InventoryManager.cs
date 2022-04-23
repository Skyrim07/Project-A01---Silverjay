﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class InventoryManager : MonoSingleton<InventoryManager>
{
    public static List<InventoryItem> InventoryItems = new List<InventoryItem>();

    /// <summary>
    /// See if item can be added to inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryAddItem(InventoryItem item)
    {
        return true;
    }

    /// <summary>
    /// Add item to inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(InventoryItem item, AddItemOptions options = null)
    {
        AddItemOptions option = options ?? new AddItemOptions();
    }

    /// <summary>
    /// See if item can be removed from inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryRemoveItem(InventoryItem item)
    {
        return true;
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(InventoryItem item)
    {

    }

    /// <summary>
    /// Call when use item
    /// </summary>
    /// <param name="item"></param>
    public void OnUseItem(InventoryItem item)
    {
        int count = item.count;
        InventoryItemInfo info = item.info;

    }
}

public class AddItemOptions
{
    public ItemEffect effect;
}

public enum ItemEffect
{
    None,
    Side,
    Center
}
