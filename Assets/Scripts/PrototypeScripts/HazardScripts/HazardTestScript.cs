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
    GameObject TornWire;
    GameObject TornBox;
    bool checkSwapWire = false;

    // Start is called before the first frame update
    void Start()
    {
        UnscrewBolt = GameObject.FindGameObjectWithTag("UnscrewBolt");

        FixedWire = GameObject.FindGameObjectWithTag("FixedWire");
        WireBox = GameObject.FindGameObjectWithTag("WireBox");
        TornWire = GameObject.FindGameObjectWithTag("TornWire");
        TornBox = GameObject.FindGameObjectWithTag("TornBox");

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

        EventSystem.current.RaycastAll(pointerData, pointerHitList);

        if (Input.GetMouseButton(0))
        {
            if (HazardPopUpRef != null)
            {
                for (int i = 0; i < pointerHitList.Count; i++)
                {
                    if (pointerHitList[i].gameObject.tag == "FixedWire" && (TornWire.transform.position == TornBox.transform.position) && checkSwapWire == false)
                    {
                        FixedWire.transform.position = currentMousePosition;
                    }

                    if(pointerHitList[i].gameObject.tag == "TornWire" && (FixedWire.transform.position == WireBox.transform.position))
                    {
                        TornWire.transform.position = currentMousePosition;
                    }
                }
            }
        }
        else
        { 
            for (int i = 0; i < pointerHitList.Count; i++)
            {
                if (pointerHitList[i].gameObject.tag == "BinBox" && (TornWire.transform.position != TornBox.transform.position))
                {
                    TornWire.SetActive(false);
                }

                if(pointerHitList[i].gameObject.tag == "TornBox" && (FixedWire.transform.position != WireBox.transform.position) && TornWire.activeSelf == false)
                {
                    checkSwapWire = true;

                    FixedWire.transform.position = TornBox.transform.position + new Vector3(0,50);

                    HazardPopUpRef.GetComponentInChildren<Slider>().value += 80f;
                }
            }

            if (HazardPopUpRef != null && (FixedWire.transform.position != WireBox.transform.position || TornWire.transform.position != TornBox.transform.position))
            {
                if (checkSwapWire == false)
                {
                    FixedWire.transform.position = WireBox.transform.position;
                }

                TornWire.transform.position = TornBox.transform.position;
            }
        }
    }
}
