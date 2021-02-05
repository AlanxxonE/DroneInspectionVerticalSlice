using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DroneController3D : MonoBehaviour
{

    private Rigidbody rb;

    private Vector3 desiredVelocity;
    public float velocity= 20000f;
    private Vector3 refVelocity;
    public float movementSmoothing = 0.2f;

    public float turnSpeed = 2f;
    private float turnTemp;
    private float turnTempVertical;

    public bool thirdPerson = false;

    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;

    public float rotationAngle = 20f;
    public GameObject rotatingBody;
    public GameObject originPointRef;
    public GameObject signalUI;
    public float decreasingValueX = 90;
    public float decreasingValueXPro = 0;
    public float decreasingValueYPro = 0;
    public float distanceValue = 100f;

    public bool checkMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        CameraCheck();
        Debug.Log("Controls: ");
        Debug.Log("Move: WASD");
        Debug.Log("Turn: Mouse");
        Debug.Log("Toggle Camera: C");
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkMovement == true)
        {
            if(GetComponentInChildren<HazardManager>().stopMovement == false)
            {
                if (Vector3.Distance(transform.position, originPointRef.transform.position) < distanceValue)
                {
                    if (Vector3.Distance(transform.position, originPointRef.transform.position) > distanceValue / 1.25f)
                    {
                        signalUI.GetComponent<Image>().color = new Color(255, 255, 255, (Vector3.Distance(transform.position, originPointRef.transform.position)) / 100);
                    }
                    else
                    {
                        signalUI.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
                    }

                    Cursor.lockState = CursorLockMode.Locked;
                    Movement();
                    Turning();
                }
                else
                {
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                rb.velocity = Vector3.SmoothDamp(rb.velocity * 0.8f, Vector3.zero, ref refVelocity, movementSmoothing);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void Movement()
    {
        Vector3 desiredVelocityX = Input.GetAxisRaw("MovementX") * transform.right;
        Vector3 desiredVelocityY = Input.GetAxisRaw("MovementY") * transform.forward;
        Vector3 desiredVelocityZ = Input.GetAxisRaw("MovementZ") * transform.up;

        //GetComponentInChildren<Camera>().transform.localEulerAngles = new Vector3 (10,0,0);

        //#region HozirontalTilt
        //if (desiredVelocityX.x == 0)
        //{
        //    if (decreasingValueX < 90)
        //    {
        //        decreasingValueX += 0.1f;
        //    }
        //    if (decreasingValueX > 90)
        //    {
        //        decreasingValueX -= 0.1f;
        //    }
        //}

        //if (desiredVelocityX.x < 0)
        //{
        //    if (rotatingBody.transform.localEulerAngles.x > 60)
        //    {
        //        if (decreasingValueX >= 60)
        //        {
        //            decreasingValueX -= 0.1f;
        //        }
        //    }
        //}

        //if (desiredVelocityX.x > 0)
        //{

        //    if (rotatingBody.transform.localEulerAngles.x < 120)
        //    {
        //        if (decreasingValueX <= 120)
        //        {
        //            decreasingValueX += 0.1f;
        //        }
        //    }
        //}

        //rotatingBody.transform.localEulerAngles = new Vector3(decreasingValueX, 90, 90); ;
        //#endregion

        #region movementTilt

        //HorizontalMovement

        if (desiredVelocityX.x == 0)
        {
            if (decreasingValueXPro < 0)
            {
                decreasingValueXPro += 0.1f;
            }
            if (decreasingValueXPro > 0)
            {
                decreasingValueXPro -= 0.1f;
            }
        }

        if (desiredVelocityX.x < 0)
        {
            if (rotatingBody.transform.localEulerAngles.z < 30)
            {
                if (decreasingValueXPro <= 30)
                {
                    decreasingValueXPro += 0.1f;
                }
            }
        }

        if (desiredVelocityX.x > 0)
        {

            if (rotatingBody.transform.localEulerAngles.z > -30)
            {
                if (decreasingValueXPro >= -30)
                {
                    decreasingValueXPro -= 0.1f;
                }
            }
        }

        //VerticalMovement

        if (desiredVelocityY.z == 0)
        {
            if (decreasingValueYPro < 0)
            {
                decreasingValueYPro += 0.1f;
            }
            if (decreasingValueYPro > 0)
            {
                decreasingValueYPro -= 0.1f;
            }
        }

        if (desiredVelocityY.z < 0)
        {
            if (rotatingBody.transform.localEulerAngles.x > -10)
            {
                if (decreasingValueYPro >= -10)
                {
                    decreasingValueYPro -= 0.1f;
                }
            }
        }

        if (desiredVelocityY.z > 0)
        {
            if (rotatingBody.transform.localEulerAngles.z < 10)
            {
                if (decreasingValueYPro <= 10)
                {
                    decreasingValueYPro += 0.1f;
                }
            }
        }

        rotatingBody.transform.localEulerAngles = new Vector3(decreasingValueYPro, 0, decreasingValueXPro);

        #endregion

        desiredVelocity = (desiredVelocityX * velocity) + (desiredVelocityY * velocity) + (desiredVelocityZ * velocity);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, desiredVelocity * Time.deltaTime, ref refVelocity, movementSmoothing);
    }

    private void Turning()
    {
        turnTemp = turnSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, turnTemp, 0);

        turnTempVertical = turnSpeed * Input.GetAxis("Mouse Y") * -1;
        thirdPersonCam.transform.Rotate(turnTempVertical, 0, 0);
    }

    private void CameraCheck()
    {
        if (Input.GetButtonDown("ToggleCam"))
        {
            thirdPerson = !thirdPerson;
        }

        if (thirdPerson)
        {
            thirdPersonCam.SetActive(true);
            firstPersonCam.SetActive(false);
        }
        else
        {
            thirdPersonCam.SetActive(false);
            firstPersonCam.SetActive(true);
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("BounceBack");
    }

    IEnumerator BounceBack()
    {
        rb.velocity *= -1;
        checkMovement = false;

        yield return new WaitForSeconds(0.2f);
        checkMovement = true;
    }
}


