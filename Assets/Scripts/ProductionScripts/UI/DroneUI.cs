using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    //Class References
    [Header("Class References")]
    private DroneController droneController;

    //References
    [Header("UI References")]
    public GameObject altitudeRef;   //Reference to altitude slider of the drone UI                           
    public GameObject rangeRef;      //Reference to range metre of the drone UI  
    public GameObject satisfactionRef;  //Reference to satisfaction slider of the drone UI  
    public GameObject artificialHorizonCircle;  //Reference to circle of the artificial horizon of the drone UI  
    public GameObject artificialHorizonLine;    //Reference to horizontal line of the artificial horizon of the drone UI  
    public Image staticEffect;     //Reference to the signal strength overlay image of the drone UI          
    private Vector3 horPosVar;   //Vector 3 to alter the position of the horizontal line of the artificial horizon of the drone UI

    //UI Variables
    [Header("UI Variables")]
    private float altitude;   //Altitude variable
    private float range;   //Range variable    
    private Slider satisfactionSliderRef;   //Reference to the slider of the satisfaction bar
    public List<Image> workersFacesImageList;  //List of images of the worker's faces
    public List<Image> signalImageList;   //List of images used to show signal strength

    private void Awake()
    {
        droneController = this.GetComponent<DroneController>();
        satisfactionSliderRef = satisfactionRef.GetComponentInChildren<Slider>();   //Gets the satisfaction slider
        satisfactionSliderRef.value = droneController.satisfactionValue;   //Sets the value of the satisfaction slider
        altitudeRef.GetComponentInChildren<Slider>().maxValue = droneController.flightCeiling;  //Sets the max value of the altitude slider equal to that of the max height the drone can fly at
        rangeRef.GetComponentInChildren<Slider>().maxValue = 1;  //Sets the maximum value of the range metre
        horPosVar = artificialHorizonLine.GetComponentInParent<Transform>().position;  //Sets the vector 3 of the horizontal line
    }
    private void Update() 
    {
        ArtificialHorizon();
        Altitude();        
        Satisfaction();
        Range();
    }

    public void EnableUI(bool enable)
    {
        artificialHorizonCircle.SetActive(enable);
        artificialHorizonLine.SetActive(enable);
    }

    /// <summary>
    /// Method tha t controls the horizontal horizon of the drone's UI
    /// </summary>
    private void ArtificialHorizon()
    {
        if (artificialHorizonLine.GetComponent<Image>().enabled == true)  //If the artificial horizon is active
        {
            artificialHorizonLine.transform.localEulerAngles = new Vector3(0, 0, droneController.droneMovement.Tilt().z * -1); //Sets the tilt of the artificial horizon about the z-axis counter to that of the drone's tilt
            artificialHorizonLine.transform.position = new Vector3(horPosVar.x, horPosVar.y + (droneController.droneMovement.Tilt().x * 40 / droneController.maxTiltAngle), 0);  //Sets the vertical position of the artificial horizon based on the drone's forward tilt angle
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
    /// Method to handle the satisfaction UI 
    /// </summary>
    private void Satisfaction()
    {
        droneController.satisfactionValue -= droneController.satisfactionDropRate * Time.deltaTime; //Removes satisfaction over time
        satisfactionSliderRef.value = droneController.satisfactionValue;  //Sets the new satisfaction value to the UI slider

        satisfactionRef.GetComponentInChildren<Image>().color = droneController.satisfactionGradient.Evaluate(satisfactionSliderRef.normalizedValue);  //Sets the colour of the satisfaction slider based on it's value
        int x; //Integer index to determine which worker face image should be displayed based on the satisfaction level

        //Sets the index for which image shoulde be selected
        if(droneController.satisfactionValue < 20)
        {
            x = 2;
        }
        else if (droneController.satisfactionValue < 55)
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
        if (droneController.satisfactionValue >= 100 || droneController.satisfactionValue <= 0)
        {
            droneController.gameManager.levelManager.SceneSelectMethod(3);
        }
    }

    /// <summary>
    /// Method to determine the distance the drone has flown from it's origin and apply a static effect if it's range is too far
    /// </summary>
    private void Range()
    {
        range = Vector3.Distance(transform.position, droneController.droneMovement.startPosition);   //Gets the distance the drone has flown from it's origin
        rangeRef.GetComponentInChildren<Text>().text = Mathf.RoundToInt(100 - ((range / droneController.maxRange) * 100)) + "%";   //Sets the range text equal to the percentage of the maximum range the drone has flown

        float staticEffectIntensity = Mathf.Pow(((range - (droneController.maxRange * droneController.signalLossPoint)) / (droneController.maxRange * (1 - (droneController.signalLossPoint / 1)))), 1.5f);  //Sets an exponentialy increasing intensity for the static effect after the drone has flown past the point where it begins to lose signal

        if (range > droneController.maxRange * droneController.signalLossPoint)  //If the drone has flown past the point where it begins to lose signal
        {
            staticEffect.GetComponent<Image>().color = new Color(255, 255, 255, staticEffectIntensity); //Applies a static effect over the screen

            if (range > droneController.maxRange)  //If the drone flies outside it max range and loses signalk
            {
                droneController.droneMovement.parentRB.transform.position = droneController.droneMovement.startPosition;  //Resets the drone's position
                droneController.droneMovement.parentRB.velocity = Vector3.zero;  //Resets the drone's velocity
            }
        }
        else   //Else removes the static effect
        {
            staticEffect.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        }

        float signalRatio = range / droneController.maxRange;  //Ratio to determine how much signal strength it has based on the distance flown
        int x; //Temp variable used for indexing

        //Statements to determine which index should be used based on the signal strength
        if (signalRatio < 0.25)
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
}

