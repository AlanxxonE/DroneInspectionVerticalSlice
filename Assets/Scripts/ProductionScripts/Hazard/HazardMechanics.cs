using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMechanics : MonoBehaviour
{
    [HideInInspector]public HazardManager hazardManager;

    public void RunHazard(float sliderProgress)
    {
        if (hazardManager.hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
        {
            hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).satisfaction, Score.GetScore(hazardManager.hazardName).score, true);
        }
        else if (hazardManager.hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
        {
            hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).dissatisfaction, 0, false);
        }

        hazardManager.hazardSlider.value += sliderProgress - (hazardManager.hazardProgressDropRate * Time.deltaTime);  //Sets the rate at which the hazard timer counts down ////Don't move this
    }
}