using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{    
    private void Awake()
    {
        this.GetComponent<MonoBehaviour>().enabled = false;        
    }

    private void Update()
    {
        RunHazard(Mechanics());
    }

    private float Mechanics()
    {
        float progress = 10 * Time.deltaTime; ///Change for the actual mechanics          
        return progress;
    }
}
