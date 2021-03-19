using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameManager gameManager;
    public Collider tutHazard;
    public GameObject[] rings;

    float tutCounter = 0f;
    int sentenceNumber = 0;
    int ringCount = -1;
    bool ringsDone = false;
    bool ringsStart = true;

     public string[] tutorialParagraphs = {
        "Welcome to the construstion site! First thing, try to get the feel of the Drone's controls. Use the \n MOUSE to look around",
        "Your drone is built with an indicator for the signal. Be careful not to go out of range or you'll lose\n control. Now try moving, use the W,A,S,D keys to move the drone",
        "Your altitude meter here gives you an idea of how far off the ground you are. Use SPACE key to\n Ascend, SHIFT key to Descend",
        "Alright, now that you've got a feel for it we'll need to configure your compass. Fly through the\n rings this will also give you a chance to get more familiar with the site",
        "One of the guys working pointed out an issue at one of the scaffolds. The point on your compass will\n lead you there.",
        "Hazards present different levels of danger within the site and therefore should potentially be\n treated with different priority",
        "These danger levels are displayed on the drone’s compass as green amber or red. Take a closer look\n at the scaffold. Use C to switch to first person",
        "Alright, like most cameras that drone needs to been in a good position for proper focus, get yourself\n in a good position, not too close, not too far, and you should be able to see it clearly.",
        "If a hazard is in your view but your range is incorrect, the viewfinder will appear red, once in the\n correct position the viewfinder will turn green.",
        "Anyway, see that bolt there? Doesn’t look quite screwed in all the way, that could leave the whole\n thing unstable, get someone over here to fix it. Click the LMB when the viewfinder is green",
        "Make sure you’re screwing that in the right way, get it nice and tight before this thing falls apart"};
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        foreach (GameObject ring in rings)
        {
            ring.SetActive(false);
        }

        //gameManager.dialogueManager.paragraphs = tutorialParagraphs;
        gameManager.dialogueManager.UpdateParagraphs(tutorialParagraphs);
        gameManager.dialogueManager.RunDialogue();
    }

    // Update is called once per frame
    void Update()
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
                gameManager.UIManager.rangeRef.SetActive(true);

                tutCounter += Mathf.Abs(Input.GetAxis("velX")) * Time.deltaTime;
                tutCounter += Mathf.Abs(Input.GetAxis("velZ")) * Time.deltaTime;

                if (tutCounter > 2) { gameManager.dialogueManager.StopSentence(); }
                break;
            //Vertical movement
            case 2:
                gameManager.UIManager.altitudeRef.SetActive(true);

                tutCounter += Mathf.Abs(Input.GetAxis("velY")) * Time.deltaTime;

                if (tutCounter > 2) { gameManager.dialogueManager.StopSentence(); }
                break;
            //rings
            case 3:
                if(ringsStart)
                {
                    UpdateRingCount();
                    ringsStart = false;
                }
                if (ringsDone) { gameManager.dialogueManager.StopSentence(); }
                break;
            //Compass
            case 4:
                gameManager.UIManager.compass.gameObject.SetActive(true);
                if (Vector3.Distance(tutHazard.gameObject.transform.position, gameManager.droneController.gameObject.transform.position) < 20) { gameManager.dialogueManager.StopSentence(); }
                break;
            //camera switch
            case 6:
                if (Input.GetButtonDown("ToggleCam")) { gameManager.dialogueManager.StopSentence(); }
                break;
            //examine hazard
            case 9:
                tutHazard.enabled = true;

                if (tutHazard.GetComponent<MonoBehaviour>().enabled == true) { gameManager.dialogueManager.StopSentence(); }
                break;
            //fix hazard
            case 10:
                if (gameManager.hazardManager.hazardSliderRef.activeSelf == false) { gameManager.dialogueManager.StopSentence(); }
                break;
            default:
                if (gameManager.dialogueManager.IsSentenceFinished())
                {
                    tutCounter += Time.deltaTime;
                }
                if (tutCounter > 2) { gameManager.dialogueManager.StopSentence(); }
                break;
        }

        if(!gameManager.dialogueManager.IsSentenceFinished())
        {
            tutCounter = 0;
        }
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
            if (ringCount < rings.Length - 1)
            {
                rings[ringCount + 1].SetActive(true);
            }
        }
        else
        {
            ringsDone = true;
        }
    }
}
