using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardManager : MonoBehaviour
{
    /// <summary>
    /// Class used to manage the initial interaction with hazards and the win/lose conditions and score associated with each subsequent hazard
    /// </summary>

    //Class References 
    [Header("Class References")]
    public GameManager gameManager;
    //public HazardMechanics hazardMechanics;

    //General References
    [Header("General References")]
    [Tooltip("Reference to the hazard slider parent object")]
    public GameObject hazardSliderRef;
    [HideInInspector] public Slider hazardSlider;
    [HideInInspector] public MonoBehaviour currentHazardScript = null;

    //Hazard Mechanics Variables
    [Header("Hazard Mechanics Variables")]
    [Tooltip("Maximum optimal distance to interact with a hazard")]
    public int optimalDistanceMax;
    [Tooltip("Minimum optimal distance to interact with a hazard")]
    public int optimalDistanceMin;
    [Tooltip("Maximum distance a hazard will be detected from")]
    public int maxDetectionDistance;
    [Tooltip("Integer to set how fast the hazard loses progress, i.e. how many ticks per second")]
    public int hazardProgressDropRate;
    [Range(0,100)]
    [Tooltip("Initial value for the hazard slider")]
    public int hazardSliderInitialValue;

    private void Awake()
    {
        hazardSlider = hazardSliderRef.GetComponent<Slider>();
        hazardSliderRef.SetActive(false);
    }
    
    public void RunHazard(GameObject hazardRef)
    {
        currentHazardScript = hazardRef.GetComponent<MonoBehaviour>();
        currentHazardScript.enabled = true;
        hazardSliderRef.SetActive(true);
    }

    /// <summary>
    /// Method to handle to progression of a hazard and manage it's win/lose states
    /// </summary>
    /// <param name="satisfaction"></param>  satifaction score gained from winning the minigame
    /// <param name="dissatisfaction"></param> satisfaction score lost from losing the minigame
    /// <param name="score"></param>  score added to the score at the end of the game
    public void HazardProgress(int satisfaction, int dissatisfaction, int score)
    {
        if (hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
        {            
            FinishHazard(satisfaction, score, true, currentHazardScript);            
        }
        else if (hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
        {
            FinishHazard(dissatisfaction, 0, false, currentHazardScript);
        }

        hazardSlider.value -= Time.deltaTime * hazardProgressDropRate;  //Sets the rate at which the hazard timer counts down ////Don't move this
    }

    /// <summary>
    /// Method use to handle the win/lose condition of a hazard minigame, called on by the hazard progress method in the hazard mechanics class
    /// </summary>
    /// <param name="satisfaction"></param> satisfaction score gained or lost by winning/losing a minigame
    /// <param name="score"></param>  score achieved at the end of the game
    /// <param name="isFixed"></param>  boolean to determine if a hazard was fixed successfully or not
    public void FinishHazard(int satisfaction,int score, bool isFixed, MonoBehaviour currentHazardScript)
    {      
        gameManager.droneController.droneRayCast.stopMovement = false;   //Allows the drone to move again
        gameManager.droneController.satisfactionValue += satisfaction; //Adds/subtracts score from the satisfaction meter depending on a win/lose
        LevelManager.scoreValue += score;  //Adds score to the end game score

        //if (isFixed)  //Only runs if a hazard minigame was completed successfuly. Changes the boolean value for each hazard in the level manager script if this is the caee.
        //{
        //    switch (hazardRef.tag)
        //    {
        //        case "ScaffoldHazard":
        //            LevelManager.isScaffoldFixed = true;
        //            break;
        //        case "CraneHazard":
        //            LevelManager.isCraneFixed = true;
        //            break;
        //        default:
        //            break;                    
        //    }
        //}
        hazardSlider.value = hazardSliderInitialValue;
        hazardSliderRef.SetActive(false);
        
        currentHazardScript.enabled = false;
        currentHazardScript = null;
    }   
}
