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
        RunHazard(Mechanics(), target, index);                                       
    }

    public float Mechanics()
    {
        if (CheckCursorState() == "Clicked")
        {
            float interactedProgress = 100;
            return interactedProgress;
        }

        float progress = -1 * Time.deltaTime; ///Change for the actual mechanics          
        return progress;
    }
}