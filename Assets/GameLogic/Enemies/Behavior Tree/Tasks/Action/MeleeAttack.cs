using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : EnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        animator.SetTrigger("MeleeAttack");
       
    }

}
