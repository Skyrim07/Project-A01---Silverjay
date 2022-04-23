using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GroundAudioEffector : MonoBehaviour
{
    public GroundAudioType type;

    const float PLAYER_DELTA_THRESHOLD = 0.05f;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            // If player moves on this, play sound
            if (RuntimeData.player_HasMoveInput)
            {
                AudioManager.instance.SwitchGroundSound(type);
            }
            else
            {
                AudioManager.instance.SwitchGroundSound(GroundAudioType.None);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player") && RuntimeData.player_PositionDelta.y<=0)
        {
            AudioManager.instance.PlayGroundDropSound(type);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            AudioManager.instance.SwitchGroundSound(GroundAudioType.None);
        }
     }
}

public enum GroundAudioType
{
    None,
    Normal,
    Grass,
    Wood
}
