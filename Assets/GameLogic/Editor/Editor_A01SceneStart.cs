using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public sealed class Editor_A01SceneStart : EditorWindow
{
    static SceneTitle sceneTitle;
    static string lastActiveScene;
    static int spawnPoint = 0;
    static bool waitForReset = false;

    static GameObject cam_Preview;

    [MenuItem("A01/Scene Controller")]
    public static void Initialize()
    {
        Editor_A01SceneStart window = GetWindow<Editor_A01SceneStart>("Scene Controller");
        Texture HierarchyIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Textures/SceneController.png");
        GUIContent content = new GUIContent("Scene Controller", HierarchyIcon);
        window.titleContent = content;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Scene:", GUILayout.Width(50));
        sceneTitle = (SceneTitle)PlayerPrefs.GetInt("StartScene");
        sceneTitle = (SceneTitle)EditorGUILayout.EnumPopup(sceneTitle, GUILayout.Width(200));
        PlayerPrefs.SetInt("StartScene", (int)sceneTitle);

        EditorGUILayout.LabelField("Spawn Point:", GUILayout.Width(100));
        spawnPoint = PlayerPrefs.GetInt("StartSceneSpawnPoint");
        spawnPoint = EditorGUILayout.IntField(spawnPoint, GUILayout.Width(30));
        PlayerPrefs.SetInt("StartSceneSpawnPoint", spawnPoint);

        if(GUILayout.Button("R", GUILayout.Width(20))){
            spawnPoint = 0;
            PlayerPrefs.SetInt("StartSceneSpawnPoint", 0);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUI.contentColor = new Color(0.9f, 0.8f, 0.2f);
        if (GUILayout.Button("Start!"))
        {
            EndCameraPreview();
            if (Application.isPlaying)
            {
                SceneController.instance.LoadSceneAsset(sceneTitle);
            }
            else
            {
                EditorApplication.ExitPlaymode();
                lastActiveScene = "Assets/Scenes/" + EditorSceneManager.GetActiveScene().name + ".unity";
                PlayerPrefs.SetString("LastActiveScene", lastActiveScene);

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene("Assets/Scenes/" + GlobalLibrary.G_SCENE_PREPARE_ASSET_NAME + ".unity");
                EditorApplication.EnterPlaymode();
            }
        }
        GUI.contentColor = Color.white;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("End!"))
            {
                EditorApplication.ExitPlaymode();
                waitForReset = true;
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open scene in editor"))
            {
                EndCameraPreview();
                EditorSceneManager.OpenScene("Assets/Scenes/" + GlobalLibrary.G_SCENE_ASSET_NAME[sceneTitle] + ".unity");
            }

            if (GUILayout.Button("Open prepare scene"))
            {
                EndCameraPreview();
                EditorSceneManager.OpenScene("Assets/Scenes/" + GlobalLibrary.G_SCENE_PREPARE_ASSET_NAME + ".unity");
            }

            if (GUILayout.Button("Camera preview"))
            {
                EnterCameraPreview();
            }
            EditorGUILayout.EndHorizontal();
        }

        if (waitForReset)
        {
            if (!Application.isPlaying)
            {
                lastActiveScene = PlayerPrefs.GetString("LastActiveScene");
                EditorSceneManager.OpenScene(lastActiveScene);
                waitForReset = false;
            }
        }
    }

    private void EnterCameraPreview()
    {
        GameObject prev = GameObject.Find("Cam_Preview");
        if(prev != null)
            DestroyImmediate(prev);
        cam_Preview = new GameObject("Cam_Preview");

        cam_Preview.hideFlags = HideFlags.DontSaveInEditor;
        Camera cam = cam_Preview.AddComponent<Camera>();

        SceneSpecifics sp = GlobalLibrary.G_SCENE_SPECIFICS[sceneTitle];
        cam.orthographic = true;
        cam.orthographicSize = sp.cameraSize;

        PostProcessLayer ppLayer = cam_Preview.AddComponent<PostProcessLayer>();
        ppLayer.volumeLayer = 1 << 8;

        GameObject prevPPVgo = GameObject.Find("Cam_Preview_PPV");
        if( prevPPVgo != null)
            DestroyImmediate (prevPPVgo);
        GameObject PPVgo = new GameObject("Cam_Preview_PPV") ;
        PPVgo.layer = 8;
        PPVgo.transform.SetParent(cam_Preview.transform);
        PostProcessVolume ppv = PPVgo.AddComponent<PostProcessVolume>();
        ppv.profile = AssetDatabase.LoadAssetAtPath<PostProcessProfile>($"Assets/Scenes/PPProfiles/{sp.sceneName}.asset");

        BoxCollider cld= PPVgo.AddComponent<BoxCollider>();
        cld.isTrigger = true;

        Transform spawnPointCT = GameObject.FindGameObjectWithTag(GlobalLibrary.G_SCENE_TAG_SPAWNPOINT).transform;
        Vector3 pos = spawnPointCT.GetChild(spawnPoint).position;
        cam.transform.position = pos;

        EditorApplication.ExecuteMenuItem("Window/General/Game");
    }

    private void EndCameraPreview()
    {
        if(cam_Preview != null)
            DestroyImmediate(cam_Preview);
    }
}
