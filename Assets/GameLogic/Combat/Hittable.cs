using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] private bool disableHitEffect = false;
    [SerializeField] private bool disablehitColorChange = false;
    [SerializeField] private GameObject customHitEffect;
    [SerializeField] private AudioClip customHitSound;
    [SerializeField] private Material hitMaterial;

    private SpriteRenderer sprite;
    private Material defaultMaterial;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 workspace;

    public virtual void Damage(float amount)
    {
        anim.SetTrigger("damage");
        CinemachineShake.Instance.ShakeCamera(GlobalLibrary.ATKScreenShakeMagnitude, GlobalLibrary.ATKShakeTIME);
        CinemachineShake.Instance.HitPause(GlobalLibrary.ATKHitPauseTime);

        if (!disableHitEffect)
        {
            if (customHitEffect)
            {
                Instantiate(customHitEffect, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360f)));
            }
        }

        if(customHitSound != null)
        {
            //Play Audio Here.
        }

        if (!disablehitColorChange)
        {
            sprite.material = hitMaterial;
            StartCoroutine(ResetMaterial(0.1f));
        }
    }

    public virtual void Kockback(Vector2 angle, float strength, int direction)
    {
        SetVelocity(strength, angle, direction);
    }

    protected virtual void Awake()
    {
        sprite =GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        defaultMaterial = sprite.material;
    }

    protected void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = workspace;
    }

    private IEnumerator ResetMaterial(float delay)
    {
        yield return new WaitForSeconds(delay);
        sprite.material = defaultMaterial;
    }
}
