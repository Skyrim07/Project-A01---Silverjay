using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public class Weapon : MonoBehaviour
{
    public SO_WeaponData weaponData;
    public int attackCounter { get; private set; }

    public Transform weaponTF;
    public Transform baseTF;

    protected Vector3 weapon_oScale;
    protected Vector3 base_oScale;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;
    



    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        weapon_oScale = weaponTF.localScale;
        base_oScale = baseTF.localScale;

        gameObject.SetActive(false); //Disable Weapon when not using it

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }
    private void Update()
    {
        HandelTurn();
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

    public virtual void HandelTurn()
    {
        
        weaponTF.localScale = new Vector3((RuntimeData.player_FacingDirection > 0 ? 1 : -1) * weapon_oScale.x, weapon_oScale.y, weapon_oScale.z);
        baseTF.localScale = new Vector3((RuntimeData.player_FacingDirection > 0 ? 1 : -1) * base_oScale.x, base_oScale.y, base_oScale.z);
    }


}
