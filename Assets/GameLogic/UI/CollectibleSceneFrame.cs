using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SKCell;
public class CollectibleSceneFrame : MonoBehaviour
{
    const string ANIM_CLIP_NAME = "Appear";

    public InventoryItem item;

    //References
    [SerializeField] SKText nameText, countText;
    [SerializeField] Image iconImage;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    public void Initialize(InventoryItem item)
    {
        this.item = item;
    }

    public void UpdateVisual()
    {
        nameText.UpdateLocalID(ItemData.GetItemInfo(item.itemID).name_LocalID);
        iconImage.sprite = ItemData.GetItemSprite(item.itemID);
        countText.text = item.count.ToString();
    }

    public void StartAnim()
    {
        anim.Play(ANIM_CLIP_NAME);
    }
}
