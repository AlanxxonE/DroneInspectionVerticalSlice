using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardTestScript : MonoBehaviour
{
    public PointerEventData pointerData = new PointerEventData(EventSystem.current);

    public List<RaycastResult> pointerHitList = new List<RaycastResult>();

    public HazardManager hazardManagerRef;

    public GameObject HazardPopUpRef;

    public string hazardTag;

    Vector3 currentMousePosition;
    Vector3 lastMousePosition;

    //HazardMethod
    float rotationAngle = 0;
    float lastRotationAngle = 0;
    GameObject UnscrewBolt;

    //CraneMethod
    GameObject FixedWire;
    GameObject WireBox;
    Vector3 originalFixedWirePosition;

    // Start is called before the first frame update
    void Start()
    {
        UnscrewBolt = GameObject.FindGameObjectWithTag("UnscrewBolt");

        FixedWire = GameObject.FindGameObjectWithTag("FixedWire");
        WireBox = GameObject.FindGameObjectWithTag("WireBox");

        HazardPopUpRef.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hazardManagerRef.stopMovement == true)
        {
            currentMousePosition = Input.mousePosition;

            switch (hazardTag)
            {
                case "ScaffoldHazard":

                    ScaffoldHazard();

                    break;
                case "CraneHazard":

                    CraneHazard();

                    break;
                default:
                    break;
            }
        }
    }

    public void ScaffoldHazard()
    {
        if (Input.GetMouseButton(0))
        {

            if (HazardPopUpRef != null)
            {
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

                    if (lastRotationAngle > rotationAngle)
                    {
                        HazardPopUpRef.GetComponentInChildren<Slider>().value += 0.2f;
                    }
                    else if (lastRotationAngle < rotationAngle)
                    {
                        HazardPopUpRef.GetComponentInChildren<Slider>().value -= 0.2f;
                    }
                }
            }
        }

    }

    public void CraneHazard()
    {
        pointerData.position = currentMousePosition;

        if(originalFixedWirePosition != WireBox.transform.position)
        {
            originalFixedWirePosition = WireBox.transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            if (HazardPopUpRef != null)
            {
                FixedWire.transform.position = currentMousePosition;
            }
        }
        else
        {
            if (HazardPopUpRef != null)
            {
                FixedWire.transform.position = originalFixedWirePosition;
            }
        }

        Debug.Log(EventSystem.current.RaycastAll(pointerData, pointerHitList));
    }
}
