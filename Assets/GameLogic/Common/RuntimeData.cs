using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class RuntimeData : MonoSingleton<RuntimeData>
{
    #region Camera
    public static Transform camera_Transform;
    public static Vector3 camera_PositionDelta;
    private static Vector3 camera_LastPos;
    #endregion

    #region Player
    public static Vector3 player_Position;
    public static Vector3 player_PositionDelta;
    public static bool player_HasMoveInput;
    public static bool player_IsInJump;
    public static bool player_IsOnSlope;
    public static bool player_IsDashing;
    public static bool player_IsLooking;
    public static int  player_FacingDirection;
    #endregion

    #region Scene
    public static SceneTitle activeSceneTitle;
    #endregion

    #region Weather
    public static bool weather_IsRaining;

    #endregion


    protected override void Awake()
    {
        base.Awake();
        camera_Transform = Camera.main.transform;
    }

    private void Update()
    {
        camera_PositionDelta = camera_Transform.position - camera_LastPos;
        camera_LastPos = camera_Transform.position;
    }
}
