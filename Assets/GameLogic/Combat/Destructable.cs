using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : Hittable
{
    public float health = 100;
    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentHealth = health;
        Invincible = false;
    }

    public override void Damage(float amount)
    {
        if (CurrentHealth <= 0 || Invincible)
        {
            return;
        }
        

        DealDamage(amount);

        base.Damage(amount);
    }

    
    public void DealDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
