using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySideTab : MonoBehaviour
{
    public InventoryItemType type;

    [SerializeField] ParticleSystem fx;
    public void OnClick()
    {
        if (fx)
            fx.Play();
        InventoryManager.instance.OnSelectSideTab(this);
    }
}
