using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroneRayCast : MonoBehaviour
{

    //Hazard Interaction Variables
    public RaycastHit hit;          //Variable to determine what the raycast hit
    RaycastHit check;        //Holds reference for the object a raycast hit
    public Image horizonRectangle;     //Reference to the UI element of the rectangular border and the artificial horizon circle  
    public GameObject hazardRef;       //Reference to the current hazard being interacted with
    public bool stopMovement = false;   //Bool to determine if the drone should stop moving

    private void Update()
    {
        RaycastDistanceCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && horizonRectangle.color == Color.green)
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
            if (check.distance > check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMin && check.distance < check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMax /*&& check.collider.GetComponentInChildren<HazardMechanics>().checkEffect == true*/)
            {
                horizonRectangle.color = Color.green;  //Sets the artificial horizon UI elements to green
            }
            //If the hazard is ouwith the optimal distance from the drone
            else if (check.distance < check.collider.GetComponentInChildren<HazardMechanics>().MaximumDistance)
            {
                horizonRectangle.color = Color.red;   //Sets the artificial horizon UI elements to red
            }
        }

        //If the raycast hits nothing 
        else if (check.collider == null)
        {
            horizonRectangle.color = Color.gray;  //Sets the artificial horizon UI elements to grey
        }
    }

    /// <summary>
    /// Method to shoot a raycast to get information about a hazard if mouse 1 is pressed when within the optimal range of the hazard
    /// </summary>
    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);  //Shoots out a raycast

        //If the raycast hits a hazard that is still in danger
        if (hit.collider != null && hit.collider.GetComponentInChildren<HazardMechanics>() != null /*&& hit.collider.GetComponentInChildren<HazardMechanics>().checkEffect == true*/)
        {
            if (horizonRectangle.color == Color.green)
            {
                stopMovement = true;   //Stops the drone
                hazardRef = hit.collider.GetComponent<HazardMechanics>().hazardPopUpRef;  //Gets the reference to the minigame canvas pop up of the subsequent hazard
                hazardRef.SetActive(true);  //Sets the minigame canvas pop up active
                hit.collider.GetComponentInChildren<HazardMechanics>().hazardTag = hazardRef.tag;   //Gets the tag of the subsequent hazard               
            }
        }
    }
}
