using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class ConvStarter : MonoBehaviour
{
    [Header("Designated conversation")]
    public int conversationID; //Single conv

    [Header("Conversations (random one)")]
    public int[] randomConversations; //Random one conv from a list

    [Header("Visual settings")]
    public Transform panelPosition;
    public float panelScale = 1;
    public TextAlignmentOptions textAlignment = TextAlignmentOptions.TopLeft;
    public Animator anim;

    [Header("Voices")]
    public AudioClip[] voices;

    [Header("Interaction with NPC Walk")]
    public NPCWalk npcWalk;

    [Header("References")]
    [SerializeField] Animator indicatorAnim;

    [HideInInspector]
    public bool isPlayerIn = false, active = false;

    public Action onConversationStart, onConversationEnd;


    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(GlobalLibrary.INPUT_INTERACT_KEYCODE))
            {
                ConversationManager.instance.panelPosition = panelPosition == null ? transform.position : panelPosition.position;
                ConversationManager.instance.panelScale = panelScale;
                if (randomConversations.Length > 0)
                {
                    conversationID = randomConversations[UnityEngine.Random.Range(0,randomConversations.Length)];
                }
                if(onConversationStart != null)
                onConversationStart.Invoke();
                ConversationManager.instance.StartConversation(conversationID, 0, this);
                indicatorAnim.SetBool("Appear", false);
                if (anim)
                {
                    anim.SetTrigger("Pop");
                }
                if(npcWalk != null)
                {
                    npcWalk.Interrupt();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Activate();
        }
    }

    public void Activate()
    {
        isPlayerIn = true;
        active = true;
        indicatorAnim.SetBool("Appear", true);

        if (npcWalk)
            npcWalk.Resume();
    }

    public void OnConversationEnd()
    {
        if (onConversationEnd != null)
            onConversationEnd.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isPlayerIn = false;
            active = false;
            indicatorAnim.SetBool("Appear", false);

            if(ConversationManager.instance.conv!=null && ConversationManager.instance.conv.id == conversationID)
            {
                OnConversationEnd();
                ConversationManager.instance.EndConversation();

                if (npcWalk != null)
                {
                    npcWalk.Resume();
                }
            } 
        }
    }
}
