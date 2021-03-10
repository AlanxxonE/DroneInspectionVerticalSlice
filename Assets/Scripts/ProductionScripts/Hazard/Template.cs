﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template/example class to be copied when adding a new hazard mechanic into the game
/// </summary>
public class Template : HazardMechanics     //Needs to inherit from HazardMechanics
{
    ///////Don't remove///////
    private void Awake()
    {
        OnWake();  //Calls the OnWake() method
    }

    ///////Don't remove///////
    private void OnEnable()
    {
        InitiateVariables();     //Can be used to initiate variables, currently just for the protected bool of checkCameraPosition
    }

    ///////Don't remove///////
    private void Update()
    {
        RunHazard(Mechanics(), cameraFocalPoint, hazardIndex);   //Calls the RunHazard() method in hazard mechanics which handles all the mechanics
    }

    //Add mechanics here
    private float Mechanics()
    {
        float progress = 10 * Time.deltaTime; ///Change for the actual mechanics  ///Number can be whatever we like, 10 * Time.deltaTime is simply a placeholder for testing purposes        
        return progress;   //Must return this value
    }
}

/// IMPORTANT - PLEASE READ ///
/*Things to add to other scripts after creating class:

Score script: 
1. Add is('this hazard')Fixed boolean to script variables and to the IF and SWITCH statements of the SetFixedBoolean() method. Note, add in the same format as the others.
2. Add 'hazardName' to the GetScore() method's switch statement and it's subsequent values. Note, 'hazardName' must be the same as this class' name.

HazardManager script:
1. Add new Vector2 for the optimal range for this hazard to the hazard manager variables.
2. Add new return case for this Vector2 in the GetOptimalRange()method. Note, 'hazardName' must be the same as this class' name.*/