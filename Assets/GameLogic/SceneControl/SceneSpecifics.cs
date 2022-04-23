using System;

/// <summary>
/// Specify scene variables
/// </summary>
[Serializable]
public struct SceneSpecifics
{
    public string sceneName;
    public int sceneNameLocalID;
    public float cameraSize;
    public SceneWeather weather;
    public int postprocessingProfile;
    public string postprocessingProfileAssetName;

    public bool disableCameraDeadzone, enableSmoothOpening;
    public bool disableUISceneLabel, disablePlayerGravity, disablePlayerDash;

    public string[] bgms; //file name of bgms
}

[Serializable]
public struct SceneWeather
{
    public bool activeOnStart;
    public WeatherType type;
    public float interval;
    public float probability;
}

public enum WeatherType
{
    None,
    Rain,
    Snow
}