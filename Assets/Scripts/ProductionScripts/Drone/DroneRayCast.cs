using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroneRayCast : MonoBehaviour
{
    //Clasas Reference
    private DroneController droneController;

    //Hazard Interaction Variables
    private RaycastHit check;        //Holds reference for the object a raycast hit 
    [HideInInspector]public RaycastHit hit;          //Variable to determine what the raycast hit    
    [HideInInspector]public bool stopMovement = false;   //Bool to determine if the drone should stop moving

    private void Awake()
    {
        droneController = this.GetComponent<DroneController>();
    }

    private void Update()
    {
        RaycastDistanceCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && droneController.droneUI.artificialHorizonCircle.GetComponent<Image>().color == Color.green && droneController.firstPersonCam.activeSelf == true)
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
        if (check.collider != null && check.collider.CompareTag("Hazard"))
        {
            //If the hazard is within the optimal distance from the drone
            if (check.distance > droneController.gameManager.hazardManager.optimalDistanceMin && check.distance < droneController.gameManager.hazardManager.optimalDistanceMax)
            {
                droneController.droneUI.artificialHorizonCircle.GetComponent<Image>().color = Color.green;  //Sets the artificial horizon UI elements to green
            }
            //If the hazard is ouwith the optimal distance from the drone
            else if (check.distance < droneController.gameManager.hazardManager.maxDetectionDistance)
            {
                droneController.droneUI.artificialHorizonCircle.GetComponent<Image>().color = Color.red;   //Sets the artificial horizon UI elements to red
            }
        }

        //If the raycast hits nothing 
        else if (check.collider == null)
        {
            droneController.droneUI.artificialHorizonCircle.GetComponent<Image>().color = Color.black;  //Sets the artificial horizon UI elements to grey
        }
    }

    /// <summary>
    /// Method to shoot a raycast to get information about a hazard if mouse 1 is pressed when within the optimal range of the hazard
    /// </summary>
    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);  //Shoots out a raycast

        //If the raycast hits a hazard that is still in danger
        if (hit.collider != null && hit.collider.CompareTag("Hazard"))
        {            
            stopMovement = true;   //Stops the drone     
            droneController.gameManager.hazardManager.RunHazard(hit.collider.gameObject);
        }
    }
}
