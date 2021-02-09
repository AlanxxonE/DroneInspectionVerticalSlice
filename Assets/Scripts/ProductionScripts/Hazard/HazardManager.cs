using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    /// <summary>
    /// Class used to manage the initial interaction with hazards and the win/lose conditions and score associated with each subsequent hazard
    /// </summary>
    
    //References 
    public DroneUI droneUIScript;   //Reference to the drone UI script 
    private GameManager gameManager;
    private HazardMechanics hazardMechanics;

    //Variables
    [Tooltip("Maximum optimal distance to interact with a hazard")]
    public int optimalDistanceMax;
    [Tooltip("Minimum optimal distance to interact with a hazard")]
    public int optimalDistanceMin;
    [Tooltip("Maximum distance a hazard will be detected from")]
    public int maxDetectionDistance;

    private void Start()
    {
        
    }

    /// <summary>
    /// Method use to handle the win/lose condition of a hazard minigame, called on by the hazard progress method in the hazard mechanics class
    /// </summary>
    /// <param name="satisfaction"></param> satisfaction score gained or lost by winning/losing a minigame
    /// <param name="score"></param>  score achieved at the end of the game
    /// <param name="isFixed"></param>  boolean to determine if a hazard was fixed successfully or not
    public void FinishHazard(int satisfaction,int score, bool isFixed)
    {
        //hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);  //Sets the hazard inactive on completion, win or lose
        gameManager.droneController.droneRayCast.stopMovement = false;   //Allows the drone to move again
        gameManager.droneController.satisfactionValue += satisfaction; //Adds/subtracts score from the satisfaction meter depending on a win/lose
        LevelManager.scoreValue += score;  //Adds score to the end game score
        hazardRef.SetActive(false);  //Sets the hazard in question inactive

        if (isFixed)  //Only runs if a hazard minigame was completed successfuly. Changes the boolean value for each hazard in the level manager script if this is the caee.
        {
            switch (hazardRef.tag)
            {
                case "ScaffoldHazard":
                    LevelManager.isScaffoldFixed = true;
                    break;
                case "CraneHazard":
                    LevelManager.isCraneFixed = true;
                    break;
                default:
                    break;                    
            }
        }       
    }   
}
