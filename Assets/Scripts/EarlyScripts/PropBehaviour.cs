using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : MonoBehaviour
{
    private float rotateSpeed = 360f;
    private float speedMultiplier = 10f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotateSpeed * Time.deltaTime * speedMultiplier, 0);     
    }
}
