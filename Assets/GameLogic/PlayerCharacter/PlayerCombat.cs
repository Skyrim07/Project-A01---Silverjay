using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class PlayerCombat : MonoBehaviour, IPlayerModule
{
    private Weapon weapon;

    private bool combatEnable;

    private bool combatState;

    private bool animationFinishTrigger;

    public TempWeaponInventory inventory { get; private set; }
    public void Initialize()
    {
        combatEnable = true;
        inventory = GetComponent<TempWeaponInventory>();
        SetWeapon(inventory.weapons[0]);//Use first weapon in the inventory for testing

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_FINISH, new SJEvent(() => //Check When the Attack Animation Finished
        {
            animationFinishTrigger = false;
        }));
    }

    public void SetState(bool isActive)
    {

    }

    public void Tick(float unscaledDeltaTime, float deltaTime)
    {
        HandleCombatInput();
        HandleCombatState();
    }

    public bool CheckCanAttack()
    {
        return true;
    }

    private void HandleCombatInput()
    {
        if (Input.GetMouseButtonDown(0) && combatEnable)
        {
            combatState = true;
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
        }
    }
}
