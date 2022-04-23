using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public sealed class FlowManager : MonoSingleton<FlowManager>
{
    public static SceneTitle scenetitle;
    public static int t_spawnPoint;
    private void Start()
    {
        Application.targetFrameRate = 60;

        scenetitle = (SceneTitle)PlayerPrefs.GetInt("StartScene");
        t_spawnPoint = PlayerPrefs.GetInt("StartSceneSpawnPoint");
        CommonUtils.InvokeAction(0.2f, () =>
        {
            LoadScene(new SceneInfo()
            {
                index = scenetitle,
                spawnPoint = t_spawnPoint
            });
        });
    }
    public void LoadScene(SceneInfo info)
    {
        SceneController.instance.LoadSceneAsset(info);
    }
}
