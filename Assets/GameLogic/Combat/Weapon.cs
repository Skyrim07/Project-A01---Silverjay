using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SO_WeaponData weaponData;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected int attackCounter;

    protected virtual void Start()
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

        if (attackCounter >= weaponData.movementSpeed.Length)//temp value
        {
            attackCounter = 0;
        }
        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);
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

    }
    public virtual void AnimationStopMovementTrigger()
    {

    }
    #endregion



}
