using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class PlayerEffect : MonoBehaviour, IPlayerModule
{
    public bool active = true;

    //References
    public ParticleSystem fx_Dash_L, fx_Dash_R;
    public void Initialize()
    {
        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_INPUT_JUMP, new SJEvent(() =>
        {
            PlayEffect(CommonReference.instance.fx_Jump, PlayerModel.Instance.playerPos_Foot.position);
        }));
        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_START_DASH, new SJEvent(() =>
        {
            SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_PLAYER_DASH);
        }));
    }

    public void SetState(bool isActive)
    {
        active = isActive;
    }

    public void Tick(float unscaledDeltaTime, float deltaTime)
    {
        HandleDashEffect();
    }

    private void HandleDashEffect()
    {
        if (RuntimeData.player_IsDashing)
        {
            if(RuntimeData.player_PositionDelta.x > 0)
            {
                if (!fx_Dash_L.isPlaying)
                {
                    fx_Dash_L.Play();
                }
                if (fx_Dash_R.isPlaying)
                {
                    fx_Dash_R.Stop();
                }
            }
            else if (RuntimeData.player_PositionDelta.x < 0)
            {
                if (!fx_Dash_R.isPlaying)
                {
                    fx_Dash_R.Play();
                }
                if (fx_Dash_L.isPlaying)
                {
                    fx_Dash_L.Stop();
                }
            }
        }
        else
        {
            if (fx_Dash_R.isPlaying)
            {
                fx_Dash_R.Stop();
            }
            if (fx_Dash_L.isPlaying)
            {
                fx_Dash_L.Stop();
            }
        }
    }
    public void PlayEffect(GameObject effect, Vector3 position, float releaseTime = 2.0f)
    {
        if (!effect)
            return;
       GameObject fx = CommonUtils.SpawnObject(effect);
        if (!fx)
        {
            return;
        }
        fx.transform.position = position;
        CommonUtils.InvokeAction(releaseTime, () => CommonUtils.ReleaseObject(fx));
    }
}
