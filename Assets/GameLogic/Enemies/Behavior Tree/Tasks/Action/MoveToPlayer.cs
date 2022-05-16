using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveToPlayer : EnemyAction
{
    public float moveSpeed = 5f;
    public float attackRange = 2f;

    private float positionDiff;
    private float baseScaleX;


    public override void OnAwake()
    {
        base.OnAwake();
        baseScaleX = transform.localScale.x;
    }
    private void MovingtoPlayer()
    {
        
        var direction = RuntimeData.player_Position.x < transform.position.x ? -1 : 1;
        positionDiff = Mathf.Abs(transform.position.x - RuntimeData.player_Position.x);
        body.velocity = new Vector2(direction * moveSpeed, body.velocity.y);
    }

    public override TaskStatus OnUpdate()
    {

        MovingtoPlayer();
        FacingPlayer();

        return positionDiff > attackRange ? TaskStatus.Running : TaskStatus.Success;
        
    }

    private void FacingPlayer()
    {
        var scale = transform.localScale;
        scale.x = transform.position.x < RuntimeData.player_Position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
    }
}
