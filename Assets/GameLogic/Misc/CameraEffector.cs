using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using SKCell;
public sealed class CameraEffector : MonoBehaviour
{
    public float transitionTime = 0.5f;

    [Header("Target Camera Parameters")]
    public bool useOrthographicSize;
    public float orthographicSize;
    public bool useScreenX, useScreenY;
    public float screenX, screenY;

    [Header("Interaction with ConvStarter")]
    public ConvStarter convStarter;

    [HideInInspector]
    public CinemachineVirtualCamera cam;
    [HideInInspector]
    public CinemachineFramingTransposer transposer;

    private float oOrthoSize, oScreenX, oScreenY;

    private void Start()
    {
        if(convStarter != null)
        {
            convStarter.onConversationStart += Activate;
            convStarter.onConversationEnd += Deactivate;
        }

        CommonUtils.InvokeAction(1f, () =>
        {
            cam = CommonReference.instance.mainVC;
            transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();

            oOrthoSize = cam.m_Lens.OrthographicSize;
            oScreenX = transposer.m_ScreenX;
            oScreenY = transposer.m_ScreenY;
        });
    }
    public void Activate()
    {
        if (cam == null)
            cam = CommonReference.instance.mainVC;
        if(transposer == null)
            transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();

        float oSize = cam.m_Lens.OrthographicSize;
        CommonUtils.StartProcedure(SKCurve.QuadraticIn, transitionTime, (f) =>
        {
            if (useOrthographicSize)
            {
                cam.m_Lens.OrthographicSize = oSize + (orthographicSize- oSize) *f;
            }
            if (useScreenX)
            {
                transposer.m_ScreenX = oScreenX+(screenX-oScreenX)*f;
            }
            if (useScreenX)
            {
                transposer.m_ScreenY = oScreenY + (screenY - oScreenY) * f;
            }
        });
    }
    public void Deactivate()
    {
        float oSize = cam.m_Lens.OrthographicSize;
        CommonUtils.StartProcedure(SKCurve.QuadraticOut, transitionTime, (f) =>
        {
            if (useOrthographicSize)
            {
                cam.m_Lens.OrthographicSize = oOrthoSize + (oSize - oOrthoSize) * f;
            }
            if (useScreenX)
            {
                transposer.m_ScreenX = oScreenX + (screenX - oScreenX) * f;
            }
            if (useScreenX)
            {
                transposer.m_ScreenY = oScreenY + (screenY - oScreenY) * f;
            }
        });
    }
}
