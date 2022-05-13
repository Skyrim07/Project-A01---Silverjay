using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

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
        foreach(IDamageable item in detectedDamageables)
        {
            item.Damage(details.damageAmount);
        }

        foreach (IKnockbackable item in detectedKnockbackables)
        {
            item.Kockback(details.knockbackAngle, details.knockbackStrength, RuntimeData.player_FacingDirection); 
        }  
    }

    public void AddToDetected(Collider2D collision)
    {
        
        IDamageable damageable = collision.GetComponent<IDamageable>();
        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if(damageable != null)
        {
            detectedDamageables.Add(damageable);
        }
        if (knockbackable != null)
        {
            detectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        
        IDamageable damageable = collision.GetComponent<IDamageable>();
        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if (damageable != null)
        {
            detectedDamageables.Remove(damageable);
        }
        if (knockbackable != null)
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }


}
