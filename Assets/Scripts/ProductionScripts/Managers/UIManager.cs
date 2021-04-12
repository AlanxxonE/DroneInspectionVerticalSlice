using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// UI Manager class. Holds variables and references to/for UI classes
    /// </summary>
    
    //Class References
    [Header("Class References")]
    [Tooltip("Reference to game manager script")]
    public GameManager gameManager;
    [Tooltip("Reference to Drone UI script")]
    public DroneUI droneUI;
    [Tooltip("Reference to compass script")]
    public Compass compass;

    //Drone UI References
    [Header("Drone UI References")]
    [Tooltip("Reference to altitude slider of the drone UI")]
    public GameObject altitudeRef;        
    [Tooltip("Reference to range metre of the drone UI  ")]
    public GameObject rangeRef;      
    [Tooltip("Reference to timer of the drone UI ")]
    public GameObject timerRef;  
    [Tooltip("Reference to circle of the artificial horizon of the drone UI  ")]
    public GameObject artificialHorizonCircle;  
    [Tooltip("Reference to horizontal line of the artificial horizon of the drone UI  ")]
    public GameObject artificialHorizonLine;    
    [Tooltip("Reference to the signal strength overlay image of the drone UI  ")]
    public Image staticEffect;     
    [Tooltip("List of images of the worker's faces")]
    public List<Image> workersFacesImageList;  
    [Tooltip("List of images used to show signal strength")]
    public List<Image> signalImageList;

    //Drone UI Variables
    [Header("Drone UI Variables")]
    [Range(0f, 1f)]
    [Tooltip("Sets percentage of the maximum range of the drone at which the drone signal begins to fade and the static effect begins to increase")]
    public float signalLossPoint;
    [Range(0f, 2f)]
    [Tooltip("Sets the rate at which the timer counts down in seconds per second")]
    public float timeRemainingDropRate;
    [Tooltip("Sets the initial value of time remaining in seconds. First value is if tutorial is disabled, second value is for tutorial enabled")]
    public Vector2 totalTime;
    [HideInInspector] public float timeRemaining;


    //Compass variables
    [Header("Compass variables")]
    [Tooltip("Reference to the hazard marker prefab")]
    public GameObject hazardMarkerPrefab;
    [Tooltip("Reference to the hazard marker up arrow")]
    public GameObject hazardMarkerUpArrow;
    [Tooltip("Reference to the hazard marker down arrow")]
    public GameObject hazardMarkerDownArrow;
    [Tooltip("Minimum distance the drone must be from a hazard before the hazard marker will show the up or down arrows")]
    public int hazardMarkerArrowMaxRange;
    [Tooltip("Sets the minimum and maximum scale of the compass marker")]
    public Vector2 hazardMarkerScale;
    [Tooltip("Sets the range for the minimum and maximum compass marker scale value in metres")]
    public Vector2 hazardMarkerScaleRange;
    [Tooltip("Sets the height range for which the arrows display or not")]
    public Vector2 hazardArrowHeightRange;
    [Tooltip("Sets the amount the hazard arrow is offset from the marker on the compass")]
    public float hazardArrowHorizontalOffset;
}
