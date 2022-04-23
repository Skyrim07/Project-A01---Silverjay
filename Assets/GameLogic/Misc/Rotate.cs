using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class Rotate : MonoBehaviour
{
    public float speed = 1;

    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
