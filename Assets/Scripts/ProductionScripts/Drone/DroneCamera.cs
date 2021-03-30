using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    /// <summary>
    /// Class that handles the drone's cameras
    /// </summary>
    
    //Class Reference
    private DroneController droneController;

    //General Variables
    [HideInInspector]public bool firstPerson = false;  //Boolean to determine if in third person   
    private bool lastCameraPerspective;
    private float camTurnSpeed;   //Variable to set the turn speed of teh camera
    private float camXAxisRotation;  //Reference to the x-axis rotation of the camera
    private float camYAxisRotation;  //Reference to the y-axis rotation of the camera
    [HideInInspector]public float interpolationTime = 0.0f;  //Variable to keep trtack of the interpolation time for the camera to move when interacting with a hazard
    private Vector3 cameraPosition;    //Initial position of the third person camera relative to the drone game object
    private Quaternion cameraRotation;   //Initial rotation of the third person camera relative to the drone game object
    [HideInInspector]public Vector3 startPosition, endPosition;   //Vector coordinates for camera movement 

    private void Awake()
    {        
        droneController = this.GetComponent<DroneController>();
        cameraPosition = droneController.thirdPersonCam.transform.localPosition;   //Sets initial position
        cameraRotation = droneController.thirdPersonCam.transform.rotation;        //Sets initial rotation
        camTurnSpeed = droneController.turnSpeed;  //Sets the camera turn speed equal to that of the drone turn speed  
        SwitchPerspective(firstPerson);   //Calls the switch perspective method to ensure everything is enabled/disabled as it should be, i.e. the camera's and UI
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleCam") && droneController.canMove == 1)
        {
            firstPerson = !firstPerson;  //If the camera toggle is pressed it swaps the camera state between third or first person
            SwitchPerspective(firstPerson); //Calls the switch perspective method
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            lastCameraPerspective = firstPerson;
            firstPerson = true;
            SwitchPerspective(firstPerson);  //Calls the switch perspective method
        }

        else if (Input.GetMouseButtonUp(1)) //If mouse 2 is no longer pressed
        {
            droneController.turnSpeed = camTurnSpeed;  //Resets the turn speed 
            droneController.firstPersonCam.transform.localEulerAngles = Vector3.zero;  //Resets the angle of the first person camera
            camXAxisRotation = 0;  //Resets the x-axis rotation of the camera
            camYAxisRotation = 0;  //Resets the Y-axis rotation of the camera
            firstPerson = lastCameraPerspective;
            SwitchPerspective(firstPerson);
        }

        if (Input.GetMouseButton(1)) //If mouse 2 is pressed
        {
            FreeLook();
        }
    }

    /// <summary>
    /// Method that manages the free look mechanic
    /// </summary>
    private void FreeLook()
    {
        droneController.turnSpeed = 0; //The drone cant turn
        camXAxisRotation += Input.GetAxis("Mouse Y") * -camTurnSpeed; camYAxisRotation += Input.GetAxis("Mouse X") * camTurnSpeed;  //Gets the x-axis and y-axis rotation based on mouse input
        float camXAxisRotationTemp = Mathf.Clamp(camXAxisRotation, -droneController.camMaxVerticalFreeLookAngle, droneController.camMaxVerticalFreeLookAngle);  //Clamps the rotation about the x-axis 
        droneController.firstPersonCam.transform.localEulerAngles = new Vector3(camXAxisRotationTemp, camYAxisRotation, 0);  //Applies the y-axis and clamped x-axis rotation to the camera
    }

    /// <summary>
    /// Method to switch camera perspectives, takes in a bool for camera state
    /// </summary>
    /// <param name="isFirstPerson"></param>
    public void SwitchPerspective(bool isFirstPerson)
    {
        firstPerson = isFirstPerson;   //Ensures new boolean is set

        //Sets first person camera variables
        droneController.firstPersonCam.SetActive(isFirstPerson);  
        droneController.firstPersonCam.GetComponent<AudioListener>().enabled = isFirstPerson;

        //Sets third person camera variables
        droneController.thirdPersonCam.SetActive(!isFirstPerson);
        droneController.thirdPersonCam.GetComponent<AudioListener>().enabled = !isFirstPerson;

        //Enables/disables FPP UI
        droneController.gameManager.UIManager.droneUI.EnableUI(firstPerson);
    }

    /// <summary>
    /// Method to handle the camera movement as it focus on the point of a hazard or returning from such. Takes in a Transform for the position and a boolean 
    /// to determine if camera is returning to it's original position. Returns true if the desired final camera position is reached.
    /// </summary>
    /// <param name="cameraFocalPoint"></param>
    /// <param name="resetCameraPosition"></param>
    /// <returns></returns>
    public bool FocusOnHazard(Transform cameraFocalPoint, Transform hazardTransform, bool resetCameraPosition)
    {
        if(!resetCameraPosition) //If camera is not being reset, i.e. is focusing on the hazard point
        {
            startPosition = this.transform.position + cameraPosition; //Start position is set to camera's original position relative to the drone
            endPosition = cameraFocalPoint.position - (droneController.interpolationOffset * (hazardTransform.forward - (hazardTransform.up/4))); //End pos of camera minus an offset in the relative z-axis
            droneController.thirdPersonCam.transform.LookAt(cameraFocalPoint.position); //Points camera at the hazard pos

            if (droneController.thirdPersonCam.transform.position == endPosition )
            {               
                return true; //Returns true when position has been reached
            }
        }

        else if (resetCameraPosition) //If camera is being reset to it's original position relative to the drone
        {
            startPosition = cameraFocalPoint.position - (droneController.interpolationOffset * (hazardTransform.forward - (hazardTransform.up / 3))); //Sets the start point equal to the previous end position
            endPosition = transform.position + cameraPosition; //Sets the end pos equal to the camera's original position relative to the drone
            droneController.thirdPersonCam.transform.LookAt(cameraFocalPoint.position); //Camera looks at hazard point

            if (droneController.thirdPersonCam.transform.position == endPosition)
            {
                //Ensures variables are reset correctly
                droneController.thirdPersonCam.transform.localRotation = cameraRotation;
                droneController.thirdPersonCam.transform.localPosition = cameraPosition;  
                
                return true; //Returns true when pend position has been reached
            }
        }
        
        //Interpolates camera position between start and end positions
        droneController.thirdPersonCam.transform.position = new Vector3(Mathf.Lerp(startPosition.x, endPosition.x, interpolationTime), Mathf.Lerp(startPosition.y, endPosition.y, interpolationTime), Mathf.Lerp(startPosition.z, endPosition.z, interpolationTime));
        //Counts the timer upwards
        interpolationTime +=  Time.deltaTime / droneController.interpolationTime ;       

        return false;
    }
}