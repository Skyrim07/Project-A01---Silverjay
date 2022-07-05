using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : EnemyAction
{
    public override void OnStart()
    {
        base.OnStart();
        animator.SetBool("isWake", true);
        Debug.Log("1");
    }
}
