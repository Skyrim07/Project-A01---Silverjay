using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour,IDamageable, IKnockbackable
{
    [SerializeField] private GameObject hitParticles;
    private Rigidbody2D rb;
    private Vector2 knockbackSpeed;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }
    public void Damage(float amount)
    {
        Debug.Log(amount + " Damage taken");
        anim.SetTrigger("damage");
        Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
    }

    public void Kockback(Vector2 angle, float strength, int direction)
    {
        SetVelocity(strength, angle, direction);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        knockbackSpeed.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = knockbackSpeed;
    }


}
