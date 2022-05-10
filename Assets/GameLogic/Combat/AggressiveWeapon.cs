using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    private List<IDamageable> detectedDamageable = new List<IDamageable>();

    protected override void Awake()
    {
        base.Awake();
        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong Data for the Weapon!");
        }
        
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        CheckMelleeAttack();
    }

    private void CheckMelleeAttack()
    {
        AttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];
        foreach(IDamageable item in detectedDamageable)
        {
            item.Damage(details.damageAmount);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if(damageable != null)
        {
            
            detectedDamageable.Add(damageable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            detectedDamageable.Remove(damageable);
        }
    }


}
