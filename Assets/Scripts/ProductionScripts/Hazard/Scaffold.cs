using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    private void Awake()
    {
        OnWake();
    }

    private void OnEnable()
    {
        InitiateVariables();
    }

    private void Update()
    {        
        RunHazard(Mechanics(), cameraFocalPoint, hazardIndex);                                       
    }

    private float Mechanics()
    {
        float progress = -1 * Time.deltaTime; ///Change for the actual mechanics  

        if (CheckCursorState())
        {
            
            progress = 100;
            return progress;
        }               
        return progress;
    }
}