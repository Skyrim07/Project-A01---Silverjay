using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class AudioManager : MonoSingleton<AudioManager>
{
    const string BGM_PREFIX = "BGM_";
    const string GROUND_PREFIX = "Ground";
    const int BGM_MAX = 5;

    public Dictionary<GroundAudioType, AudioSource> groundAudios = new Dictionary<GroundAudioType, AudioSource>();
    private AudioSource lastGroundAudio;

    [SerializeField] private Transform bgmCT;

    private void Start()
    {
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.CM_ON_SCENE_EXIT, new SJEvent(() =>
        {
            ClearBGM();
            SwitchGroundSound(GroundAudioType.None);
        }));

        EventDispatcher.AddListener(EventDispatcher.Player, EventRef.PLAYER_ON_INPUT_JUMP, new SJEvent(() =>
        {
            SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_PLAYER_JUMP);
        }));

        for (int i = 0; i < GlobalLibrary.AUDIO_GROUND_FILE_NAME.Count; i++)
        {
            groundAudios.Add((GroundAudioType)i,
            SKAudioManager.instance.PlayIdentifiableSound(GlobalLibrary.AUDIO_GROUND_FILE_NAME[(GroundAudioType)i], GROUND_PREFIX+i, true, 0, 0));
        }


    }
    public void SwitchGroundSound(GroundAudioType type)
    {
        //Dash Effect
        if (lastGroundAudio != null)
        {
            if (RuntimeData.player_IsDashing && lastGroundAudio.pitch != GlobalLibrary.G_PLAYER_FAST_RUN_AUDIO_PITCH_BONUS)
            {
                lastGroundAudio.pitch = GlobalLibrary.G_PLAYER_FAST_RUN_AUDIO_PITCH_BONUS;
            }
            if (!RuntimeData.player_IsDashing && lastGroundAudio.pitch == GlobalLibrary.G_PLAYER_FAST_RUN_AUDIO_PITCH_BONUS)
            {
                lastGroundAudio.pitch = 1.0f;
            }
        }


        if (lastGroundAudio == groundAudios[type])
        {
            return;
        }
        if (lastGroundAudio != null)
        {
            DimAudio(lastGroundAudio, 0, 0.1f);
        }
        if (type != GroundAudioType.None)
        {
            DimAudio(groundAudios[type], 1.0f, 0.1f);
        }
        lastGroundAudio = groundAudios[type];
    }

    public void PlayGroundDropSound(GroundAudioType type)
    {
        if (type != GroundAudioType.None)
        {
            SKAudioManager.instance.PlaySound(GlobalLibrary.AUDIO_GROUND_DROP_FILE_NAME[type], null, false, 0.8f, Random.Range(0.8f, 1.2f), 0f);
        }
    }
    public void DimAudioDirect(AudioSource audio, float targetVolume)
    {
        if (audio == null)
            return;
        audio.volume = targetVolume;
    }
    public void DimAudio(AudioSource audio, float targetVolume, float time)
    {
        if (audio == null)
            return;
        
        float oVolume = audio.volume;
        CommonUtils.StartProcedure(SKCurve.LinearIn, time, (f) =>
        {
            audio.volume = oVolume+(targetVolume-oVolume)*f;
        });
    }
    public void ClearBGM()
    {
        for (int i = 0; i < BGM_MAX; i++)
        {
            SKAudioManager.instance.StopIdentifiableSound(BGM_PREFIX + i);
        }
    }
    public void LoadBGM(string[] bgms)
    {
        ClearBGM();
        if (bgms == null || bgms.Length == 0)
            return;
        for (int i = 0; i < bgms.Length; i++)
        {
            SKAudioManager.instance.PlayIdentifiableSound(bgms[i], BGM_PREFIX + i, true, 0.7f);
        }
    }
}
