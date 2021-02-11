using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardManager : MonoBehaviour
{
    /// <summary>
    /// Class used to manage the initial interaction with hazards and the win/lose conditions and score associated with each subsequent hazard
    /// </summary>

    //Class References 
    [Header("Class References")]
    public GameManager gameManager;
    //public HazardMechanics hazardMechanics;

    //General References
    [Header("General References")]
    [Tooltip("Reference to the hazard slider parent object")]
    public GameObject hazardSliderRef;
    [HideInInspector] public Slider hazardSlider;
    [HideInInspector] public MonoBehaviour currentHazardScript = null;

    //Hazard Mechanics Variables
    [Header("Hazard Mechanics Variables")]
    [Tooltip("Integer to set how fast the hazard loses progress, i.e. how many ticks per second")]
    public int hazardProgressDropRate;
    [Range(0,100)]
    [Tooltip("Initial value for the hazard slider")]
    public int hazardSliderInitialValue;

    //Hazard Optimal Ranges
    [Header("Hazard Optimal Ranges")]
    [Tooltip("Maximum distance a hazard will be detected from")]
    public int maxDetectionDistance;
    [Tooltip("Optimal range to interact with scaffold hazard")]
    public Vector2 scaffoldOptimalRange;
    [Tooltip("Optimal range to interact with crane hazard")]
    public Vector2 craneOptimalRange;

    private void Awake()
    {
        hazardSlider = hazardSliderRef.GetComponent<Slider>();
        hazardSliderRef.SetActive(false);
    }
    
    public Vector2 GetOptimalRange(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                return scaffoldOptimalRange;
            case "Crane":
                return craneOptimalRange;
            default:
                return new Vector2(0, 0);
        }
    }
    public void InitialiseHazard(GameObject hazardRef)
    {
        currentHazardScript = hazardRef.GetComponent<MonoBehaviour>();
        currentHazardScript.enabled = true;
        hazardSliderRef.SetActive(true);
        gameManager.droneController.droneUI.artificialHorizonCircle.SetActive(false);
    }

    /// <summary>
    /// Method use to handle the win/lose condition of a hazard minigame, called on by the hazard progress method in the hazard mechanics class
    /// </summary>
    /// <param name="satisfaction"></param> satisfaction score gained or lost by winning/losing a minigame
    /// <param name="score"></param>  score achieved at the end of the game
    /// <param name="isFixed"></param>  boolean to determine if a hazard was fixed successfully or not
    public void FinishHazard(int satisfaction,int score, bool isFixed, MonoBehaviour currentHazardScript, string hazardName)
    {      
        gameManager.droneController.droneRayCast.stopMovement = false;   //Allows the drone to move again
        gameManager.droneController.satisfactionValue += satisfaction; //Adds/subtracts score from the satisfaction meter depending on a win/lose
        
        hazardSlider.value = hazardSliderInitialValue;
        hazardSliderRef.SetActive(false);

        gameManager.droneController.droneUI.artificialHorizonCircle.SetActive(true);

        if (isFixed)
        {
            Score.SetFixedBooleans(hazardName);
            currentHazardScript.tag = "Fixed";
        }
            
        currentHazardScript.enabled = false;
        currentHazardScript = null;
    }   
}