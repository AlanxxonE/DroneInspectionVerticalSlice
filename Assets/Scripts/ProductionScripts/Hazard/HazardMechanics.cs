using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMechanics : MonoBehaviour
{
    //Class references
    [HideInInspector]public HazardManager hazardManager;

    //General variables
    protected Transform target;    //Transform of the target game object, as it's protected it will be unique for any class that derives from hazard mechanics
    protected bool checkCameraPosition = false;   //Boolean used to determine if the CheckCameraPosition() method should run

    /// <summary>
    /// Sets current script inactive and sets it's unique target transform
    /// </summary>
    /// <param name="nameOfTargetGameObject"></param>
    protected void OnWake(string nameOfTargetGameObject)
    {
        GetComponent<MonoBehaviour>().enabled = false;
        target = transform.Find(nameOfTargetGameObject);
    }

    /// <summary>
    /// Initiates unique variables for the script the calls this method
    /// </summary>
    protected void InitiateVariables()
    {
        checkCameraPosition = true;
    }

    protected void RunHazard(float sliderProgress, Transform target)
    {
        if (checkCameraPosition)
        {
            if (CheckCameraPosition(target))
            {
                checkCameraPosition = false;
                hazardManager.gameManager.droneController.droneCamera.interpolationTime = 0;
            }
        }

        else
        {
            if (hazardManager.hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
            {
                hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).satisfaction, Score.GetScore(hazardManager.hazardName).score, true, target);
            }
            else if (hazardManager.hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
            {
                hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).dissatisfaction, 0, false, target);
            }

            hazardManager.hazardSlider.value += sliderProgress - (hazardManager.hazardProgressDropRate * Time.deltaTime);  //Sets the rate at which the hazard timer counts down ////Don't move this
        }        
    }

    private bool CheckCameraPosition(Transform target)
    {        
        if(checkCameraPosition) ///maybe take out
        {
            if (hazardManager.gameManager.droneController.droneCamera.FocusOnHazard(target, false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}