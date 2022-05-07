using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SKCell;
public sealed class InventoryManager : MonoSingleton<InventoryManager>
{
    public static Dictionary<int, InventoryItem> InventoryItems = new Dictionary<int, InventoryItem>(); //Detailed info of all items
    public List<InventoryItem> displayingInventoryItems = new List<InventoryItem>();
    public InventoryItem selectedItem;
    public InventoryItemFrame selectedFrame;

    public static bool isOpen = false;

    public InventoryItemType currentType = InventoryItemType.All;
    //References
    [SerializeField] Transform itemFrameContainer;
    [SerializeField] SKUIPanel panel;
    [SerializeField] GridLayoutGroup grid;
    //Info panel
    [SerializeField] SKUIPanel ip_Panel;
    [SerializeField] SKText ip_TitleText, ip_DescripText, ip_EffectText, ip_CategoryText, ip_CountText;
    [SerializeField] Image ip_ItemImage;
    [SerializeField] GameObject useButton;

    private Coroutine inventoryDisplayCR;
    public GameObject itemFramePF, itemFrameEmptyPF, collectibleFramePF;

    private void Start()
    {
        CommonUtils.AddKeyDownAction(GlobalLibrary.INPUT_OPEN_INVENTORY_KEYCODE, () =>
        {
            ToggleState();
        });
        CommonUtils.AddKeyDownAction(GlobalLibrary.INPUT_EXIT_KEYCODE, () =>
        {
            SetState(false);
        });

        AddItem(new InventoryItem()
        {
            itemID = 801,
            count = 2
        });
        AddItem(new InventoryItem()
        {
            itemID = 802,
            count = 4
        });
        AddItem(new InventoryItem()
        {
            itemID = 803,
            count = 1
        });
        AddItem(new InventoryItem()
        {
            itemID = 804,
            count = 1
        });
    }


    public void SetState(bool appear)
    {
        if (appear)
        {
            ClearInventoryFrames();
            CommonUtils.InvokeAction(0.5f, () =>
            {
                UpdateInventoryDisplay(false);
            });
        }
        else
        {
            SetInfoPanelState(SKUIPanelState.Inactive);
        }

        panel.SetState(appear ? SKUIPanelState.Active : SKUIPanelState.Inactive);
         isOpen = appear;
    }

    public void ToggleState()
    {
        SetState(!isOpen);
    }

    /// <summary>
    /// Updates the item frames according to currentType
    /// </summary>
    public void UpdateInventoryDisplay(bool clearInventoryFrames=true)
    {
        if(clearInventoryFrames)
            ClearInventoryFrames();
        ClearSelection();

        foreach (var item in InventoryItems)
        {
            //Filter out certain types of item
            if (currentType == InventoryItemType.All || ItemData.GetItemInfo(item.Value.itemID).type == currentType)
            {
                displayingInventoryItems.Add(item.Value);
            }
        }

        if (inventoryDisplayCR != null)
            StopCoroutine(inventoryDisplayCR);
        inventoryDisplayCR = StartCoroutine(InventoryDIsplayCR());
    }

    private void ClearSelection()
    {
        displayingInventoryItems.Clear();
        selectedFrame = null;
        selectedItem = null;
        SetInfoPanelState(SKUIPanelState.Inactive);
    }

    IEnumerator InventoryDIsplayCR()
    {
        //Display
        foreach (var item in displayingInventoryItems)
        {
            GameObject go = Instantiate(itemFramePF);
            go.transform.SetParent(itemFrameContainer);
            go.transform.localScale = Vector3.one;

            InventoryItemFrame frame = go.GetComponent<InventoryItemFrame>();
            frame.UpdateInfo(item);
            frame.UpdateVisual();
            yield return new WaitForSeconds(0.08f);
        }
        for (int i = 0; i < grid.constraintCount - displayingInventoryItems.Count% grid.constraintCount; i++)
        {
            GameObject go = Instantiate(itemFrameEmptyPF);
            go.transform.SetParent(itemFrameContainer);
            go.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ClearInventoryFrames()
    {
        foreach(var item in itemFrameContainer.GetAllChildren())
        {
            CommonUtils.SafeDestroy(item.gameObject);
        }
    }
    public void UpdateInfoPanel(InventoryItem item = null)
    {
        SetInfoPanelState(SKUIPanelState.Active);
        InventoryItem i = item == null ? selectedItem : item;
        InventoryItemInfo info = ItemData.GetItemInfo(item.itemID);
        ip_TitleText.UpdateLocalID(info.name_LocalID);
        ip_DescripText.UpdateLocalID(info.description_LocalID);
        ip_CategoryText.UpdateLocalID(ItemData.GetItemTypeLocalID(info.type));
        ip_ItemImage.sprite = ItemData.GetItemSprite(item.itemID);
        ip_CountText.UpdateText(item.count, info.maxCount);
        ip_CountText.UpdateLocalID(GlobalLibrary.G_INVENTORY_COUNT_TEXT_LOCAL_ID);

        useButton.SetActive(info.consumable);
    }

    public void SetInfoPanelState(SKUIPanelState state)
    {
        ip_Panel.SetState(state);
    }
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
        //If item already exists, update count
        if (InventoryItems.ContainsKey(item.itemID))
        {
            InventoryItem existingItem = InventoryItems[item.itemID];
            existingItem.count+=item.count;
        }
        else //If adding a new item
        {
            InventoryItems.Add(item.itemID, new InventoryItem()
            {
                count = item.count,
                itemID = item.itemID,
            });
        }
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

    }

    /// <summary>
    /// On player collects a collectible item in the scene
    /// </summary>
    /// <param name="item"></param>
    public void OnCollectibleItemCollect(CollectibleItem item, bool displayFrame=true)
    {
        InventoryItem itemInfo = item.itemInfo;
        AddItem(itemInfo);
        if (displayFrame)
        {
            InstantiateCollectibleFrame(item);
        }
    }

    public void InstantiateCollectibleFrame(CollectibleItem item)
    {
        InventoryItem itemInfo = item.itemInfo;
        GameObject frameGO = Instantiate(collectibleFramePF, item.panelPos.position, Quaternion.identity, item.transform);
        CollectibleSceneFrame frame = frameGO.GetComponent<CollectibleSceneFrame>();
        frame.Initialize(item.itemInfo);
        frame.UpdateVisual();
        frame.StartAnim();

        CommonUtils.InvokeAction(3f, () =>
        {
            if (frame.gameObject)
            {
                Destroy(frame.gameObject);
            }
        });
    }
    public void OnSelectItemFrame(InventoryItemFrame frame)
    {
        if(selectedFrame != null)
        {
            if(selectedFrame!=frame)
            selectedFrame.OnDeselect();
        }
        selectedFrame = frame;
        selectedItem = frame.item;
        UpdateInfoPanel(selectedItem);
    }
    public void OnSelectSideTab(InventorySideTab tab)
    {
        currentType = tab.type;
        UpdateInventoryDisplay();
    }
}

public class AddItemOptions
{
    public AddItemEffect effect;
}

public enum AddItemEffect
{
    None,
    Side,
    Center
}
