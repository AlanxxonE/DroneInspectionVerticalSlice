using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardManager : MonoBehaviour
{
    /// <summary>
    /// Class used to manage various aspects of hazards such as optimal distance, initialisation and the finishing of hazards as well as holding hazard variables.
    /// </summary>

    //Class References 
    [Header("Class References")]
    public GameManager gameManager; //Reference to game manager
    
    //General References
    [Header("General References")]
    [Tooltip("Reference to the hazard slider parent object")]
    public GameObject hazardSliderRef; //Reference to the hazard slider game object
    [HideInInspector] public Slider hazardSlider; //Reference to the slider itself of the hazard slider game object
    public List<Transform> hazardTransforms; //List of transforms of each hazard in the game    
    [HideInInspector] public MonoBehaviour currentHazardScript = null; //Reference to current hazard being interacted with
    [HideInInspector] public string hazardName; //Name of hazard being interacted with

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
    [Tooltip("Optimal range to interact with acrow hazard")]
    public Vector2 acrowOptimalRange;
    [Tooltip("Optimal range to interact with propane tank hazard")]
    public Vector2 propaneTankOptimalRange;

    private void Awake()
    {
        hazardSlider = hazardSliderRef.GetComponent<Slider>();
        hazardSliderRef.SetActive(false);
    }
    
    //Returns a vector 2 representing the minimum and maximum range that a hazard can be interacted with from. Take in a string to determine the hazard to return.
    public Vector2 GetOptimalRange(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                return scaffoldOptimalRange;
            case "Crane":
                return craneOptimalRange;
            case "Acrow":
                return acrowOptimalRange;
            case "PropaneTank":
                return propaneTankOptimalRange;
            default:
                return new Vector2(0, 0);
        }
    }

    /// <summary>
    /// Method that initialises a hazard after it's interacted with. Takes in a monobehaviour as input when called.
    /// </summary>
    /// <param name="currenHazardScript"></param>
    public void InitialiseHazard(MonoBehaviour currenHazardScript)
    {
        Cursor.lockState = CursorLockMode.None;  //Unlocks cursor
        gameManager.droneController.droneCamera.interpolationTime = 0;  //Resets interpolation time timer
        gameManager.droneController.droneCamera.SwitchPerspective(false); //Switches to TPP camera

        currentHazardScript = currenHazardScript;  //Sets the hazard script ref
        currentHazardScript.enabled = true;  //Enables the monobehaviour
        hazardName = currenHazardScript.GetType().Name; //Gets/Sets the name of the class

        hazardSliderRef.SetActive(true);  //Sets the hazard progress slider active
    }

    /// <summary>
    /// Method use to handle the win/lose condition of a hazard minigame, called on by the hazard progress method in the hazard mechanics class
    /// </summary>
    /// <param name="timeGainedOrLost"></param> time gained or lost by winning/losing a minigame
    /// <param name="score"></param>  score achieved at the end of the game
    /// <param name="isFixed"></param>  boolean to determine if a hazard was fixed successfully or not
    public void FinishHazard(int timeGainedOrLost,int score, bool isFixed, Transform cameraFocalPoint, Transform hazardTransform, int index)
    {
        Cursor.lockState = CursorLockMode.Locked;  //Locks the cursor
        if (gameManager.droneController.droneCamera.FocusOnHazard(cameraFocalPoint, hazardTransform, true))
        {
            gameManager.droneController.droneRayCast.stopMovement = false;   //Allows the drone to move again
            gameManager.UIManager.timeRemaining += timeGainedOrLost; //Adds/subtracts time from the time remaining meter depending on a win/lose            

            hazardSlider.value = hazardSliderInitialValue;    //Resets hazard slider
            hazardSliderRef.SetActive(false);  //Sets hazard progress slider inactive

            if (isFixed)  //If the hazard is fixed, i.e. minigame was completed
            {
                Score.SetFixedBooleans(hazardName, isFixed, false);  //Sets the boolean for the hazard being fixed to true
                currentHazardScript.tag = "Fixed";  //Changes tag of the hazard
                gameManager.UIManager.compass.hazardMarkers[index].GetComponent<RawImage>().enabled = false;  //Disbales the compass marker for this hazard
                Destroy(cameraFocalPoint.gameObject);   //Destroy the temporary gameobject created as a transform for the camera to focuse on during the hazard interaction

                foreach (Transform c in hazardTransform)
                {
                    if (c.CompareTag("Repaired"))
                    {
                        c.gameObject.SetActive(true);
                    }
                    else
                    {
                        c.gameObject.SetActive(false);
                    }
                }

                StartCoroutine(gameManager.dialogueManager.EnableDialogueBox(0, true));
                gameManager.dialogueManager.DisplayParagraph(Random.Range(0, gameManager.dialogueManager.GetParagraphsLength()), 1);
                StartCoroutine(gameManager.dialogueManager.EnableDialogueBox(5, false));

                if (Score.AreAllHazardsFixed())
                {
                    gameManager.levelManager.SceneSelectMethod(3);
                }
            }

            currentHazardScript.enabled = false;  //Disables this hazard class
            currentHazardScript = null;  //Resets the referenece
            hazardName = null; //Resets the hazard name
          
            gameManager.droneController.droneCamera.SwitchPerspective(true);  //Switches to FPP camera
        }
    }   

    /// <summary>
    /// Method used to return the list of hazard transform
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetHazardTransforms()
    {
        return hazardTransforms; 
    }
}