using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{    
     /// <summary>
     /// Class that dictates the behaviour of the cars on the streets
     /// </summary>
     
    private float carSpeed = 0; //The speed variable of the car

    private Rigidbody rb; //The RigidBody Reference component of the Car object

    /// <summary>
    /// User-defined type to differentiate the lane where the car is driving through
    /// </summary>
    public enum Lane
    {
        Center,     //The center lane
        Left,       //The left lane
        Right       //The Right lane
    }

    public Lane carLane; //The variable that denominates the state of the car

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //The RigidBody Reference

        StartCoroutine(StartEngine()); //The coroutine that makes the car start moving
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * carSpeed * Time.fixedDeltaTime; //Every frame the velocity of the RigidBody component is updated making the car move forward based on where the car is facing
    }

    /// <summary>
    /// Method use to detect if the car approaches either a crossroad or the end of a road
    /// </summary>
    /// <param name="other"></param> The trigger that the car encounters
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndOfRoad")
        {
            transform.eulerAngles += new Vector3(0, 180, 0); //Fully turns the car if the road is finished
        }

        if (other.gameObject.tag == "CrossRoad" && carLane == Lane.Left)
        {
            transform.eulerAngles += new Vector3(0, 270, 0); //Makes the car turn left if the car reaches a crossroad while on the left lane
        }

        if (other.gameObject.tag == "CrossRoad" && carLane == Lane.Right)
        {
            transform.eulerAngles += new Vector3(0, 90, 0); //Makes the car turn right if the car reaches a crossroad while on the right lane
        }
    }

    /// <summary>
    /// Coroutine that makes the car move after an amount of seconds
    /// </summary>
    IEnumerator StartEngine()
    {
        float secondsToWait = Random.Range(1, 3f); //The random amount of seconds the car needs in order to start moving

        yield return new WaitForSeconds(secondsToWait);

        int randomSpeed = Random.Range(2000, 3000); //The speed that the car will have on the road once the amount of seconds are passed

        carSpeed = randomSpeed; //This speed is then assigned to the variable used in the update method
    }
}
