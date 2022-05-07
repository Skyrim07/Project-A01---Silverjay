using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Item Information")]
    public InventoryItem itemInfo;

    public int collectibleCount = 1;

    [Header("References")]
    [SerializeField] private GameObject fullVisual, emptyVisual;
    [SerializeField] private Animator indicatorAnim;
    [SerializeField] ParticleSystem fx;
    public Transform panelPos;

    private bool isPlayerIn;
    private int currentCount = 0;

    private void Start()
    {
        fullVisual.SetActive(true);
        emptyVisual.SetActive(false);
    }
    private void Update()
    {
        if (isPlayerIn && currentCount<collectibleCount)
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
        if (collision.CompareTag(GlobalLibrary.G_PLAYER_TAG) && currentCount < collectibleCount)
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
        if (fx)
            fx.Play();
        InventoryManager.instance.OnCollectibleItemCollect(this);
    }
    /// <summary>
    /// Call this when no more things can be collected
    /// </summary>
    private void OnCollectDeplete()
    {
        fullVisual.SetActive(false);
        emptyVisual.SetActive(true);
        isPlayerIn = false;
        if (indicatorAnim)
    {
            indicatorAnim.SetBool("Appear", false);
        }
    }
}
