using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackJump : EnemyAction
{
    public override void OnStart()
    {
        

        base.OnStart();

        animator.SetTrigger("BackJump");

        

    }
}
