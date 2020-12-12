using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    //References
    public GameObject altitudeRef;   //Reference to altitude slider of the drone UI                           
    public GameObject rangeRef;      //Reference to range metre of the drone UI  
    public GameObject satisfactionRef;  //Reference to satisfaction slider of the drone UI  
    public GameObject horizonCircleRef;  //Reference to circle of the artificial horizon of the drone UI  
    public GameObject horizonLineRef;    //Reference to horizontal line of the artificial horizon of the drone UI  
    public GameObject horizonPointerRef;   //Reference to vertical line of the artificial horizon of the drone UI  
    public GameObject[] artificialHorizonRef;   //List of references to the vertical line, horizontal line and angle markers of the artificial horizon of the drone UI
    public Image signalUI;     //Reference to the signal strength overlay image of teh drone UI          
    public DroneController droneController;   //Reference to the drone controller script
    private Vector3 horPosVar;   //Vector 3 to alter the position of the horizontal line of the artificial horizon of the drone UI
    public LevelManager levelManagerScript;   //Reference to the level manager script

    //UI Variables
    private float altitude;   //Altitude variable
    private float range;   //Range variable
    [Range(0f, 1f)]
    [Tooltip("Sets percentage of the maximum range of the drone at which the drone signal begins to fade and the static effect begins to increase")]
    public float signalLossPoint;   //Sets percentage of the maximum range of the drone at which the drone signal begins to fade and the static effect begins to increase
    [Range(0f, 1f)]
    [Tooltip("Sets the rate of satisfaction loss at rate of 0-1 ticks per second. Total ticks = 100.")]
    public float satisfactionDropRate;   //Sets the rate of satisfaction loss at rate of 0-1 ticks per second. Total ticks = 100.
    public float satisfactionValue = 50f;   //Initial value of worker satisfaction
    private Slider satisfactionSliderRef;   //Reference to the slider of the satisfaction bar
    public Gradient satisfactionGradient;   //Reference to the colour gradient of the slider of the satisfaction bar
    public List<Image> workersFacesImageList;  //List of images of the worker's faces
    public List<Image> signalImageList;   //List of images used to show signal strength

    //RangeVariables
    private float maxRange;   //Float to indicate the max range the drone can fly from it's origin

    private void Start()
    {
        satisfactionSliderRef = satisfactionRef.GetComponentInChildren<Slider>();   //Gets the satisfaction slider
        satisfactionSliderRef.value = satisfactionValue;   //Sets the value of the satisfaction slider
        maxRange = droneController.maxRange;   //Sets the max range
        altitudeRef.GetComponentInChildren<Slider>().maxValue = droneController.flightCeiling;  //Sets the max value of the altitude slider equal to that of the max height the drone can fly at
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
        rangeRef.GetComponentInChildren<Text>().text = Mathf.RoundToInt(100 - ((range / maxRange) * 100)) + "%";

        float staticEffectIntensity = Mathf.Pow(((range - (maxRange * signalLossPoint)) / (maxRange * (1 - (signalLossPoint / 1)))), 1.5f);

        float signalRatio = range / maxRange;
        int x;

        if(signalRatio < 0.25)
        {
            x = 0;
        }
        else if (signalRatio < 0.50)
        {
            x = 1;
        }
        else if (signalRatio < 0.75)
        {
            x = 2;
        }
        else
        {
            x = 3;
        }

        for (int i = 0; i < signalImageList.Count; i++)
        {
            if (x == i)
            {
                signalImageList[i].enabled = true;
            }
            else
            {
                signalImageList[i].enabled = false;
            }
        }
        
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
        altitudeRef.GetComponentInChildren<Text>().text = altitude + "M";
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

        satisfactionRef.GetComponentInChildren<Image>().color = satisfactionGradient.Evaluate(satisfactionSliderRef.normalizedValue);
        int x;

        if(satisfactionValue < 20)
        {
            x = 2;
        }
        else if (satisfactionValue < 55)
        {
            x = 1;
        }
        else
        {
            x = 0;
        }

        for ( int i = 0; i < workersFacesImageList.Count; i++)
        {
            if (x == i)
            {
                workersFacesImageList[i].enabled = true;
            }
            else
            {
                workersFacesImageList[i].enabled = false;
            }
        }

        if (satisfactionValue >= 100 || satisfactionValue <= 0)
        {
            levelManagerScript.SceneSelectMethod(3);
        }
    }
}

