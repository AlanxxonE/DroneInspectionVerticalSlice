using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private DroneUI droneUi;
    private DroneMovement droneMovement;
    private DroneRayCast droneRayCast;
    private GameManager gameManager;

    private bool isPaused;
    public int droneHits = 0; //How many time the drone can collide before being destroyed 

    public bool GetIsPaused(){return isPaused;}
 
    public DroneRayCast GetDroneRayCast() {return droneRayCast;}
    public DroneMovement GetDroneMovement() { return droneMovement;}
}
