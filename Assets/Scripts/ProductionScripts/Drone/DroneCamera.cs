using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    /// <summary>
    /// Claa that handles the drone's cameras
    /// </summary>
    
    //Class Reference
    private DroneController droneController;

    //General Variables
    private bool firstPerson = false;  //Boolean to determine if in third person   
    private float camTurnSpeed;   //Variable to set the turn speed of teh camera
    private float camXAxisRotation;  //Reference to the x-axis rotation of the camera
    private float camYAxisRotation;  //Reference to the y-axis rotation of the camera
    [HideInInspector]public float interpolationTime = 0.0f;
    private Vector3 cameraPosition, cameraRotation;    
    [HideInInspector]public Vector3 startPosition, targetPosition;

    private void Awake()
    {        
        droneController = this.GetComponent<DroneController>();
        cameraPosition = this.transform.position - droneController.thirdPersonCam.transform.position;
        cameraRotation = droneController.thirdPersonCam.transform.rotation.eulerAngles;
        camTurnSpeed = droneController.turnSpeed;  //Sets the camera turn speed equal to that of the drone turn speed  
        SwitchPerspective(firstPerson);
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleCam"))
        {
            firstPerson = !firstPerson;  //If the camera toggle is pressed it swaps the camera state between third or first person
            SwitchPerspective(firstPerson);
        }

        if (Input.GetMouseButtonDown(1))
        {
            firstPerson = true;
            SwitchPerspective(firstPerson);
        }

        else if (Input.GetMouseButtonUp(1)) //If mouse 2 is no longer pressed
        {
            droneController.turnSpeed = camTurnSpeed;  //Resets the tunr speed 
            droneController.firstPersonCam.transform.localEulerAngles = Vector3.zero;  //Resets the angle of the first person camera
            camXAxisRotation = 0;  //Resets the x-axis rotation of the camera
            camYAxisRotation = 0;  //Resets the Y-axis rotation of the camera
        }

        if (Input.GetMouseButton(1)) //If mouse 2 is pressed
        {
            FreeLook();
        }
        Debug.Log(targetPosition);
    }

    private void FreeLook()
    {
        droneController.turnSpeed = 0; //The drone cant turn
        camXAxisRotation += Input.GetAxis("Mouse Y") * -camTurnSpeed; camYAxisRotation += Input.GetAxis("Mouse X") * camTurnSpeed;  //Gets the x-axis and y-axis rotation based on mouse input
        float camXAxisRotationTemp = Mathf.Clamp(camXAxisRotation, -droneController.camMaxVerticalFreeLookAngle, droneController.camMaxVerticalFreeLookAngle);  //Clamps the rotation about the x-axis 
        droneController.firstPersonCam.transform.localEulerAngles = new Vector3(camXAxisRotationTemp, camYAxisRotation, 0);  //Applies the y-axis and clamped x-axis rotation to the camera
    }

    public void SwitchPerspective(bool isFirstPerson)
    {
        firstPerson = isFirstPerson;
        droneController.firstPersonCam.SetActive(isFirstPerson);
        droneController.firstPersonCam.GetComponent<AudioListener>().enabled = isFirstPerson;
        droneController.thirdPersonCam.SetActive(!isFirstPerson);
        droneController.thirdPersonCam.GetComponent<AudioListener>().enabled = !isFirstPerson;
        droneController.droneUI.EnableUI(firstPerson);
    }

    public bool FocusOnHazard(Transform target, bool resetCameraPosition)
    {
        if(!resetCameraPosition)
        {
            startPosition = this.transform.position + cameraPosition; 
            targetPosition = target.position - (droneController.interpolationOffset * target.GetComponentInParent<Transform>().forward);
            droneController.thirdPersonCam.transform.LookAt(target.position);

            if (droneController.thirdPersonCam.transform.position == targetPosition )
            {               
                return true;
            }
        }

        else if (resetCameraPosition)
        {           
            startPosition = target.position - (droneController.interpolationOffset * target.GetComponentInParent<Transform>().forward);
            targetPosition = this.transform.position - cameraPosition;

            if(Vector3.Distance(droneController.thirdPersonCam.transform.position, targetPosition) == 0.00f)
            {
                droneController.thirdPersonCam.transform.position = targetPosition;
                droneController.thirdPersonCam.transform.eulerAngles = cameraRotation;
                return true;
            }
        }

        droneController.thirdPersonCam.transform.position = new Vector3(Mathf.Lerp(startPosition.x, targetPosition.x, interpolationTime), Mathf.Lerp(startPosition.y, targetPosition.y, interpolationTime), Mathf.Lerp(startPosition.z, targetPosition.z, interpolationTime));
        interpolationTime += droneController.interpolationTime * Time.deltaTime;       

        return false;
    }
}
