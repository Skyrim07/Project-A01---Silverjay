using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float positionDiff;

    // Update is called once per frame
    void Update()
    {
        positionDiff = Mathf.Abs(transform.position.x - RuntimeData.player_Position.x);
        
    }
}
