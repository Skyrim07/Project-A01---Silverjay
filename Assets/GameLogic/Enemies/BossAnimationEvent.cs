using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossAnimationEvent : MonoBehaviour
{
    protected Rigidbody2D body;
    protected Animator animator;
    protected TestEnemyBlackBoard BB;
    protected BoxCollider2D Hitbox;

    [SerializeField] private float MeleeAttackDamage;

    

    private List<IDamageable> detectedDamageables = new List<IDamageable>();



    

    private void DealDamage()
    {
        if (detectedDamageables != null)
        {
            foreach (IDamageable item in detectedDamageables.ToList())
            {

                item.Damage(MeleeAttackDamage);
                Debug.Log(item);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        AddToDetected(collision);
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveFromDetected(collision);
        
    }

    private void Awake()
    {

        animator = gameObject.GetComponentInChildren<Animator>();
        BB = gameObject.GetComponentInParent<TestEnemyBlackBoard>();
        body = gameObject.GetComponentInParent<Rigidbody2D>();
        Hitbox = gameObject.GetComponent<BoxCollider2D>();
    }
    public void BackJumpStart()
    {
        var direction = RuntimeData.player_Position.x < transform.position.x ? -1 : 1;
        body.velocity = new Vector2(BB.facingDirection * 30f, 30f);
    }

    public void AttackStart()
    {
        Hitbox.enabled = true;
    }

    public void AttackEnd()
    {
        Hitbox.enabled = false;
    }


    public void AddToDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            detectedDamageables.Add(damageable);
        }
    }


    public void RemoveFromDetected(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            detectedDamageables.Remove(damageable);
        }
    }
}
