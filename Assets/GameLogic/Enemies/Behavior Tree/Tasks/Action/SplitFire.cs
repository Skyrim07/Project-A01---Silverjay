using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitFire : EnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        animator.SetTrigger("SplitFire");

    }
}
