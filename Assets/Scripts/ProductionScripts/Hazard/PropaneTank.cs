using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template/example class to be copied when adding a new hazard mechanic into the game
/// </summary>
public class PropaneTank : HazardMechanics
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

    //Add mechanics here
    private float Mechanics()
    {
        float progress = -0.1f * Time.deltaTime;  //Loses some progress over time until hazard is complete

        if (CheckCursorState()) //If check cursor state returns true, i.e. each target has been interacted with
        {
            progress = 100; //Adds 100 to progress (Completing minigame in RunHazard())
            return progress;
        }
        return progress;
    }
}