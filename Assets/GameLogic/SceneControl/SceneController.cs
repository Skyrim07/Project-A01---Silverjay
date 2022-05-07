using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using SKCell;
public sealed class SceneController : MonoSingleton<SceneController>
{
    [HideInInspector]
    public Transform spawnPointCT, checkPointCT;

    [HideInInspector]
    public List<Transform> spawnPoints, checkPoints;

    public SceneInfo sceneInfo;

    private void Start()
    {
        SKSceneManager.instance.onNextSceneLoaded.AddListener( ()=>
        {
            CommonUtils.InvokeAction(0.2f, () =>
            {
                LoadSceneSetup();
            });
        });
    }
    public void LoadSceneAsset(SceneInfo info)
    {
        EventDispatcher.Dispatch(EventDispatcher.Common, EventRef.CM_ON_SCENE_EXIT);
        sceneInfo = info;
        SKSceneManager.instance.LoadSceneAsync(GlobalLibrary.G_SCENE_LOADING_ASSET_NAME, GlobalLibrary.G_SCENE_ASSET_NAME[info.index]);
    }
    public void LoadSceneAsset(SceneTitle title)
    {
        EventDispatcher.Dispatch(EventDispatcher.Common, EventRef.CM_ON_SCENE_EXIT);
        sceneInfo = new SceneInfo()
        {
            index = title
        };
        SKSceneManager.instance.LoadSceneAsync(GlobalLibrary.G_SCENE_LOADING_ASSET_NAME, GlobalLibrary.G_SCENE_ASSET_NAME[sceneInfo.index]);
    }
    public void LoadSceneSetup()
    {
        LoadInterestPoints();
        LoadCamera();
        LoadPlayer();
        LoadSpecifics();

        RuntimeData.activeSceneTitle = sceneInfo.index;
        EventDispatcher.Dispatch(EventDispatcher.Common, EventRef.CM_ON_SCENE_LOADED);
    }
    public void LoadCamera()
    {
        Collider2D confiner =  GameObject.FindGameObjectWithTag(GlobalLibrary.G_CAMERA_CONFINER_TAG)?.GetComponent<Collider2D>();
        CommonReference.instance.mainVC.GetComponent<CinemachineConfiner>().m_BoundingShape2D = confiner;
        CommonReference.instance.mainVC.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].disableCameraDeadzone ? 0 : GlobalLibrary.G_CAMERA_DEADZONE_WIDTH;

        if (GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].enableSmoothOpening)
        {
            CommonUtils.InvokeAction(0.8f, () =>
            {
                float osize = CommonReference.instance.mainVC.m_Lens.OrthographicSize;
                CommonUtils.StartProcedure(SKCurve.QuadraticIn, 3.0f, (f) =>
                {
                    CommonReference.instance.mainVC.m_Lens.OrthographicSize = osize + (3) * (1-f);
                });
            });
        }
    
    }

    public void LoadPlayer()
    {
        CommonReference.instance.playerModel.PlayerMain.TeleportToSpawnPoint(sceneInfo.spawnPoint);
    }

    public void LoadSpecifics()
    {
        CommonReference.instance.mainVC.m_Lens.OrthographicSize = GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].cameraSize;
        CommonReference.instance.uiCam.orthographicSize = GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].cameraSize;
        if(GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].postprocessingProfile>=0)
        CommonReference.instance.ppv.profile = CommonReference.instance.ppp[GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].postprocessingProfile];

        CommonReference.instance.playerModel.PlayerMovement.canDash = !GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].disablePlayerDash;
        WeatherController.instance.OnLoadWeatherAsset(GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].weather);
        AudioManager.instance.LoadBGM(GlobalLibrary.G_SCENE_SPECIFICS[sceneInfo.index].bgms);
    }
    public void LoadInterestPoints()
    {
        spawnPointCT = GameObject.FindGameObjectWithTag(GlobalLibrary.G_SCENE_TAG_SPAWNPOINT).transform;
        checkPointCT = GameObject.FindGameObjectWithTag(GlobalLibrary.G_SCENE_TAG_CHECKPOINT).transform;

        if(spawnPointCT==null)
        {
            CommonUtils.EditorLogWarning("Load Interest Points FAILED: Cannot find spawn point container.");
        }
        else
        {
            LoadSpawnPoints();
        }

        if (checkPointCT == null)
        {
            CommonUtils.EditorLogWarning("Load Interest Points FAILED: Cannot find check point container.");
        }
        else
        {
            LoadCheckPoints();
        }
    }

    public void LoadSpawnPoints()
    {
        spawnPoints = new List<Transform>();
        for (int i = 0; i < spawnPointCT.childCount; i++)
        {
            spawnPoints.Add(spawnPointCT.GetChild(i));
        }
    }

    public void LoadCheckPoints()
    {
        checkPoints = new List<Transform>();
        for (int i = 0; i < checkPointCT.childCount; i++)
        {
            checkPoints.Add(checkPointCT.GetChild(i));
        }
    }
}

[System.Serializable]
public struct SceneInfo
{
    public SceneTitle index;
    public int spawnPoint;
}

public enum SceneTitle
{
    MainMenu = 0,
    FireflyFrontier = 1,
    FireflyTavern = 2,
    WesternTower = 3,
    FarawayShack = 4,
    TestScene = 5,
}
