using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMechanics : MonoBehaviour
{
    [HideInInspector]public HazardManager hazardManager;
    
    public void RunHazard(string hazardName, float sliderProgress)
    {
        HazardProgress(Score.GetScore(hazardName).satisfaction, Score.GetScore(hazardName).dissatisfaction, Score.GetScore(hazardName).score, hazardName, sliderProgress);        
    }

    /// <summary>
    /// Method to keep track of progress of hazard
    /// </summary>
    /// <param name="satisfaction"></param>  satifaction score gained from winning the minigame
    /// <param name="dissatisfaction"></param> satisfaction score lost from losing the minigame
    /// <param name="score"></param>  score added to the score at the end of the game
    private void HazardProgress(int satisfaction, int dissatisfaction, int score, string hazardName, float sliderProgress)
    {
        if (hazardManager.hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
        {
            hazardManager.FinishHazard(satisfaction, score, true, hazardManager.currentHazardScript, hazardName);
        }
        else if (hazardManager.hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
        {
            hazardManager.FinishHazard(dissatisfaction, 0, false, hazardManager.currentHazardScript, hazardName);
        }

        hazardManager.hazardSlider.value += sliderProgress - (hazardManager.hazardProgressDropRate * Time.deltaTime);  //Sets the rate at which the hazard timer counts down ////Don't move this
    }
}
