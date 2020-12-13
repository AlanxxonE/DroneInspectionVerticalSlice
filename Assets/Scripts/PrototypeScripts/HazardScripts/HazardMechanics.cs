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
    public int optimalDistanceMax = 0;            //Minimum optimal distance to hazard
    public int optimalDistanceMin = 0;            //Maximum optimal distance to hazard
    public int MaximumDistance = 0;               //Maximum distance a raycast can detect a hazard from

    private PointerEventData pointerData = new PointerEventData(EventSystem.current);        ////////////Alessio///////I don't know how this works !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private List<RaycastResult> pointerHitList = new List<RaycastResult>();

    Vector3 currentMousePosition;               //Reference for current mouse position
    Vector3 lastMousePosition;

    public GameObject hazardPopUpRef;           //Reference to the current hazard being interacted with
    public string hazardTag;                    //Tag of the hazard being interacted with

    private Slider hazardSlider;                //Slider attached to each hazard, used to keep track of hazard completion progress
    
    //ScaffoldMethod
    float rotationAngle = 0;                    //////Not sure how these work exactly !!!!!!!!!!!!!!!!!!!!!!!!!!
    float lastRotationAngle = 0;             
    GameObject UnscrewBolt;

    //CraneMethod
    GameObject FixedWire;      //Reference to fixed wire game object in the crane hazard minigame
    GameObject WireBox;        //Reference to fixed wire box game object in the crane hazard minigame
    GameObject TornWire;       //Reference to torn wire game object in the crane hazard minigame
    GameObject TornBox;        //Reference to torn wire box game object in the crane hazard minigame
    bool checkSwapWire = false;       // 

    public bool checkEffect = false;

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

        hazardTag = null;

        hazardPopUpRef.SetActive(false);  //Sets the hazard inactive at the start until interacted with
    }

    // Update is called once per frame
    void Update()
    {
        currentMousePosition = Input.mousePosition;   //Sets current mouse position

        if (hazardManagerRef.hazardRef != null && hazardManagerRef.hazardRef.GetComponent<Animator>().GetBool("ActiveHazard") == true)   //True if hazard is currently being interacted with
        {           
            switch (hazardTag)            //Dependent on the hazard's tag it runs it's corresponding method, aswell as the hazard progress method
            {
                case "ScaffoldHazard":
                    HazardProgress(40, -20, 20);
                    ScaffoldHazard();
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

    //////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Don't know how this works, needs commented!!!!!!!!!!!!!!!!!!!!!!!!
    /// <summary>
    /// Method to handle the scaffold hazard minigame
    /// </summary>
    public void ScaffoldHazard()
    {       
        if (Input.GetMouseButton(0))
        {
            if (hazardPopUpRef != null)
            {
                Vector2 MousePoint = new Vector2(Mathf.Abs(currentMousePosition.x - UnscrewBolt.transform.position.x), Mathf.Abs(currentMousePosition.y - UnscrewBolt.transform.position.y));

                lastRotationAngle = rotationAngle;

                int MaxDistance = 200;

                if (MousePoint.x < MaxDistance && MousePoint.y < MaxDistance)
                {
                    rotationAngle = Mathf.Atan2(currentMousePosition.y - UnscrewBolt.transform.position.y, currentMousePosition.x - UnscrewBolt.transform.position.x) * Mathf.Rad2Deg;

                    UnscrewBolt.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0, 0, rotationAngle);

                    if (rotationAngle < 0 && rotationAngle > -180)
                    {
                        rotationAngle = 360 - Mathf.Abs(rotationAngle);
                    }

                    if (lastRotationAngle > rotationAngle)
                    {
                        hazardSlider.value += 0.2f;
                    }
                    else if (lastRotationAngle < rotationAngle)
                    {
                        hazardSlider.value -= 0.2f;
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
        pointerData.position = currentMousePosition;
        EventSystem.current.RaycastAll(pointerData, pointerHitList);

        if (Input.GetMouseButton(0))
        {
            if (hazardPopUpRef != null)
            {
                for (int i = 0; i < pointerHitList.Count; i++)
                {
                    if (pointerHitList[i].gameObject.CompareTag("FixedWire") && (TornWire.transform.position == TornBox.transform.position) && checkSwapWire == false)
                    {
                        FixedWire.transform.position = currentMousePosition;
                    }

                    if(pointerHitList[i].gameObject.CompareTag("TornWire") && (FixedWire.transform.position == WireBox.transform.position))
                    {
                        TornWire.transform.position = currentMousePosition;
                    }
                }
            }
        }
        else
        { 
            for (int i = 0; i < pointerHitList.Count; i++)
            {
                if (pointerHitList[i].gameObject.CompareTag("BinBox") && (TornWire.transform.position != TornBox.transform.position))
                {
                    TornWire.SetActive(false);
                }

                if(pointerHitList[i].gameObject.CompareTag("TornBox") && (FixedWire.transform.position != WireBox.transform.position) && TornWire.activeSelf == false)
                {
                    checkSwapWire = true;
                    FixedWire.transform.position = TornBox.transform.position + new Vector3(0,50);
                    hazardSlider.value += 100f;
                }
            }

            if (hazardPopUpRef != null && (FixedWire.transform.position != WireBox.transform.position || TornWire.transform.position != TornBox.transform.position))
            {
                if (checkSwapWire == false)
                {
                    FixedWire.transform.position = WireBox.transform.position;
                }
                TornWire.transform.position = TornBox.transform.position;
            }
        }
    }
}
