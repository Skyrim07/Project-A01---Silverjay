using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public class Weapon : MonoBehaviour
{
    public SO_WeaponData weaponData;
    public int attackCounter { get; private set; }

    protected Animator baseAnimator;
    protected Animator weaponAnimator;



    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false); //Disable Weapon when not using it

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true); //Enable Weapon when using it

        if (attackCounter >= weaponData.amountOfAttacks)//temp value
        {
            attackCounter = 0;
        }
        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);
        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);


        attackCounter++;


        gameObject.SetActive(false);//Disable Weapon when finishing using it
    }

    #region Animation Triggers

    public virtual void AnimationFinishTrigger()
    {
        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_FINISH);
        
    }

    public virtual void AnimationStartMovementTrigger()
    {
        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_START);
    }
    public virtual void AnimationStopMovementTrigger()
    {
        EventDispatcher.Dispatch(EventDispatcher.Player, EventRef.PLAYER_ON_ATTACK_MOVEMENT_END);
    }

    public virtual void AnimationActionTrigger()
    {

    }
    #endregion



}
