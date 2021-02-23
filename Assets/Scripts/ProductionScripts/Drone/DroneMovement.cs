﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    /// <summary>
    /// Class that handles the drone's movement and tilt
    /// </summary>

    //General Variables
    [HideInInspector]public Rigidbody parentRB; //Reference to the drone's rigidbody
    private DroneController droneController; 
    private int droneHits = 0; //Keeps track of drone collision count    

    //Movement Variables
    [HideInInspector] public Vector3 startPosition;   //Reference to the position teh dropne starts at   
    private Vector3 desiredVelocity;  //Velocity the drone is aiming for
    private Vector3 referenceVelocity; //Reference to the default velocity of the drone          
    private Vector3 currentVelocity;  //Reference to current velocity of the drone
    
    //Tilt Variables
    private float theta;     //Math function used to tie the drone's velocity to it's tilt
    private bool rotationFix;  //Boolean used to set a new rotation if the drone interacts with a hazard    

    private void Awake()
    {
        droneController = this.GetComponent<DroneController>();
        parentRB = GetComponent<Rigidbody>(); //gets the drone's rigidbody
        startPosition = parentRB.transform.position;  //Sets the start position
        theta = droneController.maxTiltAngle / droneController.droneVelocity;   //Sets the theta maths function        
        Cursor.lockState = CursorLockMode.Locked; //Locks the mouse cursor        
    }

    private void Update()
    {
        if (!droneController.isPaused) //If game is not paused
        {
            if (droneController.droneRayCast.stopMovement == false) //If the drone can move
            {
                Cursor.lockState = CursorLockMode.Locked;  //Locks the cursor
                droneController.canMove = 1;   //lets the drone move 
            }
            else
            {
                Cursor.lockState = CursorLockMode.None; //Unlocks the cursor
                droneController.canMove = 0;  //prevents the drone moving
            }

            Movement();
            Rotation();
            Tilt();            
            currentVelocity = parentRB.velocity;  //Sets the current velocity of the drone equal to the drone's velocity
        }        
    }

    /// <summary>
    /// Method to handle the drone's movement
    /// </summary>
    private void Movement()
    {
        //Sets normalised value for each vector of the drone's velocity
        Vector3 normalVelocityX = Input.GetAxisRaw("velX") * transform.right;
        Vector3 normalVelocityY = Input.GetAxisRaw("velY") * transform.up;
        Vector3 normalVelocityZ = Input.GetAxisRaw("velZ") * transform.forward;

        float yVelocityLimiter; //Used to prevent drone from flying too high 
        if (parentRB.transform.position.y >= droneController.flightCeiling && normalVelocityY.y >= 0)
        {
            yVelocityLimiter = -1; //Sets drone's y-velocity to negative if at or above the flight ceiling with a positive y-velocity(Makes it bounce down)
        }
        else
        {
            yVelocityLimiter = 1; //Sets the drones velocity back to normal if under the flight ceiling 
        }
        desiredVelocity = (normalVelocityX + (normalVelocityY * yVelocityLimiter) + normalVelocityZ) * droneController.droneVelocity * droneController.canMove; //Sets the drone's desired velocity
        parentRB.velocity = Vector3.SmoothDamp(parentRB.velocity, desiredVelocity, ref referenceVelocity, droneController.smoothTime);  //Sets the drone's velocity
    }

    /// <summary>
    /// Merthod to handle the rotation of the drone
    /// </summary>
    private void Rotation()
    {
        float turnTemp = droneController.turnSpeed * Input.GetAxis("Mouse X"); //Sets the rate at which the drone should turn based on the mouse's x-axis input
        transform.Rotate(0, turnTemp * droneController.canMove, 0);  //Rotates the drone based on the value of turn temp

        if (droneController.canMove == 0) //If the drone can't move (when interacting with a hazard)
        {
            transform.LookAt(droneController.droneRayCast.hit.collider.transform.position); //Looks at the hazard
            rotationFix = true;  //Sets rotation fix equals true
        }
        else if (droneController.canMove == 1 && rotationFix == true)  //If the drone can move and has interacted with a hazard
        {
            rotationFix = false; //Sets rotation fix equals to false
            Vector3 temp = transform.localEulerAngles; //Gets the drone's rotation
            transform.localEulerAngles = new Vector3(0, temp.y, temp.z); //Sets new rotation for the drone
        }
    }

    /// <summary>
    /// Method to handle the tilt angle of the drone
    /// </summary>
    /// <returns></returns>
    public Vector3 Tilt()
    {
        float forwardTiltAngle = Vector3.Dot(parentRB.velocity, transform.forward);  //Sets the forward tilt angle of the drone based on it's local forward velocity
        float rightTiltAngle = Vector3.Dot(parentRB.velocity, transform.right * -1);  //Sets the right tilt angle of the drone based on it's local right velocity

        Vector3 desiredTiltAngle = new Vector3(forwardTiltAngle * theta, 0, rightTiltAngle * theta);  //Sets the vector for the desired tilt angle based on the drone's velocity. Theta is used to tie the max tilt angle and max velocity together

        droneController.tiltingChild.transform.localEulerAngles = desiredTiltAngle;  //Sets the tilt angle of the drone

        return desiredTiltAngle;
    }

    /// <summary>
    /// Method to handle drone collisiosn
    /// </summary>
    /// <param name="obstacle"></param>
    private void OnCollisionEnter(Collision obstacle)
    {
        droneHits++;       //keeps track of number of collisions 
        if (droneHits == droneController.droneLives) //If the drone collides 3 times
        {
            droneController.gameManager.levelManager.SceneSelectMethod(3);  //Loads score scene
        }

        Vector3 angleAtCollision = currentVelocity.normalized;  //Normalised vector of the direction the drone is flying in at time of collision
        Vector3 normalAngleAtCollision = obstacle.contacts[0].normal.normalized;  //Normal angle of the face the drone collides with
        Vector3 directionFix = new Vector3(1, 1, 1);  //Vector to determine which angle the drone should be knocked in

        //If the face has been rotated around any axis the angle to drone bounces off will change
        if (normalAngleAtCollision.x != 0)
        {
            directionFix += new Vector3(-2, 0, 0);
        }
        if (normalAngleAtCollision.y != 0)
        {
            directionFix += new Vector3(0, -2, 0);
        }
        if (normalAngleAtCollision.z != 0)
        {
            directionFix += new Vector3(0, 0, -2);
        }

        Vector3 bounceAngle = (normalAngleAtCollision + Vector3.Scale(angleAtCollision, directionFix)) * 360;    //Bounce angle  is equal to the normal angle at collision plus the angle the drone is flying at rotated around the normal  
        parentRB.AddForce(bounceAngle * droneController.pushBackForce);       //Adds force in teh driection of the bounce angle
    }
}
