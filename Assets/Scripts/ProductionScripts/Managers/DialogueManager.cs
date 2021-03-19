using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueSystem;
    public GameManager gameManager;
    public Collider tutHazard;
    public Collider droneCollider;
    Text dialogue;
    int currentAmount = 0;
    float tutCounter = 0f;
    public GameObject[] rings;
    int ringCount = 0;
    bool ringsDone = false;

    bool isRunning = false;
    bool stop = false;

    bool moveOnCheck = false;

    int sentenceNumber = 0;

    string[] paragraphs = {
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

    void Awake()
    {
        foreach (GameObject ring in rings)
        {
            ring.SetActive(false);
        }
        dialogue = dialogueSystem.GetComponentInChildren<Text>();
        //dialogueSystem.SetActive(false);
        StartCoroutine(StartIduction());
    }

    void Update()
    {
        switch (sentenceNumber)
        {
            //mouse movement
            case 0:
                gameManager.droneController.droneVelocity = 0;
                tutCounter += Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime;

                if (tutCounter > 1) { StopSentence(); }
                break;
            //WASD movement
            case 1:
                gameManager.droneController.droneVelocity = 12;
                gameManager.UIManager.rangeRef.SetActive(true);

                tutCounter += Mathf.Abs(Input.GetAxis("velX")) * Time.deltaTime;
                tutCounter += Mathf.Abs(Input.GetAxis("velZ")) * Time.deltaTime;

                if (tutCounter > 2) { StopSentence(); }
                break;
            //Vertical movement
            case 2:
                gameManager.UIManager.altitudeRef.SetActive(true);

                tutCounter += Mathf.Abs(Input.GetAxis("velY")) * Time.deltaTime;

                if (tutCounter > 2) { StopSentence(); }
                break;
            //rings
            case 3:
                

                if (ringsDone) { StopSentence(); }
                break;
            //Compass
            case 4:
                gameManager.UIManager.compass.gameObject.SetActive(true);
                if (Vector3.Distance(tutHazard.gameObject.transform.position, gameManager.droneController.gameObject.transform.position) < 20) { StopSentence(); }
                break;
            //camera switch
            case 6:
                if (Input.GetButtonDown("ToggleCam")) { StopSentence(); }
                break;
            //examine hazard
            case 9:
                tutHazard.enabled = true;

                if(tutHazard.GetComponent<MonoBehaviour>().enabled == true) { StopSentence(); }
                break;
            //fix hazard
            case 10:
                if(gameManager.hazardManager.hazardSliderRef.activeSelf == false) { StopSentence(); }
                break;
            default:
                if (dialogue.text.Length > paragraphs[sentenceNumber].Length)
                {
                    tutCounter += Time.deltaTime;
                }
                if (tutCounter > 2) { StopSentence(); }
                break;
        }
    }

    public void DisplayParagraph(int paraNum, int amountOfLetters, bool selfRun)
    {
        isRunning = true;

        if (!stop)
        {
            currentAmount = amountOfLetters;
            dialogue.text = " ";
        }
        else
        {
            currentAmount = 0;
        }

        int lettersToDisplay = 0;
        
        foreach (char c in paragraphs[paraNum])
        {
            if (lettersToDisplay < currentAmount)
            {
                dialogue.text += c;
            }

            lettersToDisplay++;
        }

        if (dialogue.text.Length <= paragraphs[paraNum].Length && !stop) //&& !isRunning)
        {
            currentAmount++;
            StartCoroutine(LetterDelay(paraNum, currentAmount));
        }
        else
        {
            isRunning = false;
            stop = false;
        }
    }

    IEnumerator LetterDelay(int paraNum, int amountOfLetters)
    {
        
        if (!stop)
        {
            yield return new WaitForSeconds(0.02f);
            DisplayParagraph(paraNum, amountOfLetters, true);
        }
    }

    public void StopSentence()
    {
        if (dialogue.text.Length > paragraphs[sentenceNumber].Length)
        {
            if(sentenceNumber == 2)
            {
                rings[0].SetActive(true);
                rings[1].SetActive(true);
            }
            tutCounter = 0f;
            moveOnCheck = true;
        }
    }

    IEnumerator StartIduction()
    {
        moveOnCheck = false;
        DisplayParagraph(sentenceNumber,1, false);

        while(!moveOnCheck)
        {
            yield return true;
        }

        if (sentenceNumber < paragraphs.Length - 1)
        {
            sentenceNumber++;
            StartCoroutine(StartIduction());
        }
        else
        {
            dialogue.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }

    public void UpdateRingCount()
    {
        ringCount++;
        rings[ringCount - 1].SetActive(false);
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
