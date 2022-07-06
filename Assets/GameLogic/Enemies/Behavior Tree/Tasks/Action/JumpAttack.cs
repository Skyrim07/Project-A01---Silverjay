using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : EnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        animator.SetTrigger("JumpAttack");

        
    }

}
