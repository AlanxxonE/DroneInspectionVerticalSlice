using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    private void Awake()
    {
        GetComponent<MonoBehaviour>().enabled = false;
        target = transform.Find("Target");
    }

    private void OnEnable()
    {
        checkCameraPosition = true;
    }

    private void Update()
    {           
        RunHazard(Mechanics(), target);                                       
    }

    private float Mechanics()
    {
        float progress = 10 * Time.deltaTime; ///Change for the actual mechanics          
        return progress;
    }
}