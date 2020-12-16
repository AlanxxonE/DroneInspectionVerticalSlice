using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    /// <summary>
    /// Class that handles the drone's movement ,tilt and camera
    /// </summary>
    
    //General Variables
    public Rigidbody parentRB; //Reference to the drone's rigidbody
    public bool isPaused;
    public int droneHits = 0; //How many time the drone can collide before being destroyed 

    //Camera Variables
    public bool thirdPerson = true;  //Boolean to determine if in third person
    public GameObject thirdPersonCam;   //Reference to third person camera
    public GameObject firstPersonCam;   //Reference to first person camera
    public bool freelook = false;   //Boolean to determine if the players is free looking
    private float camTurnSpeed;   //Variable to set the turn speed of teh camera
    public float camMaxVerticalFreeLookAngle = 90f; //Variable to set the max angle the free look camera can rotate through
    private float camXAxisRotation;  //Reference to the x-axis rotation of the camera
    private float camYAxisRotation;  //Reference to the y-axis rotation of the camera

    //Movement Variables
    private float turnSpeed = 2f;  //Variable for turn speed
    [SerializeField]
    [Tooltip("Sets the velocity of the drone in meters per second")]
    private float droneVelocity;  
    private float speed;   //Variable to determine speed of the drone
    private Vector3 desiredVelocity;  //Velocity the drone is aiming for
    private Vector3 referenceVelocity; //Reference to the default velocity of the drone
    private float smoothTime = 0.2f;  //Time taken to accelerate to new velocity, smaller is faster
    [SerializeField]
    [Tooltip("Sets the maximum height the drone can fly to in metres")]
    public float flightCeiling = 150;  
    [SerializeField]
    [Tooltip("Sets the maximum range the drone can fly from it's start position in metres, i.e. the distance where signal strength becomes zero and the 'static' is at it's maximum")]
    public float maxRange;
    public Vector3 startPosition;   //Reference to the position teh dropne starts at
    public float canMove = 1;    //Float to allow/disallow movement
    private Vector3 currentVelocity;  //Reference to current velocity of the drone

    //Tilt Variables
    public GameObject tiltingChild;  //The child object opf the drone that tilts about the parent
    [SerializeField]
    [Tooltip("Sets the maximum angle the drone will tilt by in any given direction when at it's maximum velocity in any given direction. Does not affect flight mechanics, purely visual.")]
    public float maxTiltAngle;  //maximum angle the drone can tilt by at it's maximum velocity
    private float theta;     //Math function used to tie the drone's velocity to it's tilt
    private bool rotationFix;  //Boolean used to set a new rotation if the drone interacts with a hazard
    [SerializeField]
    [Tooltip("Sets the force for the Push Back when Colliding with something")]
    public float pushBackForce;
    
    private void Awake()
    {
        startPosition = parentRB.transform.position;  //Sets the start position
        speed = droneVelocity / Time.fixedDeltaTime;  //Sets speed
        theta = maxTiltAngle / droneVelocity;   //Sets the theta maths function
        camTurnSpeed = turnSpeed;  //Sets the camera turn speed equal to that of the drone turn speed
    }
    void Start()
    { 
        parentRB = GetComponent<Rigidbody>(); //gets the drone's rigidbody

        Cursor.lockState = CursorLockMode.Locked; //Locks the mouse cursor
    }

    void Update()
    {
        if (!isPaused) //If game is not paused
        {
            if (GetComponentInChildren<HazardManager>().stopMovement == false) //If the drone can move
            {
                Cursor.lockState = CursorLockMode.Locked;  //Locks the cursor
                canMove = 1;   //lets the drone move 
            }
            else
            {
                Cursor.lockState = CursorLockMode.None; //Unlocks the cursor
                canMove = 0;  //prevents the drone moving
            }

            Movement();
            Rotation();
            Tilt();
            Camera();
            currentVelocity = parentRB.velocity;  //Sets the current velocity of the drone equal to the drone's velocity
        }
    }

    /// <summary>
    /// Method to handle the drone's movement
    /// </summary>
    private void Movement()
    {
        //Sets normalised value fopr each vector of the drone's velocity
        Vector3 normalVelocityX = Input.GetAxisRaw("velX") * transform.right;
        Vector3 normalVelocityY = Input.GetAxisRaw("velY") * transform.up;
        Vector3 normalVelocityZ = Input.GetAxisRaw("velZ") * transform.forward;

        float yVelocityLimiter; //Used to prevent drone from flying too high 
        if (parentRB.transform.position.y >= flightCeiling && normalVelocityY.y >= 0)
        {
            yVelocityLimiter = -1; //Sets drone's y-velocity to negative if at or above the flight ceiling with a positive y-velocity(Makes it bounce down)
        }
        else
        {
            yVelocityLimiter = 1; //Sets the drones velocity back to normal if under the flight ceiling 
        }
        desiredVelocity = ((normalVelocityX * speed) + (normalVelocityY * speed * yVelocityLimiter) + (normalVelocityZ * speed)) * canMove; //Sets the drone's desired velocity
        parentRB.velocity = Vector3.SmoothDamp(parentRB.velocity, desiredVelocity * Time.fixedDeltaTime, ref referenceVelocity, smoothTime);  //Sets the drone's velocity
    }

    /// <summary>
    /// Merthod to handle the rotation of the drone
    /// </summary>
    private void Rotation()
    {
        float turnTemp = turnSpeed * Input.GetAxis("Mouse X"); //Sets the rate at which the drone should turn based on the mouse's x-axis input
        transform.Rotate(0, turnTemp * canMove, 0);  //Rotates the drone based on the value of turn temp

        if (canMove == 0 ) //If the drone can't move (when interacting with a hazard)
        {
            transform.LookAt(GetComponentInChildren<HazardManager>().hit.collider.transform.position); //Looks at the hazard
            rotationFix = true;  //Sets rotation fix equals true
        }
        else if (canMove == 1 && rotationFix == true)  //If the drone can move and has interacted with a hazard
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

        tiltingChild.transform.localEulerAngles = desiredTiltAngle;  //Sets the tilt angle of the drone

        return desiredTiltAngle;
    }

    /// <summary>
    /// Method to handle the different camera states 
    /// </summary>
    private void Camera()
    {
        if (Input.GetButtonDown("ToggleCam"))
        {
            thirdPerson = !thirdPerson;  //If the camera toggle is pressed it swaps the camera state between third or first person
        }

        if (Input.GetMouseButton(1)) //If mopuse 2 is pressed
        {
            turnSpeed = 0; //The drone cant turn
            freelook = true;  //Free look is set to true
            thirdPerson = false;  //Third person is set to false

            camXAxisRotation += Input.GetAxis("Mouse Y") * -camTurnSpeed; camYAxisRotation += Input.GetAxis("Mouse X") * camTurnSpeed;  //Gets the x-axis and y-axis rotation based on mouse input
            float camXAxisRotationTemp = Mathf.Clamp(camXAxisRotation, -camMaxVerticalFreeLookAngle, camMaxVerticalFreeLookAngle);  //Clamps the rotation about the x-axis 
            firstPersonCam.transform.localEulerAngles = new Vector3(camXAxisRotationTemp, camYAxisRotation, 0);  //Applies the y-axis and clamped x-axis rotation to the camera
        }
        else if (Input.GetMouseButtonUp(1)) //If mouse 2 is no longer pressed
        {
            turnSpeed = camTurnSpeed;  //Resets the tunr speed 
            freelook = false;  //Sets free look equal to false
            firstPersonCam.transform.localEulerAngles = Vector3.zero;  //Resets the angle of the first person camera
            camXAxisRotation = 0;  //Resets the x-axis rotation of the camera
            camYAxisRotation = 0;  //Resets the Y-axis rotation of the camera
        }

        //Sets third person camera active and first person camera inactive
        if (thirdPerson)
        {
            thirdPersonCam.SetActive(true);
            thirdPersonCam.GetComponent<AudioListener>().enabled = true;
            firstPersonCam.SetActive(false);
            firstPersonCam.GetComponent<AudioListener>().enabled = false;
        }

        //Sets first person camera active and third person camera inactive
        else
        {
            thirdPersonCam.SetActive(false);
            thirdPersonCam.GetComponent<AudioListener>().enabled = false;
            firstPersonCam.SetActive(true);
            firstPersonCam.GetComponent<AudioListener>().enabled = true;
        }
    }

    /// <summary>
    /// Method to handle drone collisiosn
    /// </summary>
    /// <param name="obstacle"></param>
    private void OnCollisionEnter(Collision obstacle)
    {
        droneHits++;       //keeps track of number of collisions 
        if(droneHits == 3) //If the drone collides 3 times
        {
            GetComponent<DroneUI>().levelManagerScript.SceneSelectMethod(3);  //Loads score scene
        }

        Vector3 angleAtCollision = currentVelocity.normalized;  //Normalised vector of the direction the drone is flying in at time of collision
        Vector3 normalAngleAtCollision = obstacle.contacts[0].normal.normalized;  //Normal angle of the face the drone collides with
        Vector3 directionFix = new Vector3(1,1,1);  //Vector to determine which angle the drone should be knocked in

        //If the face has been rotated around any axis the angle to drone bounces off will change
        if(normalAngleAtCollision.x !=0)  
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
        
        Vector3 bounceAngle = (normalAngleAtCollision + Vector3.Scale(angleAtCollision,directionFix)) * 360;    //Bounce angle  is equal to the normal angle at collision plus the angle the drone is flying at rotated around the normal  
        parentRB.AddForce(bounceAngle * pushBackForce);       //Adds force in teh driection of the bounce angle
    }   
}
