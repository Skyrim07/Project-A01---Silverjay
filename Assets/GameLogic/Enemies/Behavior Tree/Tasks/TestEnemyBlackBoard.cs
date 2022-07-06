using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBlackBoard : MonoBehaviour
{
    protected Rigidbody2D body;
    protected Animator animator;
    protected Destructable destructable;
    protected PlayerMovement player;
    public float facingDirection;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        destructable = GetComponent<Destructable>();
        animator = gameObject.GetComponentInChildren<Animator>();
        facingDirection = 1;
    }
    
    public void BackJumpStart()
    {
        var direction = RuntimeData.player_Position.x < transform.position.x ? -1 : 1;
        body.velocity = new Vector2(facingDirection * 30f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
