using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SKCell;

public class InventoryItemFrame : MonoBehaviour
{
    const string SELECTED_BOOL = "Selected";
    public InventoryItem item;

    //References
    [SerializeField] Button button;
    [SerializeField] SKText countText;
    [SerializeField] Image iconImage;

    [SerializeField] Animator anim, selectionAnim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnSelect()
    {
        selectionAnim.SetBool(SELECTED_BOOL, true);
        button.interactable = false;
        InventoryManager.instance.OnSelectItemFrame(this);
    }
    public void OnDeselect()
    {
        button.interactable = true;
        selectionAnim.SetBool(SELECTED_BOOL, false);
    }
    public void UpdateInfo(InventoryItem item)
    {
        this.item = item;
    }
    public void UpdateVisual()
    {
        iconImage.sprite = ItemData.GetItemSprite(item.itemID);
        countText.text = item.count.ToString();
    }
}
