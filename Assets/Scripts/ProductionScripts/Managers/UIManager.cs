using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    [Tooltip("Reference to satisfaction slider of the drone UI ")]
    public GameObject satisfactionRef;  
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
    [Range(0f, 1f)]
    [Tooltip("Sets the rate of satisfaction loss at rate of 0-1 ticks per second. Total ticks = 100.")]
    public float satisfactionDropRate;
    [Range(0f, 100f)]
    [Tooltip("Sets the initial value of worker satisfaction. Range from 0 - 100.")]
    public float satisfactionValue;
    [Tooltip("Sets the gradient for the satisfaction slider")]
    public Gradient satisfactionGradient;

    //Compass variables
    [Header("Compass variables")]
    [Tooltip("Reference top the hazard marker prefab")]
    public GameObject hazardMarkerPrefab;
    [Tooltip("Sets the minimum and maximum scale of the compass marker")]
    public Vector2 hazardMarkerScale;
    [Tooltip("Sets the range for the minimum and maximum compass marker scale value in metres")]
    public Vector2 hazardMarkerScaleRange;
}
