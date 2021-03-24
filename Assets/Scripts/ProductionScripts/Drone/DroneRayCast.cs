using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroneRayCast : MonoBehaviour
{
    /// <summary>
    /// Class to manage the raycast used to detect hazards
    /// </summary>
    
    //Class Reference
    private DroneController droneController; //Reference to the drone controller script 

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

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && droneController.gameManager.UIManager.artificialHorizonCircle.GetComponent<Image>().color == Color.green && droneController.firstPersonCam.activeSelf == true && !Input.GetMouseButton(1))
        {
            ShootRaycast();  //Called if player presses mouse 1 while in first person view and can interact with a hazard
        }
    }

    /// <summary>
    /// Method to determine if the drone is looking at a hazard and within range to detect it as such
    /// </summary>
    public void RaycastDistanceCheck()
    {
        Physics.Raycast(droneController.firstPersonCam.transform.position, droneController.firstPersonCam.transform.forward, out check, droneController.gameManager.hazardManager.maxDetectionDistance); //Sends out a raycast 100m in front of the drone each frame

        //If the raycast hits a hazard
        if (check.collider != null && check.collider.CompareTag("Hazard"))
        {
            string hazardName = check.collider.GetComponent<MonoBehaviour>().GetType().Name;
            //If the hazard is within the optimal distance from the drone
            if (check.distance > droneController.gameManager.hazardManager.GetOptimalRange(hazardName).x && check.distance < droneController.gameManager.hazardManager.GetOptimalRange(hazardName).y)
            {
                droneController.gameManager.UIManager.artificialHorizonCircle.GetComponent<Image>().color = Color.green;  //Sets the artificial horizon UI elements to green
            }
            //If the hazard is ouwith the optimal distance from the drone
            else if (check.distance < droneController.gameManager.hazardManager.maxDetectionDistance)
            {
                droneController.gameManager.UIManager.artificialHorizonCircle.GetComponent<Image>().color = Color.red;   //Sets the artificial horizon UI elements to red
            }
        }

        //If the raycast hits nothing 
        else if (check.collider == null || check.collider.CompareTag("Fixed"))
        {
            droneController.gameManager.UIManager.artificialHorizonCircle.GetComponent<Image>().color = Color.black;  //Sets the artificial horizon UI elements to grey
        }
    }

    /// <summary>
    /// Method to shoot a raycast to get information about a hazard if mouse 1 is pressed when within the optimal range of the hazard
    /// </summary>
    public void ShootRaycast()
    {
        Physics.Raycast(droneController.firstPersonCam.transform.position, droneController.firstPersonCam.transform.forward, out hit, 100f);  //Shoots out a raycast

        //If the raycast hits a hazard 
        if (hit.collider != null && hit.collider.CompareTag("Hazard"))
        {            
            stopMovement = true;   //Stops the drone     
            droneController.gameManager.hazardManager.InitialiseHazard(hit.collider.GetComponent<MonoBehaviour>()); //Calls the InitialiseHazard() method in hazard manager           
        }
    }
}
