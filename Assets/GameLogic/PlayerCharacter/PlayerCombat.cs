using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class PlayerCombat : MonoBehaviour, IPlayerModule
{
    private Weapon weapon;
    private PlayerMovement playerMovement;

    private bool combatEnable;

    private bool combatState;

    private bool animationFinishTrigger;

    private float velocityToSet;

    private bool setVelocity;

    private CapsuleCollider2D collider;

    public TempWeaponInventory inventory { get; private set; }
    public void Initialize()
    {
        collider = GetComponent<CapsuleCollider2D>();

        combatEnable = true;
        inventory = GetComponent<TempWeaponInventory>();
        SetWeapon(inventory.weapons[0]);//Use first weapon in the inventory for testing
        setVelocity = false;
        playerMovement = GetComponent<PlayerMovement>();

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_FINISH, new SJEvent(() => //Check When the Attack Animation Finished
        {
            
            animationFinishTrigger = true;
        }));

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_START, new SJEvent(() => 
        {
            
            velocityToSet = weapon.weaponData.movementSpeed[weapon.attackCounter]; //set the velocity by current attack Counter
            
            setVelocity = true;
        }));

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_END, new SJEvent(() => 
        {
            
            velocityToSet = 0f;

        }));
    }

    public void SetState(bool isActive)
    {

    }

    public void Tick(float unscaledDeltaTime, float deltaTime)
    {
        HandleCombatInput();
        HandleCombatState();
        HandleAttackMovement();
    }

    public bool CheckCanAttack()
    {
        return true;
    }

    private void HandleCombatInput()
    {
        if (Input.GetKeyDown(GlobalLibrary.INPUT_ATTACK_KEYCODE) && combatEnable)
        {
            combatState = true;
            weapon.EnterWeapon();
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    private void HandleCombatState()
    {
        if (combatState && animationFinishTrigger)
        {
            
            weapon.ExitWeapon();
            combatState = false;
            animationFinishTrigger = false;
            setVelocity = false;
        }
    }

    private void HandleAttackMovement()
    { 
        if (setVelocity) 
        {
            playerMovement.SetPlayerVelocity(velocityToSet);
        }
    }

    public bool GetCombatState()
    {
        return combatState;
    }

    
    
}
