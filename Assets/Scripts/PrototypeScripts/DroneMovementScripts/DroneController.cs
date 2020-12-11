using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Rigidbody parentRB;

    //Camera Variables
    public bool thirdPerson = true;
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public bool freelook = false;
    private float camTurnSpeed;
    public float camMaxVerticalFreeLookAngle = 45f;
    private float camXAxisRotation;
    private float camYAxisRotation;

    //Movement Variables
    private float turnSpeed = 2f;
    [SerializeField]
    [Tooltip("Sets the velocity of the drone in meters per second")]
    private float droneVelocity;
    private float speed;
    private Vector3 desiredVelocity;
    private Vector3 referenceVelocity;
    private float smoothTime = 0.2f;
    [SerializeField]
    [Tooltip("Sets the maximum height the drone can fly to in metres")]
    public float flightCeiling = 150;
    [SerializeField]
    [Tooltip("Sets the maximum range the drone can fly from it's start position in metres, i.e. the distance where signal strength becomes zero and the 'static' is at it's maximum")]
    public float maxRange;
    public Vector3 startPosition;
    public float canMove = 1;
    private Vector3 currentVelocity;

    //Tilt Variables
    public GameObject tiltingChild;
    [SerializeField]
    [Tooltip("Sets the maximum angle the drone will tilt by in any given direction when at it's maximum velocity in any given direction. Does not affect flight mechanics, purely visual.")]
    public float maxTiltAngle;
    private float theta;
    private bool rotationFix;
    [SerializeField]
    [Tooltip("Sets the force for the Push Back when Colliding with something")]
    public float pushBackForce;
    public bool checkBounce = false;
    public Collision collisionRef;

    public PauseBehaviour pauseRef;

    public int droneHits = 0;

    private void Awake()
    {
        startPosition = parentRB.transform.position;
        speed = droneVelocity / Time.fixedDeltaTime;
        theta = maxTiltAngle / (speed * Time.fixedDeltaTime);
        camTurnSpeed = turnSpeed;
    }
    void Start()
    {
        parentRB = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (pauseRef.activePause == false)
        {
            if (GetComponentInChildren<HazardManager>().stopMovement == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                canMove = 1;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                canMove = 0;
            }

            Movement();
            Rotation();
            Tilt();
            Camera();
            currentVelocity = parentRB.velocity;
        }
    }

    private void Movement()
    {

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
        desiredVelocity = ((normalVelocityX * speed) + (normalVelocityY * speed * yVelocityLimiter) + (normalVelocityZ * speed)) * canMove;
        parentRB.velocity = Vector3.SmoothDamp(parentRB.velocity, desiredVelocity * Time.fixedDeltaTime, ref referenceVelocity, smoothTime);
    }

    private void Rotation()
    {
        float turnTemp = turnSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, turnTemp * canMove, 0);

        if (canMove == 0 && checkBounce == false)
        {
            transform.LookAt(GetComponentInChildren<HazardManager>().hit.collider.transform.position);
            rotationFix = true;
        }
        else if (canMove == 1 && rotationFix == true)
        {
            rotationFix = false;
            Vector3 temp = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, temp.y, temp.z);
        }
    }

    public Vector3 Tilt()
    {
        float forwardTiltAngle = Vector3.Dot(parentRB.velocity, transform.forward);
        float rightTiltAngle = Vector3.Dot(parentRB.velocity, transform.right * -1);

        Vector3 desiredTiltAngle = new Vector3(forwardTiltAngle * theta, 0, rightTiltAngle * theta);

        tiltingChild.transform.localEulerAngles = desiredTiltAngle;

        return desiredTiltAngle;
    }

    private void Camera()
    {
        if (Input.GetButtonDown("ToggleCam"))
        {
            thirdPerson = !thirdPerson;
        }

        if (Input.GetMouseButton(1))
        {
            turnSpeed = 0;
            freelook = true;
            thirdPerson = false;

            camXAxisRotation += Input.GetAxis("Mouse Y") * -camTurnSpeed; camYAxisRotation += Input.GetAxis("Mouse X") * camTurnSpeed;
            float camXAxisRotationTemp = Mathf.Clamp(camXAxisRotation, -camMaxVerticalFreeLookAngle, camMaxVerticalFreeLookAngle);
            firstPersonCam.transform.localEulerAngles = new Vector3(camXAxisRotationTemp, camYAxisRotation, 0);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            turnSpeed = camTurnSpeed;
            freelook = false;
            firstPersonCam.transform.localEulerAngles = Vector3.zero;
            camXAxisRotation = 0;
            camYAxisRotation = 0;
        }

        if (thirdPerson)
        {
            thirdPersonCam.SetActive(true);
            thirdPersonCam.GetComponent<AudioListener>().enabled = true;
            firstPersonCam.SetActive(false);
            firstPersonCam.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            thirdPersonCam.SetActive(false);
            thirdPersonCam.GetComponent<AudioListener>().enabled = false;
            firstPersonCam.SetActive(true);
            firstPersonCam.GetComponent<AudioListener>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision obstacle)
    {
        droneHits++;       
        if(droneHits == 3)
        {
            GetComponent<DroneUI>().levelManagerScript.SceneSelectMethod(3);
        }

        Vector3 angleAtCollision = currentVelocity.normalized;
        Vector3 normalAngleAtCollision = obstacle.contacts[0].normal.normalized;
        Vector3 directionFix = new Vector3(1,1,1);

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
        
        Vector3 bounceAngle = (normalAngleAtCollision + Vector3.Scale(angleAtCollision,directionFix)) * 360;      
        parentRB.AddForce(bounceAngle * pushBackForce);       
    }   
}
