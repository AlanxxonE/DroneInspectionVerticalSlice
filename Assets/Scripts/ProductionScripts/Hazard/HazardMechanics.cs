using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardMechanics : MonoBehaviour
{
    /// <summary>
    /// Class that handles all the hazard mechanics. Each hazard has an instance of this class that is called from the hazard manager class when a hazard is interacted with.
    /// </summary>
    /// 

    public HazardManager hazardManagerRef; //Reference to hazard manager script

    //GenericHazardVariables
    public int optimalDistanceMax = 0;    //Minimum optimal distance to hazard
    public int optimalDistanceMin = 0;    //Maximum optimal distance to hazard
    public int MaximumDistance = 0;       //Maximum distance a raycast can detect a hazard from

    private PointerEventData pointerData = new PointerEventData(EventSystem.current);   //Used to find what the mouse is hovering over in minigames
    private List<RaycastResult> pointerHitList = new List<RaycastResult>();     //Holds a list of objects the mouse can interact with in minigames

    Vector3 currentMousePosition;               //Reference for current mouse position
    Vector3 lastMousePosition;

    public GameObject hazardPopUpRef;           //Reference to the current hazard being interacted with
    public string hazardTag;                    //Tag of the hazard being interacted with

    private Slider hazardSlider;                //Slider attached to each hazard, used to keep track of hazard completion progress
    
    //ScaffoldMethod
    float rotationAngle = 0;         //Holds the absolute value for the rotation of the bolt 
    float lastRotationAngle = 0;             
    GameObject UnscrewBolt;

    //CraneMethod
    GameObject FixedWire;      //Reference to fixed wire game object in the crane hazard minigame
    GameObject WireBox;        //Reference to fixed wire box game object in the crane hazard minigame
    GameObject TornWire;       //Reference to torn wire game object in the crane hazard minigame
    GameObject TornBox;        //Reference to torn wire box game object in the crane hazard minigame
    bool checkSwapWire = false;       //Boolean to check if the cable has been swapped 

    //public bool checkEffect = false;

    public List<Image> dangerImageList;  //List to determine danger level of a hazard
    private int dangerToDisplay;         //Reference to determine which danger image to display

    public enum LevelsOfDangers
    {
        Green,
        Amber,
        Red
    }

    public LevelsOfDangers hazardDangerLevel;

    // Start is called before the first frame update
    void Start()
    {
        hazardTag = hazardPopUpRef.tag;  //Gets the tag of the current hazard being interacted with 

        //Reference for minigames
        UnscrewBolt = GameObject.FindGameObjectWithTag("UnscrewBolt");
        FixedWire = GameObject.FindGameObjectWithTag("FixedWire");
        WireBox = GameObject.FindGameObjectWithTag("WireBox");
        TornWire = GameObject.FindGameObjectWithTag("TornWire");
        TornBox = GameObject.FindGameObjectWithTag("TornBox");

        switch (hazardTag)          //Switch case to set optimal distances for hazard interaction dependent on their tag 
        {
            case "ScaffoldHazard":                              
                optimalDistanceMax = 12;
                optimalDistanceMin = 8;
                MaximumDistance = 30;
                break;

            case "CraneHazard":                                
                optimalDistanceMax = 20;
                optimalDistanceMin = 10;
                MaximumDistance = 40;
                break;
            default:
                break;
        }

        hazardTag = null;   //Resets the hazard tag, prevents errors

        switch (hazardDangerLevel)  //Sets the index for the list of images for the hazard danger level
        {
            case HazardMechanics.LevelsOfDangers.Green:
                dangerToDisplay = 0;
                break;

            case HazardMechanics.LevelsOfDangers.Amber:
                dangerToDisplay = 1;
                break;

            case HazardMechanics.LevelsOfDangers.Red:
                dangerToDisplay = 2;
                break;

            default:
                break;
        }

        for (int i = 0; i < dangerImageList.Count; i++)   //Sets the current image that should be displayed for the subsequent hazard this script is attached to
        {
            if (dangerToDisplay == i)
            {
                dangerImageList[i].enabled = true;
            }
            else
            {
                dangerImageList[i].enabled = false;
            }
        }

        hazardPopUpRef.SetActive(false);  //Sets the hazard inactive at the start until interacted with
    }

    // Update is called once per frame
    void Update()
    {
        currentMousePosition = Input.mousePosition;   //Sets current mouse position

        if (hazardManagerRef.hazardRef != null /*&& hazardManagerRef.hazardRef.GetComponent<Animator>().GetBool("ActiveHazard") == true*/)   //True if hazard is currently being interacted with
        {           
            switch (hazardTag)            //Dependent on the hazard's tag it runs it's corresponding method, aswell as the hazard progress method
            {
                case "ScaffoldHazard":
                    HazardProgress(40, -20, 20);
                    //ScaffoldHazard();
                    break;

                case "CraneHazard":
                    HazardProgress(40, -20, 20);
                    CraneHazard();
                    break;

                default:
                    break;
            }
        }        
    }

    /// <summary>
    /// Method to handle to progression of a hazard and manage it's win/lose states
    /// </summary>
    /// <param name="satisfaction"></param>  satifaction score gained from winning the minigame
    /// <param name="dissatisfaction"></param> satisfaction score lost from losing the minigame
    /// <param name="score"></param>  score added to the score at the end of the game
    public void HazardProgress(int satisfaction, int dissatisfaction, int score)
    {
        hazardSlider = hazardManagerRef.hazardRef.GetComponentInChildren<Slider>().GetComponent<Slider>();  //Gets the slider of the hazard, used to keep track of progress
        
        if (hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
        {
            this.GetComponent<HazardEffect>().endEffect = true;
            hazardTag = null;
            hazardManagerRef.FinishHazard(satisfaction, score, true);          
        }
        else if (hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
        {
            this.GetComponent<HazardEffect>().endEffect = true;
            hazardTag = null;
            hazardManagerRef.FinishHazard(dissatisfaction, 0, false);
        }

        hazardSlider.value -= Time.deltaTime * 2f;  //Sets the rate at which the hazard timer counts down ////Don't move this
    }

    /// <summary>
    /// Method to handle the scaffold hazard minigame
    /// </summary>
    public void ScaffoldHazard()
    {       
        if (Input.GetMouseButton(0))    //Runs if mouse 1 is pressed
        {
            if (hazardPopUpRef != null)   
            {
                Vector2 MousePoint = new Vector2(Mathf.Abs(currentMousePosition.x - UnscrewBolt.transform.position.x), Mathf.Abs(currentMousePosition.y - UnscrewBolt.transform.position.y));  //Gets an absolute value for the x and y coordinates relative to where the cursor was when mouse 1 was initially pressed 

                lastRotationAngle = rotationAngle;  //Reference to previous cursor position

                int MaxDistance = 200;  //Sets the max distance from the bolt the cursor still has a turning effect

                if (MousePoint.x < MaxDistance && MousePoint.y < MaxDistance) //Runs if within the distance limit
                {
                    rotationAngle = Mathf.Atan2(currentMousePosition.y - UnscrewBolt.transform.position.y, currentMousePosition.x - UnscrewBolt.transform.position.x) * Mathf.Rad2Deg; //Gets the angle the bolt has been turned based on the cursor position

                    UnscrewBolt.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0, 0, rotationAngle);  //Sets the bolts rotation equal to the rotation angle

                    if (rotationAngle < 0 && rotationAngle > -180)
                    {
                        rotationAngle = 360 - Mathf.Abs(rotationAngle);
                    }

                    if (lastRotationAngle > rotationAngle)
                    {
                        hazardSlider.value += 0.2f;   //Adds value to the slider if a clockwise rotation is input
                    }
                    else if (lastRotationAngle < rotationAngle)
                    {
                        hazardSlider.value -= 0.2f;   //Removes value from the slider if an anti-clockwise rotation is input
                    }
                }
            }
        }

    }

    /// <summary>
    /// Method to handle the crane hazard minigame
    /// </summary>
    public void CraneHazard()
    {       
        pointerData.position = currentMousePosition;    //Sets the pointer data variables poisition equal to the cursor position
        EventSystem.current.RaycastAll(pointerData, pointerHitList); 

        if (Input.GetMouseButton(0)) //Runs while mouse 1 is held down
        {
            if (hazardPopUpRef != null)
            {
                for (int i = 0; i < pointerHitList.Count; i++)
                { 
                    //Moves the fixed wire's position if the torn wire hasn't been moved and the wire hasn't been swapped
                    if (pointerHitList[i].gameObject.CompareTag("FixedWire") && (TornWire.transform.position == TornBox.transform.position) && checkSwapWire == false)
                    {
                        FixedWire.transform.position = currentMousePosition;
                    }

                    //Moves the tornwire's position if the fixed wire hasn't been moved 
                    if(pointerHitList[i].gameObject.CompareTag("TornWire") && (FixedWire.transform.position == WireBox.transform.position))
                    {
                        TornWire.transform.position = currentMousePosition;
                    }
                }
            }
        }
        else //Runs when mouse 1 has been released
        { 
            for (int i = 0; i < pointerHitList.Count; i++)
            {
                //If the torn wire is being moved, and is over the recycle bin as mouse 1 is released it is set inactive
                if (pointerHitList[i].gameObject.CompareTag("BinBox") && (TornWire.transform.position != TornBox.transform.position))
                {
                    TornWire.SetActive(false);
                }

                //If the fixed wire is being moved, and is over where the original position of the torn wire and the torn wire has been recycled when mouse 1 has been released
                if(pointerHitList[i].gameObject.CompareTag("TornBox") && (FixedWire.transform.position != WireBox.transform.position) && TornWire.activeSelf == false)
                {
                    checkSwapWire = true;  //Sets the boolean for checking if  the wire has been swapped equal to true
                    FixedWire.transform.position = TornBox.transform.position + new Vector3(0,50);  //Sets the fixed wire's position equal to the torn wire's original position
                    hazardSlider.value += 100f;  //Adds 100 to the slider to ensure completion in the hazard progress method
                }
            }

            //If the game is running and the fixed wire or town wire are not in the original positions as mouse 1 is released
            if (hazardPopUpRef != null && (FixedWire.transform.position != WireBox.transform.position || TornWire.transform.position != TornBox.transform.position))
            {
                //If the wire has not been swapepd
                if (checkSwapWire == false)
                {
                    FixedWire.transform.position = WireBox.transform.position;  //The fixed wire returns to it's original position
                }
                TornWire.transform.position = TornBox.transform.position;  //The torn wire returns to it's original position
            }
        }
    }
}
