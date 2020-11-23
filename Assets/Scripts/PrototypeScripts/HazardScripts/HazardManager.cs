using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardManager : MonoBehaviour
{

    //References 
    public DroneUI droneUIScript;
    //
    public RaycastHit hit;
    RaycastHit check;
    public Image uIRef;
    public bool stopMovement = false;
    public GameObject hazardRef;
    public Vector3 lastMousePosition;
    public Vector3 currentMousePosition;

    //
    public bool CHECK = true;
    float rotationAngle = 0;
    float lastRotationAngle = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentMousePosition = Input.mousePosition;

        RaycastCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false)
        {
            ShootRaycast();
        }

        if (stopMovement == true)
        {
            FinishHazard();

            CheckMouseState();
        }
    }

    public void RaycastCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out check, 10f);
        if (check.collider != null && check.collider.tag == "Hazard")
        {
            uIRef.color = Color.red;
        }
        else if (check.collider == null)
        {
            uIRef.color = Color.gray;
        }
    }

    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 10f);
        if (hit.collider != null && hit.collider.tag == "Hazard")
        {
            stopMovement = true;
            Debug.Log(hit.collider);
            hazardRef = hit.collider.GetComponent<HazardTestScript>().HazardPopUpRef;
            hazardRef.SetActive(true);
            hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", true);
        }
    }

    public void FinishHazard()
    {
        if (hazardRef.GetComponentInChildren<Slider>().value >= 100)
        {
            hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);
            stopMovement = false;
            droneUIScript.satisfactionValue += 80f; //////This value needs changed
        }
        else if (hazardRef.GetComponentInChildren<Slider>().value != 0)
        {
            hazardRef.GetComponentInChildren<Slider>().value -= Time.deltaTime * 5f;
        }
        else
        {
            hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);
            stopMovement = false;
        }
    }

    public void CheckMouseState()
    {
        if (Input.GetMouseButton(0))
        {
            if (hazardRef != null)
            {
                GameObject UnscrewBolt = GameObject.FindGameObjectWithTag("UnscrewBolt");

                Vector2 MousePoint = new Vector2(Mathf.Abs(currentMousePosition.x - UnscrewBolt.transform.position.x), Mathf.Abs(currentMousePosition.y - UnscrewBolt.transform.position.y));

                lastRotationAngle = rotationAngle;

                int MaxDistance = 200;

                if (MousePoint.x < MaxDistance && MousePoint.y < MaxDistance)
                {
                    rotationAngle = Mathf.Atan2(currentMousePosition.y - UnscrewBolt.transform.position.y, currentMousePosition.x - UnscrewBolt.transform.position.x) * Mathf.Rad2Deg;

                    UnscrewBolt.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0, 0, rotationAngle);

                    if (rotationAngle < 0 && rotationAngle > -180)
                    {
                        rotationAngle = 360 - Mathf.Abs(rotationAngle);
                    }

                    Debug.Log(rotationAngle);

                    if (lastRotationAngle > rotationAngle)
                    {
                        hazardRef.GetComponentInChildren<Slider>().value += 0.2f;
                    }
                    else if (lastRotationAngle < rotationAngle)
                    {
                        hazardRef.GetComponentInChildren<Slider>().value -= 0.2f;
                    }
                }
            }
        }

    }
}
