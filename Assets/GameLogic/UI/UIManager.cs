using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SKCell;
public sealed class UIManager : MonoSingleton<UIManager>
{
    #region Reference
    public SKUIAnimation sceneLabelAnim;
    public SKText sceneLabelText;
    public ParticleSystem fx_SceneLabel;
    #endregion

    private void Start()
    {
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_LOADED, new SJEvent(() =>
        {
            if (GlobalLibrary.G_SCENE_SPECIFICS[RuntimeData.activeSceneTitle].disableUISceneLabel)
                return;

            CommonUtils.InvokeAction(0.8f, () =>
            {
                SetState_SceneLabel(true, GlobalLibrary.G_SCENE_SPECIFICS[RuntimeData.activeSceneTitle].sceneNameLocalID);
                fx_SceneLabel.Play();
                SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_SCENE_TITLE);
                CommonUtils.CancelInvoke("UI_SceneLabelCR");
                CommonUtils.InvokeAction(GlobalLibrary.G_UI_SCENE_LABEL_DURATION, () =>
                {
                    SetState_SceneLabel(false);
                }, 0, 0, "UI_SceneLabelCR");
            });
        }));
    }
    public void SetState_SceneLabel(bool appear, int localID)
    {
        sceneLabelAnim.SetState(appear);
        if (localID > 0)
        {
            sceneLabelText.UpdateLocalID(localID);
        }
    }
    public void SetState_SceneLabel(bool appear, string label = "")
    {
        sceneLabelAnim.SetState(appear);
        if(label.Length > 0)
        {
            sceneLabelText.text = label;
        }
    }
}
