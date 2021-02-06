using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    //Class References
    private DroneController droneController;

    //References
    public GameObject altitudeRef;   //Reference to altitude slider of the drone UI                           
    public GameObject rangeRef;      //Reference to range metre of the drone UI  
    public GameObject satisfactionRef;  //Reference to satisfaction slider of the drone UI  
    public GameObject horizonCircleRef;  //Reference to circle of the artificial horizon of the drone UI  
    public GameObject horizonLineRef;    //Reference to horizontal line of the artificial horizon of the drone UI  
    public GameObject horizonPointerRef;   //Reference to vertical line of the artificial horizon of the drone UI  
    public GameObject[] artificialHorizonRef;   //List of references to the vertical line, horizontal line and angle markers of the artificial horizon of the drone UI
    public Image signalUI;     //Reference to the signal strength overlay image of the drone UI          
    private Vector3 horPosVar;   //Vector 3 to alter the position of the horizontal line of the artificial horizon of the drone UI

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
        droneController = GetComponent<DroneController>();
        satisfactionSliderRef = satisfactionRef.GetComponentInChildren<Slider>();   //Gets the satisfaction slider
        satisfactionSliderRef.value = satisfactionValue;   //Sets the value of the satisfaction slider
        maxRange = droneController.maxRange;   //Sets the max range
        altitudeRef.GetComponentInChildren<Slider>().maxValue = droneController.flightCeiling;  //Sets the max value of the altitude slider equal to that of the max height the drone can fly at
        rangeRef.GetComponentInChildren<Slider>().maxValue = 1;  //Sets the maximum value of the range metre
        horPosVar = horizonLineRef.GetComponentInParent<Transform>().position;  //Sets the vector 3 of the horizontal line
    }
    private void Update()
    {
        Range();
        Altitude();
        ArtificialHorizon();
        Satisfaction();
    }

    /// <summary>
    /// Method to determine the distance the drone has flown from it's origin and apply a static effect if it's range is too far
    /// </summary>
    private void Range()
    {
        range = Vector3.Distance(transform.position, droneController.droneMovement.startPosition);   //Gets the distance the drone has flown from it's origin
        rangeRef.GetComponentInChildren<Text>().text = Mathf.RoundToInt(100 - ((range / maxRange) * 100)) + "%";   //Sets the range text equal to the percentage of the maximum range the drone has flown

        float staticEffectIntensity = Mathf.Pow(((range - (maxRange * signalLossPoint)) / (maxRange * (1 - (signalLossPoint / 1)))), 1.5f);  //Sets an exponentialy increasing intensity for the static effect after the drone has flown past the point where it begins to lose signal

        if (range > maxRange * signalLossPoint)  //If the drone has flown past the point where it begins to lose signal
        {
            signalUI.GetComponent<Image>().color = new Color(255, 255, 255, staticEffectIntensity); //Applies a static effect over the screen

            if (range > maxRange)  //If the drone flies outside it max range and loses signalk
            {
                droneController.droneMovement.parentRB.transform.position = droneController.droneMovement.startPosition;  //Resets the drone's position
                droneController.droneMovement.parentRB.velocity = Vector3.zero;  //Resets the drone's velocity
            }
        }
        else   //Else removes the static effect
        {
            signalUI.GetComponent<Image>().color = new Color(255, 255, 255, 0f); 
        }

        float signalRatio = range / maxRange;  //Ratio to determine how much signal strength it has based on the distance flown
        int x; //Temp variable used for indexing

        //Statements to determine which index should be used based on the signal strength
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

        //Enables different images for the range UI dependent on the distance the droen has flown
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
    }

    /// <summary>
    /// Method to determine the altitude the drone is at and set the UI elements for this
    /// </summary>
    private void Altitude()
    {
        altitude = Mathf.RoundToInt(transform.position.y);  //Gets the drone's altitude
        altitudeRef.GetComponentInChildren<Text>().text = altitude + "M";  //Sets the text of the altitude slider 
        altitudeRef.GetComponentInChildren<Slider>().value = altitude;  //Sets the value of the altitude slider
    }

    /// <summary>
    /// Method tha t controls the horizontal horizon of the drone's UI
    /// </summary>
    private void ArtificialHorizon()
    {
        if (droneController.droneMovement.thirdPerson == false) //Sets the artificial horizon active if in first person
        {
            for (int i = 0; i < artificialHorizonRef.Length; i++)
            {
                artificialHorizonRef[i].GetComponent<Image>().enabled = true;
            }
        }
        else if (droneController.droneMovement.thirdPerson == true) //Sets the artificial horizon inactive if in third person
        {
            for (int i = 0; i < artificialHorizonRef.Length; i++)
            {
                artificialHorizonRef[i].GetComponent<Image>().enabled = false;
            }
        }

        if (horizonLineRef.GetComponent<Image>().enabled == true)  //If the artificial horizon is active
        {
            horizonLineRef.transform.localEulerAngles = new Vector3(0, 0, droneController.droneMovement.Tilt().z * -1); //Sets the tilt of the artificial horizon about the z-axis counter to that of the drone's tilt
            horizonLineRef.transform.position = new Vector3(horPosVar.x, horPosVar.y + (droneController.droneMovement.Tilt().x * 40 / droneController.maxTiltAngle), 0);  //Sets the vertical position of the artificial horizon based on the drone's forward tilt angle
        }
    }

    /// <summary>
    /// Method to handle the satisfaction UI 
    /// </summary>
    private void Satisfaction()
    {
        satisfactionValue -= satisfactionDropRate * Time.deltaTime; //Removes satisfaction over time
        satisfactionSliderRef.value = satisfactionValue;  //Sets the new satisfaction value to the UI slider

        satisfactionRef.GetComponentInChildren<Image>().color = satisfactionGradient.Evaluate(satisfactionSliderRef.normalizedValue);  //Sets the colour of the satisfaction slider based on it's value
        int x; //Integer index to determine which worker face image should be displayed based on the satisfaction level

        //Sets the index for which image shoulde be selected
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
        
        //Enables or disables worker face images based on  the satisfaction
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

        //If satisfaction of 100 is achieved the score scene is loaded
        if (satisfactionValue >= 100 || satisfactionValue <= 0)
        {
            droneController.gameManager.levelManager.SceneSelectMethod(3);
        }
    }
}

