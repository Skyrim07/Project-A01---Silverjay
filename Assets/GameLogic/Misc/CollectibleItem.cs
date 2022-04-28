using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Item Information")]
    public InventoryItem itemInfo;

    public int collectibleCount = 1;

    [Header("References")]
    [SerializeField] private Animator indicatorAnim;

    private bool isPlayerIn;
    private int currentCount = 0;

    private void Update()
    {
        if (isPlayerIn)
        {
            if (Input.GetKeyDown(GlobalLibrary.INPUT_INTERACT_KEYCODE))
            {
                OnCollect();
                currentCount++;
                if (currentCount >= collectibleCount)
                {
                    OnCollectDeplete();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalLibrary.G_PLAYER_TAG))
        {
            isPlayerIn = true;
            if (indicatorAnim)
            {
                indicatorAnim.SetBool("Appear", true);
            }
        }   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalLibrary.G_PLAYER_TAG))
        {
            isPlayerIn = false;
            if (indicatorAnim)
            {
                indicatorAnim.SetBool("Appear", false);
            }
        }
    }
    /// <summary>
    /// Call this when successfully collects an item
    /// </summary>
    private void OnCollect()
    {
        InventoryManager.instance.OnCollectibleItemCollect(this);
    }
    /// <summary>
    /// Call this when no more things can be collected
    /// </summary>
    private void OnCollectDeplete()
    {

    }
}
