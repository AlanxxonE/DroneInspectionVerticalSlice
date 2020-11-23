using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    //References
    public GameObject altitudeRef;
    public GameObject rangeRef;
    public GameObject satisfactionRef;
    public GameObject horizonCircleRef;
    public GameObject horizonLineRef;
    public GameObject horizonPointerRef;
    public GameObject[] artificialHorizonRef;
    public Image signalUI;
    public DroneController droneController;
    private Vector3 horPosVar;
    public LevelManager levelManagerScript;

    //UI Variables
    private float altitude;
    private float range;
    [Range(0f, 1f)]
    [Tooltip("Sets percentage of the maximum range of the drone at which the drone signal begins to fade and the static effect begins to increase")]
    public float signalLossPoint;
    [Range(0f, 1f)]
    [Tooltip("Sets the rate of satisfaction loss at rate of 0-1 ticks per second. Total ticks = 100.")]
    public float satisfactionDropRate;
    public float satisfactionValue = 100f;
    private Slider satisfactionSliderRef;

    //RangeVariables
    private float distanceValue = 200f;
    private float maxRange;

    private void Start()
    {
        satisfactionSliderRef = satisfactionRef.GetComponentInChildren<Slider>();
        satisfactionSliderRef.value = satisfactionValue;
        maxRange = droneController.maxRange;
        altitudeRef.GetComponentInChildren<Slider>().maxValue = droneController.flightCeiling;
        rangeRef.GetComponentInChildren<Slider>().maxValue = 1;
        horPosVar = horizonLineRef.GetComponentInParent<Transform>().position;
    }
    private void Update()
    {
        Range();
        Altitude();
        ArtificialHorizon();
        Satisfaction();
    }

    private void Range()
    {
        range = Vector3.Distance(transform.position, droneController.startPosition);
        rangeRef.GetComponentInChildren<Text>().text = "SIGNAL STRENGTH\n" + Mathf.RoundToInt(100 - ((range / maxRange) * 100)) + "%";
        rangeRef.GetComponentInChildren<Slider>().value = range / maxRange;

        float staticEffectIntensity = Mathf.Pow(((range - (maxRange * signalLossPoint)) / (maxRange * (1 - (signalLossPoint / 1)))), 1.5f);
        if (range > maxRange * signalLossPoint)
        {
            signalUI.GetComponent<Image>().color = new Color(255, 255, 255, staticEffectIntensity);

            if (range > maxRange)
            {
                droneController.parentRB.transform.position = droneController.startPosition;
                droneController.parentRB.velocity = Vector3.zero;
            }
        }

        else
        {
            signalUI.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        }
    }

    private void Altitude()
    {
        altitude = Mathf.RoundToInt(transform.position.y);
        altitudeRef.GetComponentInChildren<Text>().text = "ALTITUDE\n" + altitude + "M";
        altitudeRef.GetComponentInChildren<Slider>().value = altitude;
    }

    private void ArtificialHorizon()
    {
        if (droneController.thirdPerson == false)
        {
            for (int i = 0; i < artificialHorizonRef.Length; i++)
            {
                artificialHorizonRef[i].GetComponent<Image>().enabled = true;
            }


        }
        else if (droneController.thirdPerson == true)
        {
            for (int i = 0; i < artificialHorizonRef.Length; i++)
            {
                artificialHorizonRef[i].GetComponent<Image>().enabled = false;
            }
        }

        if (horizonLineRef.GetComponent<Image>().enabled == true)
        {
            horizonLineRef.transform.localEulerAngles = new Vector3(0, 0, droneController.Tilt().z * -1);
            horizonLineRef.transform.position = new Vector3(horPosVar.x, horPosVar.y + (droneController.Tilt().x * 40 / droneController.maxTiltAngle), 0);
        }
    }

    private void Satisfaction()
    {
        satisfactionValue -= satisfactionDropRate * Time.deltaTime;
        satisfactionSliderRef.value = satisfactionValue;

        if (satisfactionValue >= 100 || satisfactionValue <= 0)
        {
            levelManagerScript.SceneSelectMethod(0);
        }
    }
}

