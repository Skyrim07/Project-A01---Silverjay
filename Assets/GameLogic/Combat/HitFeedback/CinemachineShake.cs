using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{

    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera Playercamera;

    private float shakeTimer;
    private float shakeTtimerTotal;
    private float startingIntensity;

    private bool isPause;

    private void Awake()
    {
        isPause = false;
        Instance = this;
        Playercamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin Perlin = Playercamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
        startingIntensity = intensity;
        shakeTtimerTotal = time;
    }

    private void Update()
    {
        shakeTimer -= Time.deltaTime;
        if (shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin Perlin = Playercamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - shakeTimer / shakeTtimerTotal));
        }

    }


    public void HitPause(int duration)
    {
        if (!isPause)
        {
            StartCoroutine(Pause(duration));
        }

    }
    IEnumerator Pause(int duration)
    {

            isPause = true;
            float pauseTime = duration / 60f;
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }

            yield return new WaitForSecondsRealtime(pauseTime);
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                isPause = false;
            }
            else
            {
                Time.timeScale = 1;
                isPause = false;
            }
 
    }
}
