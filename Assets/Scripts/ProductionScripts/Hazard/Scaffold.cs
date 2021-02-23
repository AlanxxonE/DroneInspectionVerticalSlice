using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    public Transform target;

    private void Awake()
    {
        this.GetComponent<MonoBehaviour>().enabled = false;        
    }

    private void Update()
    {
        if(CheckCameraPosition(target))
        {
            RunHazard(Mechanics());
        }
        else if (!CheckCameraPosition(target))
        {
            CheckCameraPosition(target);
        }
    }

    private float Mechanics()
    {
        float progress = 10 * Time.deltaTime; ///Change for the actual mechanics          
        return progress;
    }
}
