using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NPCWalk : MonoBehaviour
{
    const string MOVE_BOOL = "Move";

    public bool active = true;

    public float leftX, rightX;
    public bool isLeft = false;

    public float speed = 2;
    public float restInterval = 1.5f, walkInterval = 3f;

    private float timer;
    private bool isWalking = true;
    private float restWait, walkWait;

    [Header("Object to not flip")]
    public GameObject notFlipGO;
    private Vector3 notFlipoScale;

    [SerializeField] private Animator anim;

    private void Start()
    {
        leftX += transform.position.x;
        rightX+=transform.position.x;
        restWait = restInterval * Random.Range(0.5f, 1.5f);
        walkWait = walkInterval * Random.Range(0.5f, 1.5f);

        if (notFlipGO)
            notFlipoScale = notFlipGO.transform.localScale;
        transform.localScale = new Vector3(transform.localScale.x * (isLeft ? -1 : 1), transform.localScale.y, transform.localScale.z);

        if(notFlipGO)
            notFlipGO.transform.localScale = new Vector3(notFlipoScale.x * (isLeft ? -1 : 1), notFlipoScale.y, notFlipoScale.z);
        anim.SetBool(MOVE_BOOL, true);
    }

    private void FixedUpdate()
    {
        if (!active)
            return;

        if (leftX == 0)
            return;
        timer += Time.fixedDeltaTime;

        if (isWalking)
        {
            transform.Translate(new Vector3(speed * Time.fixedDeltaTime * (isLeft ? -1 : 1), 0, 0));
            if (timer >= walkWait)
            {
                walkWait = walkInterval * Random.Range(0.5f, 1.5f);
                timer = 0;
                isWalking = false;

                anim.SetBool(MOVE_BOOL, false);
            }
        }
        else
        {
            if (timer >= restWait)
            {
                float r = Random.Range(0.0f, 1.0f);
                if (r < 0.3f)
                {
                    isLeft = !isLeft;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    if (notFlipGO)
                        notFlipGO.transform.localScale = new Vector3(notFlipoScale.x *(transform.localScale.x<0?1:-1) , notFlipoScale.y, notFlipoScale.z);
                }

                restWait = restInterval * Random.Range(0.5f, 1.5f);
                timer = 0;
                isWalking = true;

                anim.SetBool(MOVE_BOOL, true);
            }
        }

        if ((transform.position.x < leftX && isLeft) || (transform.position.x > rightX && !isLeft))
        {
            isLeft = !isLeft;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (notFlipGO)
                notFlipGO.transform.localScale = new Vector3(notFlipoScale.x * (transform.localScale.x < 0 ? 1 : -1), notFlipoScale.y, notFlipoScale.z);
        }
    }

    public void Interrupt()
    {
        active = false;
        anim.SetBool(MOVE_BOOL, false);
    }

    public void Resume()
    {
        active = true;
        anim.SetBool(MOVE_BOOL, isWalking);
    }
}
