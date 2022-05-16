using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;


public class EnemyConditional : Conditional
{
    protected Rigidbody2D body;
    protected Animator animator;
    protected Destructable destructable;
    protected PlayerMovement player;

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody2D>();
        destructable = GetComponent<Destructable>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }
}
