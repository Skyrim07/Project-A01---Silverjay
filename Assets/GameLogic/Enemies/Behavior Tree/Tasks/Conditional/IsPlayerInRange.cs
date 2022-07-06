using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsPlayerInRange : EnemyConditional
{
    public float EnemyDetectedRange;

    
    public override TaskStatus OnUpdate()
    {
     
        return Mathf.Abs(transform.position.x - RuntimeData.player_Position.x) > EnemyDetectedRange ? TaskStatus.Failure : TaskStatus.Success;
    }


}
