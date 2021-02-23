﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    /// <summary>
    /// Claa that handles the drone's cameras
    /// </summary>
    
    //Class Reference
    private DroneController droneController;

    //Camera Variables
    private bool firstPerson = false;  //Boolean to determine if in third person   
    private float camTurnSpeed;   //Variable to set the turn speed of teh camera
    private float camXAxisRotation;  //Reference to the x-axis rotation of the camera
    private float camYAxisRotation;  //Reference to the y-axis rotation of the camera
    [HideInInspector]public float interpolationTime = 0.0f;

    private void Awake()
    {        
        droneController = this.GetComponent<DroneController>();
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

    public bool FocusOnHazard(Transform target)
    {
        //While Lerping
        
        droneController.thirdPersonCam.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, target.position.x, interpolationTime), Mathf.Lerp(this.transform.position.y, target.position.y, interpolationTime), Mathf.Lerp(this.transform.position.z, target.position.z, interpolationTime)) - (droneController.interpolationOffset * target.GetComponentInParent<Transform>().forward);
        interpolationTime += droneController.interpolationTime * Time.deltaTime;

        droneController.thirdPersonCam.transform.LookAt(target.position);

        if (interpolationTime >= 1)
        {
            return true;
        }

        return false;
    }
}
