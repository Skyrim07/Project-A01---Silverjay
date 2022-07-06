using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : EnemyAction
{
    private float baseScaleX;


    public override void OnAwake()
    {
        base.OnAwake();
        baseScaleX = transform.localScale.x;
    }
    public override void OnStart()
    {
        FacingPlayer();
    }


    private void FacingPlayer()
    {
        var scale = transform.localScale;
        scale.x = transform.position.x < RuntimeData.player_Position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
        if (scale.x > 0)
        {
            blackBorad.facingDirection = 1;
        }
        else
        {
            blackBorad.facingDirection = -1;
        }
        
    }
}
