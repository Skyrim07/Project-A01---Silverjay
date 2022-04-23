using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

using SKCell;

/// <summary>
/// Common references throughout the game. [Global Only]
/// </summary>
public sealed class CommonReference : MonoSingleton<CommonReference>
{
    public Camera mainCam, uiCam;
    public CinemachineVirtualCamera mainVC;
    public PostProcessVolume ppv;
    public PostProcessProfile[] ppp;
    public PlayerModel playerModel;

    public GameObject fx_Jump;
}
