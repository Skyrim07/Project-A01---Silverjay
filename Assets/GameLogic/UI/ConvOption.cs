using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class ConvOption : MonoBehaviour
{
    public int id;
    public SentenceOption option;

    [SerializeField] SKButton button;
    public void Initialize(SentenceOption option)
    {
        this.option = option;
    }

    public void SetState(bool isOn)
    {
        //button.gameObject.SetActive(isOn);
        gameObject.SetActive(isOn);
    }
    public void UpdateVisual()
    {
        button.SetText(option.content);

        //Localization
        if(option.localID>=0)
            button.UpdateText(option.localID);

        button.TransitNormal();
    }

    public void OnSelect()
    {
        ConversationManager.instance.OnSelectResponse(id);
    }
}
