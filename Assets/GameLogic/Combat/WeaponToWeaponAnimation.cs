using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToWeaponAnimation : MonoBehaviour
{
    private Weapon weapon;
    private void Start()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    private void AnimaitonFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }
    private void AnimationStartMovementTrigger()
    {
        weapon.AnimationStartMovementTrigger();
    }

    private void AnimationStopMovementTrigger()
    {
        weapon.AnimationStopMovementTrigger();
    }
}
