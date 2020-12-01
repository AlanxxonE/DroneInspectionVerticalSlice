using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardTestScript : MonoBehaviour
{

    public HazardManager hazardManagerRef;

    public GameObject HazardPopUpRef;

    public string hazardTag;

    Vector3 currentMousePosition;
    Vector3 lastMousePosition;

    //HazardMethod
    float rotationAngle = 0;
    float lastRotationAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        hazardTag = gameObject.tag;
    }

    //When the raycast hit the object, the following code gets executed
    private void OnEnable()
    {
        HazardPopUpRef.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hazardManagerRef.stopMovement == true)
        {
            switch (hazardTag)
            {
                case "ScaffoldHazard":

                    currentMousePosition = Input.mousePosition;

                    ScaffoldHazard();

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
}
