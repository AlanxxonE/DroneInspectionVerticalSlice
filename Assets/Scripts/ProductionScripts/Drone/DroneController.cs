using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    //Class References   
    [Header("Class References")]
    public DroneMovement droneMovement;
    public DroneRayCast droneRayCast;
    public DroneCamera droneCamera;
    public GameManager gameManager;

    //General Variables
    [Header("General Variables")]    
    public int droneLives = 3; //How many times the drone can collide before being destroyed 
    [HideInInspector] public bool isPaused;

    //Camera Variables
    [Header("Camera Variables")]
    [Tooltip("Sets the first person camera of the drone")]
    public GameObject firstPersonCam;
    [Tooltip("Sets the third person camera of the drone")]
    public GameObject thirdPersonCam;
    [Tooltip("Sets the vertical angle limit of the first person camera")]
    public float camMaxVerticalFreeLookAngle = 90f;
    [Tooltip("Sets the time taken for the camera to interpolate to a hazard target position when being interacted with")]
    public float interpolationTime;
    [Tooltip("Sets the z-axis offset for the camera to reach after interpolation")]
    public float interpolationOffset;


    //Movement Variables
    [Header("Movement Variables")]
    [Tooltip("Sets the velocity of the drone in meters per second")]
    public float droneVelocity;
    [Tooltip("Sets the maximum height the drone can fly to in metres")]
    public float flightCeiling;
    [Tooltip("Sets the maximum range the drone can fly from it's start position in metres, i.e. the distance where signal strength becomes zero and the 'static' is at it's maximum")]
    public float maxRange;
    [Tooltip("Sets the speed at which the drone turns")]
    public float turnSpeed;
    [Tooltip("Time taken to accelerate to new velocity, smaller is faster")]
    public float smoothTime;  
    [HideInInspector] public float canMove = 1; //Float to allow/disallow movement

    //Tilt Variables
    [Header("Tilt Variables")]
    [Tooltip("Sets the child game object, i.e. the drone model, that tilts about the parent object")]
    public GameObject tiltingChild;
    [Tooltip("Sets the maximum angle the drone will tilt by in any given direction when at it's maximum velocity in any given direction. Does not affect flight mechanics, purely visual.")]
    public float maxTiltAngle;
    [Tooltip("Sets the force for the Push Back when Colliding with something")]
    public float pushBackForce;
}
