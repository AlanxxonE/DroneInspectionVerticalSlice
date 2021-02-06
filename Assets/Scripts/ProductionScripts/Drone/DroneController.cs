using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    //Class References
    public DroneUI droneUi;
    public DroneMovement droneMovement;
    public DroneRayCast droneRayCast;
    public GameManager gameManager;

    //General Variables
    public bool isPaused;
    public int droneLives = 3; //How many time the drone can collide before being destroyed 

    //Camera Variables
    [Tooltip("Sets the first person camera of the drone")]
    public GameObject thirdPersonCam;
    [Tooltip("Sets the third person camera of the drone")]
    public GameObject firstPersonCam;
    [Tooltip("Sets the vertical angle limit of the first person camera")]
    public float camMaxVerticalFreeLookAngle = 90f;

    //Movement Variables    
    [Tooltip("Sets the velocity of the drone in meters per second")]
    public float droneVelocity;
    [Tooltip("Sets the maximum height the drone can fly to in metres")]
    public float flightCeiling = 150;
    [Tooltip("Sets the maximum range the drone can fly from it's start position in metres, i.e. the distance where signal strength becomes zero and the 'static' is at it's maximum")]
    public float maxRange;    
    [HideInInspector] public float canMove = 1; //Float to allow/disallow movement

    //Tilt Variables
    [Tooltip("Sets the child game object, i.e. the drone model, that tilts about the parent object")]
    public GameObject tiltingChild;
    [Tooltip("Sets the maximum angle the drone will tilt by in any given direction when at it's maximum velocity in any given direction. Does not affect flight mechanics, purely visual.")]
    public float maxTiltAngle;
    [Tooltip("Sets the force for the Push Back when Colliding with something")]
    public float pushBackForce;
}
