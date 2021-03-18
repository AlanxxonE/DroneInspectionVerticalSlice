using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    /// <summary>
    /// Responsible for handling the drone UI elements
    /// </summary>
    
    //Class reference 
    [Tooltip("Reference to the UIManager class")]
    public UIManager UIManager;

    //UI Variables
    private float altitude;   //Altitude variable
    private float range;   //Range variable    
    private Slider satisfactionSliderRef;   //Reference to the slider of the satisfaction bar
    private Vector3 horPosVar;   //Vector 3 to alter the position of the horizontal line of the artificial horizon of the drone UI    

    private void Awake()
    {      
        satisfactionSliderRef = UIManager.satisfactionRef.GetComponentInChildren<Slider>();   //Gets the satisfaction slider
        satisfactionSliderRef.value = UIManager.satisfactionValue;   //Sets the value of the satisfaction slider
        UIManager.altitudeRef.GetComponentInChildren<Slider>().maxValue = UIManager.gameManager.droneController.flightCeiling;  //Sets the max value of the altitude slider equal to that of the max height the drone can fly at
        UIManager.rangeRef.GetComponentInChildren<Slider>().maxValue = 1;  //Sets the maximum value of the range metre
        horPosVar = UIManager.artificialHorizonLine.GetComponentInParent<Transform>().position;  //Sets the vector 3 of the horizontal line
    }
    private void Update() 
    {
        ArtificialHorizon();
        Altitude();        
        Satisfaction();
        Range();
    }

    /// <summary>
    /// Method to enable/disable UI elements
    /// </summary>
    /// <param name="enable"></param>
    public void EnableUI(bool enable)
    {
        UIManager.artificialHorizonCircle.SetActive(enable);
        UIManager.artificialHorizonLine.SetActive(enable);
    }

    /// <summary>
    /// Method that controls the artificial horizon of the drone's UI
    /// </summary>
    private void ArtificialHorizon()
    {
        if (UIManager.artificialHorizonLine.GetComponent<Image>().enabled == true)  //If the artificial horizon is active
        {
            UIManager.artificialHorizonLine.transform.localEulerAngles = new Vector3(0, 0, UIManager.gameManager.droneController.droneMovement.Tilt().z * -1); //Sets the tilt of the artificial horizon about the z-axis counter to that of the drone's tilt
            UIManager.artificialHorizonLine.transform.position = new Vector3(horPosVar.x, horPosVar.y + (UIManager.gameManager.droneController.droneMovement.Tilt().x * 40 / UIManager.gameManager.droneController.maxTiltAngle), 0);  //Sets the vertical position of the artificial horizon based on the drone's forward tilt angle
        }
    }

    /// <summary>
    /// Method to determine the altitude the drone is at and set the UI elements for this
    /// </summary>
    private void Altitude()
    {
        altitude = Mathf.RoundToInt(UIManager.gameManager.droneController.transform.position.y);  //Gets the drone's altitude
        UIManager.altitudeRef.GetComponentInChildren<Text>().text = altitude + "M";  //Sets the text of the altitude slider 
        UIManager.altitudeRef.GetComponentInChildren<Slider>().value = altitude;  //Sets the value of the altitude slider
    }    

    /// <summary>
    /// Method to handle the satisfaction UI 
    /// </summary>
    private void Satisfaction()
    {
        UIManager.satisfactionValue -= UIManager.satisfactionDropRate * Time.deltaTime; //Removes satisfaction over time
        satisfactionSliderRef.value = UIManager.satisfactionValue;  //Sets the new satisfaction value to the UI slider

        UIManager.satisfactionRef.GetComponentInChildren<Image>().color = UIManager.satisfactionGradient.Evaluate(satisfactionSliderRef.normalizedValue);  //Sets the colour of the satisfaction slider based on it's value
        int x; //Integer index to determine which worker face image should be displayed based on the satisfaction level

        //Sets the index for which image should be selected
        if(UIManager.satisfactionValue < 20)
        {
            x = 2;
        }
        else if (UIManager.satisfactionValue < 55)
        {
            x = 1;
        }
        else
        {
            x = 0;
        }
        
        //Enables or disables worker face images based on  the satisfaction
        for ( int i = 0; i < UIManager.workersFacesImageList.Count; i++)
        {
            if (x == i)
            {
                UIManager.workersFacesImageList[i].enabled = true;
            }
            else
            {
                UIManager.workersFacesImageList[i].enabled = false;
            }
        }

        //If satisfaction of 100 is achieved the score scene is loaded
        if (UIManager.satisfactionValue >= 100 || UIManager.satisfactionValue <= 0)
        {
            UIManager.gameManager.levelManager.SceneSelectMethod(3);
        }
    }

    /// <summary>
    /// Method to determine the distance the drone has flown from it's origin and apply a static effect if it's range is too far
    /// </summary>
    private void Range()
    {
        range = Vector3.Distance(UIManager.gameManager.droneController.transform.position, UIManager.gameManager.droneController.droneMovement.anchorPosition);   //Gets the distance the drone has flown from it's origin
        UIManager.rangeRef.GetComponentInChildren<Text>().text = Mathf.RoundToInt(100 - ((range / UIManager.gameManager.droneController.maxRange) * 100)) + "%";   //Sets the range text equal to the percentage of the maximum range the drone has flown

        float staticEffectIntensity = Mathf.Pow(((range - (UIManager.gameManager.droneController.maxRange * UIManager.signalLossPoint)) / (UIManager.gameManager.droneController.maxRange * (1 - (UIManager.signalLossPoint / 1)))), 1.5f);  //Sets an exponentialy increasing intensity for the static effect after the drone has flown past the point where it begins to lose signal

        if (range > UIManager.gameManager.droneController.maxRange * UIManager.signalLossPoint)  //If the drone has flown past the point where it begins to lose signal
        {
            UIManager.staticEffect.GetComponent<Image>().color = new Color(255, 255, 255, staticEffectIntensity); //Applies a static effect over the screen

            if (range > UIManager.gameManager.droneController.maxRange)  //If the drone flies outside it max range and loses signalk
            {
                UIManager.gameManager.droneController.droneMovement.parentRB.transform.position = UIManager.gameManager.droneController.droneMovement.startPosition;  //Resets the drone's position
                UIManager.gameManager.droneController.droneMovement.parentRB.velocity = Vector3.zero;  //Resets the drone's velocity
            }
        }
        else   //Else removes the static effect
        {
            UIManager.staticEffect.GetComponent<Image>().color = new Color(255, 255, 255, 0f);
        }

        float signalRatio = range / UIManager.gameManager.droneController.maxRange;  //Ratio to determine how much signal strength it has based on the distance flown
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
        for (int i = 0; i < UIManager.signalImageList.Count; i++)
        {
            if (x == i)
            {
                UIManager.signalImageList[i].enabled = true;
            }
            else
            {
                UIManager.signalImageList[i].enabled = false;
            }
        }
    }
}

