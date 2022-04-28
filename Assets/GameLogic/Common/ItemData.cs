using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class ItemData : Singleton<ItemData>
{
    public static Dictionary<int, Sprite> InventoryItemSprites = new Dictionary<int, Sprite>(); 
    public static Dictionary<int, InventoryItemInfo> InventoryItemInfo = new Dictionary<int, InventoryItemInfo>()
    {
        //Common 100-200

        //Consumable 201-300
        //阿希雅之花
        {201, new InventoryItemInfo(){
            id = 201,
            name_LocalID = 2001,
            type = InventoryItemType.Consumable
        }},
        //瑟雅之瓶
        {202, new InventoryItemInfo(){
            id = 202,
            name_LocalID = 2002,
            type = InventoryItemType.Consumable
        }},
        //理之砂
        {203, new InventoryItemInfo(){
            id = 203,
            name_LocalID = 2003,
            type = InventoryItemType.Consumable
        }},
        //Artifact 310-400

        //Feather 401-500

        //Collectible 501-600

        //Narrative 601-700

        //Treasure 701-800

        //Food 801-900
        //精酿啤酒
        {801, new InventoryItemInfo(){
            id = 801,
            name_LocalID = 2012,
            type = InventoryItemType.Food
        }},
        //萤火鸡尾酒
        {804, new InventoryItemInfo(){
            id = 804,
            name_LocalID = 2013,
            type = InventoryItemType.Food
        }},
        //松软的蜜饼
        {802, new InventoryItemInfo(){
            id = 802,
            name_LocalID = 2016,
            type = InventoryItemType.Food
        }},
        //热辣角
        {803, new InventoryItemInfo(){
            id = 803,
            name_LocalID = 2015,
            type = InventoryItemType.Food
        }},
        //Misc 901-1000

    };

    public static InventoryItemInfo GetItemInfo(int id)
    {
        return InventoryItemInfo[id];
    }

    public static Sprite GetItemSprite(int id)
    {
        if(!InventoryItemSprites.ContainsKey(id))
        {
            Sprite sp = Resources.Load<Sprite>($"ItemIcon/Item_{id}");
            CommonUtils.InsertOrUpdateKeyValueInDictionary(InventoryItemSprites, id, sp);
        }
        return InventoryItemSprites[id];
    }
}

[System.Serializable]
public class InventoryItem
{
    public int itemID; //Use this to index the invertoryiteminfo
    public int count, maxCount;
}
public class InventoryItemInfo
{
    public int id;
    public InventoryItemType type;
    public int name_LocalID, description_LocalID;
}

public enum InventoryItemType
{
    Consumable,
    Food,
    Narrative,
    Treasure,
    Ingredient,
}

