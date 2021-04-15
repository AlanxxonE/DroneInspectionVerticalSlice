using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameManager gameManager;
    //public Collider tutHazard;
    public GameObject[] rings;

    float tutCounter = 0f;
    int ringCount = -1;
    bool ringsDone = false;
    bool ringsStart = true;

    [HideInInspector]public int scaffoldIndex;
    public bool tutorialEnabled;
    private bool runTutorialMethod = true;
    private bool runDialogue = false;
    public bool endTutorialParagraph = false;

    string[] tutorialParagraphs = {
        "Welcome to the construction site! First thing, try to get the feel of the Drone's controls.\n Use the MOUSE to look around.",
        "Your drone is built with an indicator in the top left for the signal. Be careful not to go out of range \n or you'll lose control. Now try moving, use the W,A,S,D keys to move the drone.",
        "You can swap between the third person and first person cameras by pressing C, try it now!",
        "In first person your altitude meter will give you an idea of how far off the ground you are. \n Use SPACE key to Ascend, SHIFT key to Descend.",
        "Alright, now that you've got a feel for it, fly through the purple RINGS, this will give you a chance \n to get more familiar with the site.",
        "One of the guys working pointed out an issue at one of the scaffolds. \n The point on your compass at the top of the screen will help lead you there.",
        "The closer to a hazard you are the large the marker on the compass shall appear.",
        "Take a closer look at the scaffold. Ensure you are using the first person camera to focus.",
        "Alright, like most cameras the drone needs to be in a good position for proper focus, get\n yourself in a good position, not too close, not too far, and you should be able to see it clearly.",
        "If a hazard is in your view but your range is incorrect, the viewfinder will appear red, once in the\n correct position the viewfinder will turn green.",
        "Is everything okay with this? It doesn’t look very stable, that could make the whole thing collapse, \n get someone over here to fix it. Click the LEFT MOUSE BUTTON when the viewfinder is green.",
        "Make sure you point out everything hazardous by clicking on them, the manager will confirm if you were succesful.",
        "Perfect that's the scaffold finished, you're all warmed up now!",
        "Your remaining time is displayed in the top right of the screen. Fixing hazards will grant \n you more time, failing to do so will result in a time penalty.",
        "If you need a break you can press P to open/close the pause menu.",
        " ",
        "Ok, you're here to find hazards on the construction site to make sure everyone will be safe...\n Get to it!"};

    string[] postTutorialParagraphs = {        
        "Well done, you fixed a hazard!",
        "That equipment looked unsafe, great job spotting it!",
        "Keep up the good work, that's how you find hazards!",
        "That could've ruined our shift!",
        "Thanks for preventing a disaster!",
        "Great job, that's how you do it!",
        "Thank you for finding that unsafe piece of gear!",
        "You saved us a lot of trouble with that one!"
    };
        
    
    private void Awake()
    {
        tutorialEnabled = LevelManager.tutorialEnabled;

        foreach (GameObject ring in rings)
        {
            ring.SetActive(false);
        }

        gameManager.dialogueManager.UpdateParagraphs(tutorialParagraphs);
        
        if(tutorialEnabled)
        {
            gameManager.UIManager.timeRemaining = gameManager.UIManager.totalTime.y;
        }
        else
        {
            gameManager.UIManager.timeRemaining = gameManager.UIManager.totalTime.x;
        }
    }

    private void Update()
    {
        if (runTutorialMethod)
        {
            StartCoroutine(TutorialCheck(tutorialEnabled, Time.deltaTime));            
        }
        
        if(runDialogue && !endTutorialParagraph)
        {           
            switch (gameManager.dialogueManager.GetSentenceNumber())
            {
                //mouse movement
                case 0:
                    gameManager.droneController.droneVelocity = 0;
                    tutCounter += Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime;

                    if (tutCounter > 1) { gameManager.dialogueManager.StopSentence(); }
                    break;

                //WASD movement
                case 1:
                    gameManager.droneController.droneVelocity = 12;                   

                    tutCounter += Mathf.Abs(Input.GetAxis("velX")) * Time.deltaTime;
                    tutCounter += Mathf.Abs(Input.GetAxis("velZ")) * Time.deltaTime;

                    if (tutCounter > 2) { gameManager.dialogueManager.StopSentence(); }
                    break;

                //camera switch
                case 2:
                    if (Input.GetButtonDown("ToggleCam")) { gameManager.dialogueManager.StopSentence(); }
                    break;

                //Vertical movement
                case 3:
                    tutCounter += Mathf.Abs(Input.GetAxis("velY")) * Time.deltaTime;

                    if (tutCounter > 2) { gameManager.dialogueManager.StopSentence(); }
                    break;

                //rings
                case 4:
                    if (ringsStart)
                    {
                        UpdateRingCount();
                        ringsStart = false;
                    }
                    if (ringsDone) { gameManager.dialogueManager.StopSentence(); }
                    break;

                //Compass
                case 5:                    
                    if (Vector3.Distance(gameManager.hazardManager.hazardTransforms[scaffoldIndex].position, gameManager.droneController.gameObject.transform.position) < 20) { gameManager.dialogueManager.StopSentence(); }
                    break;
                
                //examine hazard
                case 9:
                    if (gameManager.hazardManager.hazardTransforms[scaffoldIndex].GetComponent<MonoBehaviour>().enabled == false)
                    {
                        gameManager.hazardManager.hazardTransforms[scaffoldIndex].GetComponent<Collider>().enabled = true;                       
                    }

                    if (gameManager.dialogueManager.IsSentenceFinished())
                    {
                        tutCounter += Time.deltaTime;
                        if (tutCounter > 2)
                        {
                            gameManager.dialogueManager.StopSentence();
                        }
                    }                    
                    break;
                
                //Interact with hazard
                case 10:
                    if (gameManager.hazardManager.hazardTransforms[scaffoldIndex].GetComponent<MonoBehaviour>().enabled == true)
                    {
                        gameManager.dialogueManager.StopSentence();
                    }
                    break;

                //fix hazard
                case 11:
                    if (Score.isScaffoldFixed == true)
                    {
                        gameManager.dialogueManager.StopSentence();
                    }
                    break;

                case 14:
                    if(Input.GetButtonDown("Pause"))
                    {
                        gameManager.dialogueManager.StopSentence();
                    }
                    break;

                case 15:
                    foreach (Transform t in gameManager.hazardManager.hazardTransforms)
                    {
                        t.GetComponent<Collider>().enabled = true;
                    }
                    foreach (GameObject markers in gameManager.UIManager.compass.hazardMarkers)
                    {
                        markers.GetComponent<RawImage>().enabled = true;
                    }

                    if(Score.isScaffoldFixed)
                    {
                        gameManager.hazardManager.hazardTransforms[scaffoldIndex].GetComponent<Collider>().enabled = false;
                        gameManager.UIManager.compass.hazardMarkers[scaffoldIndex].GetComponent<RawImage>().enabled = false;
                    }

                    gameManager.dialogueManager.StopSentence();
                    break;

                case 16:
                    tutCounter += Time.deltaTime;

                    if (tutCounter > 5)
                    {
                        gameManager.dialogueManager.StopSentence();

                        endTutorialParagraph = true;

                        gameManager.dialogueManager.UpdateParagraphs(postTutorialParagraphs);                    
                    }
                    break;

                default:

                    if (!endTutorialParagraph)
                    {
                        if (gameManager.dialogueManager.IsSentenceFinished())
                        {
                            tutCounter += Time.deltaTime;
                        }
                        if (tutCounter > 2)
                        {
                            gameManager.dialogueManager.StopSentence();
                        }
                    }
                    break;
            }
        }
        

        if(!gameManager.dialogueManager.IsSentenceFinished())
        {
            tutCounter = 0;
        }
    }

    IEnumerator TutorialCheck(bool isTutorialEnabled, float time)
    {
        runTutorialMethod = !runTutorialMethod;

        yield return new WaitForSeconds(time);

        foreach (GameObject markers in gameManager.UIManager.compass.hazardMarkers)
        {
            markers.GetComponent<RawImage>().enabled = !isTutorialEnabled;
        }        

        gameManager.UIManager.compass.hazardMarkers[scaffoldIndex].GetComponent<RawImage>().enabled = true;

        if (!isTutorialEnabled)
        {
            gameManager.dialogueManager.sentenceNumber = 15;
        }

        runDialogue = true;
        gameManager.dialogueManager.RunDialogue();
    }  

    public void UpdateRingCount()
    {
        ringCount++;
        Debug.Log(ringCount);
        if (ringCount > 0)
        {
            rings[ringCount - 1].SetActive(false);
        }
        if (ringCount < rings.Length)
        {
            rings[ringCount].SetActive(true);
            //if (ringCount < rings.Length - 1)
            //{
            //    rings[ringCount + 1].SetActive(true);
            //}
        }
        else
        {
            ringsDone = true;
        }
    }
}
