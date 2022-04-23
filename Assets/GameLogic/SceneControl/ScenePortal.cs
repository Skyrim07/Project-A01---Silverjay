using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class ScenePortal : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = true;

    public ScenePortalType type;
    public SceneInfo sceneInfo;

    [Header("Audio")]
    public AudioClip[] clips;

    private bool isPlayerIn;
    [Header("References")]
    [SerializeField] private Animator indicator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
            return;
        
        if (collision.gameObject.tag.Equals("Player"))
        {
            isPlayerIn = true;
            if (type == ScenePortalType.Instant)
            {
                PlayAudio();
                SceneController.instance.LoadSceneAsset(sceneInfo);
                isActive = false;
            }
            else if(type == ScenePortalType.WaitInput)
            {
                indicator.SetBool("Appear", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActive)
            return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            isPlayerIn = false;
            if (type == ScenePortalType.WaitInput)
            {
                indicator.SetBool("Appear", false);
            }
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (type == ScenePortalType.WaitInput)
        {
            if (isPlayerIn)
            {
                if (Input.GetKeyDown(GlobalLibrary.INPUT_ENTER_SCENE_KEYCODE))
                {
                    PlayAudio();
                    SceneController.instance.LoadSceneAsset(sceneInfo);
                    isActive = false;
                }
            }
        }
    }

    private void PlayAudio()
    {
        if (clips.Length == 0)
            return;

        AudioClip c = clips[Random.Range(0, clips.Length)];
        SKAudioManager.instance.PlaySound(c.name, null, false, 1, 1, 0);
    }
}

public enum ScenePortalType
{
    Instant,
    WaitInput
}
