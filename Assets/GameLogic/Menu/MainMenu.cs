using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class MainMenu : MonoBehaviour
{
    public SKUIAnimation moreInfoPanel;
    public ParticleSystem startFx, moreInfoFx;
    public void OnPressStart()
    {
        if(startFx != null)
            startFx.Play();

        CommonUtils.InvokeAction(0.6f, () =>
        {
            SceneController.instance.LoadSceneAsset(SceneTitle.FireflyFrontier);
        });
    }
    public void OnPressMoreInfo()
    {
        if (moreInfoFx != null)
            moreInfoFx.Play();

        CommonUtils.InvokeAction(0.2f, () =>
        {
            SetState_MoreInfo(true);
        });
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }

    public void SetState_MoreInfo(bool active)
    {
        moreInfoPanel.SetState(active);
    }
}
