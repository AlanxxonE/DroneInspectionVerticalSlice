using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    private void Awake()
    {
        OnWake(); //Calls the OnWake() method
        hazardManager.gameManager.tutorialManager.scaffoldIndex = hazardIndex;
    }

    private void OnEnable()
    {
        InitiateVariables(); //Initiates variables
    }

    private void Update()
    {        
        RunHazard(Mechanics(), cameraFocalPoint, hazardIndex);                                
    }

    /// <summary>
    /// Mechanics of this hazard
    /// </summary>
    /// <returns></returns>
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