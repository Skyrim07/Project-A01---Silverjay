using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class PlayerModel : MonoBase
{
    public static PlayerModel Instance;
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerMain PlayerMain { get; private set; }
    public PlayerEffect PlayerEffect { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }

    //References
    public Transform playerPos_Foot;
    public override void Start()
    {
        base.Start();
        Instance = this;

        //Get references
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerMain = GetComponent<PlayerMain>();
        PlayerEffect = GetComponent<PlayerEffect>();
        PlayerCombat = GetComponent<PlayerCombat>();

        //Initialize modules
        if(PlayerMovement)
            PlayerMovement.Initialize();
        if (PlayerMain)
            PlayerMain.Initialize();
        if (PlayerEffect)
            PlayerEffect.Initialize();
        if (PlayerCombat)
            PlayerCombat.Initialize();

        //Console events
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CONSOLE_ON_OPEN, new SJEvent(() =>
        {
            SetModuleState(PlayerModule.Movement, false);
        }));
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CONSOLE_ON_CLOSE, new SJEvent(() =>
        {
            SetModuleState(PlayerModule.Movement, true);
        }));

        //Dispatch event
        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_MAIN_MODULE_START);
    }

    public void SetModuleState(PlayerModule mod, bool isActive)
    {
        if (mod == PlayerModule.Movement)
        {
            if (PlayerMovement)
                PlayerMovement.SetState(isActive);
        }
        else if (mod == PlayerModule.Main)
        {
            if (PlayerMain)
                PlayerMain.SetState(isActive);
        }
        else if (mod == PlayerModule.Combat)
        {
            if (PlayerCombat)
                PlayerCombat.SetState(isActive);
        }
    }
    public override void Tick(float unscaledDeltaTime, float deltaTime)
    {
        //Tick modules
        if (PlayerMovement)
            PlayerMovement.Tick(unscaledDeltaTime, deltaTime);
        if (PlayerMain)
            PlayerMain.Tick(unscaledDeltaTime, deltaTime);
        if (PlayerEffect)
            PlayerEffect.Tick(unscaledDeltaTime, deltaTime);
        if (PlayerCombat)
            PlayerCombat.Tick(unscaledDeltaTime, deltaTime);
    }
}

public enum PlayerModule
{
    Main,
    Movement,
    Combat
}
