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
    RaycastHit check;        
    public Image uIRef;
    public bool stopMovement = false;
    public GameObject hazardRef;

    // Update is called once per frame
    void Update()
    {
        RaycastDistanceCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && uIRef.color == Color.green)
        {
            ShootRaycast();
        }       
    }

    public void RaycastDistanceCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out check, 100f);
        if (check.collider != null && check.collider.GetComponentInChildren<HazardMechanics>() != null)
        {
            if (check.distance > check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMin && check.distance < check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMax)
            {
                uIRef.color = Color.green;
            }
            else if(check.distance < check.collider.GetComponentInChildren<HazardMechanics>().MaximumDistance)
            {
                uIRef.color = Color.red;
            }
        }
        else if (check.collider == null)
        {
            uIRef.color = Color.gray;
        }
    }

    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);
        if (hit.collider != null && hit.collider.GetComponentInChildren<HazardMechanics>() != null)
        {
            if (uIRef.color == Color.green)
            {
                stopMovement = true;
                hazardRef = hit.collider.GetComponent<HazardMechanics>().hazardPopUpRef;
                hazardRef.SetActive(true);
                //hit.collider.GetComponentInChildren<HazardMechanics>().hazardTag = hazardRef.tag;
                hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", true);
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

        if (isFixed)  //Only runs if a hazard minigame was completed successfuly. Changes the boolean value for each hazard in the level manager scrfipt if this is the caee.
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
