using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class AnimTrigger : MonoBehaviour
{
    [Header("Anim pops when trigger enter")]
    public string targetTag = "Player";

    public bool randomTwoState;

    private Animator anim;
    private string state0;
    private void Awake()
    {
           anim = GetComponent<Animator>();
        state0 = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(targetTag))
        {
            anim.Play(state0);
            if (randomTwoState)
            {
                if (Random.value < 0.5f)
                {
                    anim.ResetTrigger("Pop");
                    anim.SetTrigger("Pop");
                }
                else
                {
                    anim.ResetTrigger("Pop2");
                    anim.SetTrigger("Pop2");
                }
            }
            else
            {
                anim.ResetTrigger("Pop");
                anim.SetTrigger("Pop");
            }
        }
        
    }
}
