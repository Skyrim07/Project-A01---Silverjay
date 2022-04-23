using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public sealed class ParallexEffector : MonoBehaviour
{
    public Vector3 offset;
    [Header("1-Farest    15-Nearest")]
    public float nearFactor = 5;
    [Space]
    //public float effectRadius = 50;

    [Space]
    public bool randomMove = false;
    public float moveSpeed = 1;
    public float moveRange = 30;

    private Vector3 moveVector;
    private SpriteRenderer sr;
    private float oAlpha;
    private bool isFading, init;

    Vector3 oPos;
    private void Start()
    {
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_LOADED, new SJEvent(() =>
        {
            CommonUtils.InvokeAction(0.5f, () =>
            {
                Init();
            });
        }));
    }

    private void Init()
    {
        if (!this)
            return;

        oPos = transform.position;
        offset = -RuntimeData.camera_Transform.position * ((1.0f / nearFactor));
        offset.z = 0;
        sr = GetComponent<SpriteRenderer>();
        oAlpha = sr.color.a;

        moveSpeed *= Random.value * 0.5f + 0.5f;
        moveRange *= Random.value * 0.5f + 0.5f;
        if (Random.value < 0.5f)
            moveSpeed *= -1;

        init = true;
    }

    private void Update()
    {
        if (!init)
            return;

        Vector3 pos = RuntimeData.camera_Transform.position;

        transform.position = moveVector + offset + oPos + pos * ((1.0f / nearFactor));
        transform.SetPositionZ(oPos.z);

        if (randomMove)
        {
            moveVector.x += Time.deltaTime * moveSpeed;
            float dist = transform.position.SimpleDistance(oPos);
            if (dist > moveRange && !isFading)
            {
                isFading = true;
                CommonUtils.StartProcedure(SKCurve.LinearIn, 2f, (f) =>
                {
                    if (sr)
                    {
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, oAlpha * (1-f));
                    }
                }, (f) =>
                {
                    moveVector.x = 0;
                    CommonUtils.StartProcedure(SKCurve.LinearIn, 2f, (ff) =>
                    {
                        if (sr)
                        {
                            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, oAlpha * ff);
                        }
                    }, (ff) =>
                    {
                        isFading = false;
                    });
                });
            }
        }
    }
}
