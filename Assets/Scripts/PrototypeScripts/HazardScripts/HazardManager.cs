using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardManager : MonoBehaviour
{
    /// <summary>
    /// Class used to manage the initial interaction with hazards and the win/lose conditions and score associated with each subsequent hazard
    /// </summary>
    
    //References 
    public DroneUI droneUIScript;   //Reference to the drone UI script 

    //Hazard Interaction Variables
    public RaycastHit hit;          //Variable to determine what the raycast hit
    RaycastHit check;        //Holds reference for the object a raycast hit
    public Image uIRef;     //Reference to the UI element of the rectangular border and the artificial horizon circle
    public bool stopMovement = false;   //Bool to determine if the drone should stop moving
    public GameObject hazardRef;       //Reference to the current hazard being interacted with

    // Update is called once per frame
    void Update()
    {
        RaycastDistanceCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && uIRef.color == Color.green)
        {
            ShootRaycast();
        }       
    }

    /// <summary>
    /// Method to determine if the drone is looking at a hazard and within range to detect it as such
    /// </summary>
    public void RaycastDistanceCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out check, 100f); //Sends out a raycast 100m in front of the drone each frame

        //If the raycast hits a hazard
        if (check.collider != null && check.collider.GetComponentInChildren<HazardMechanics>() != null)
        {
            //If the hazard is within the optimal distance from the drone
            if (check.distance > check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMin && check.distance < check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMax && check.collider.GetComponentInChildren<HazardMechanics>().checkEffect == true)
            {
                uIRef.color = Color.green;  //Sets the artificial horizon UI elements to green
            }
            //If the hazard is ouwith the optimal distance from the drone
            else if (check.distance < check.collider.GetComponentInChildren<HazardMechanics>().MaximumDistance)
            {
                uIRef.color = Color.red;   //Sets the artificial horizon UI elements to red
            }
        }

        //If the raycast hits nothing 
        else if (check.collider == null)
        {
            uIRef.color = Color.gray;  //Sets the artificial horizon UI elements to grey
        }
    }

    /// <summary>
    /// Method to shoot a raycast to get information about a hazard if mouse 1 is pressed when within the optimal range of the hazard
    /// </summary>
    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);  //Shoots out a raycast

        //If the raycast hits a hazard that is still in danger
        if (hit.collider != null && hit.collider.GetComponentInChildren<HazardMechanics>() != null && hit.collider.GetComponentInChildren<HazardMechanics>().checkEffect == true)
        {
            if (uIRef.color == Color.green)
            {
                stopMovement = true;   //Stops the drone
                hit.collider.GetComponent<HazardEffect>().particleClone.GetComponent<ParticleSystem>().Pause();  //Pause the particle effect of the hazard
                hazardRef = hit.collider.GetComponent<HazardMechanics>().hazardPopUpRef;  //Gets the reference to the minigame canvas pop up of the subsequent hazard
                hazardRef.SetActive(true);  //Sets the minigame canvas pop up active
                hit.collider.GetComponentInChildren<HazardMechanics>().hazardTag = hazardRef.tag;   //Gets the tag of the subsequent hazard
                hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", true);   //Sets the hazards run state equal to true
            }
        }
    }

    /// <summary>
    /// Method use to handle the win/lose condition of a hazard minigame, called on by the hazard progress method in the hazard mechanics class
    /// </summary>
    /// <param name="satisfaction"></param> satisfaction score gained or lost by winning/losing a minigame
    /// <param name="score"></param>  score achieved at the end of the game
    /// <param name="isFixed"></param>  boolean to determine if a hazard was fixed successfully or not
    public void FinishHazard(int satisfaction,int score, bool isFixed)
    {
        hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);  //Sets the hazard inactive on completion, win or lose
        stopMovement = false;   //Allows the drone to move again
        droneUIScript.satisfactionValue += satisfaction; //Adds/subtracts score from the satisfaction meter depending on a win/lose
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
