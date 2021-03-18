using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueSystem;
    public TutRings tutMarker;
    public GameManager gameManager;
    public Collider tutHazard;
    public Collider droneCollider;
    Text dialogue;
    int currentAmount = 0;
    float tutCounter = 0f;

    bool isRunning = false;
    bool stop = false;

    bool moveOnCheck = false;

    int sentenceNumber = 0;

    string[] paragraphs = {
        "Welcome to the construstion site! First thing, try to get the feel of the Drone's controls. Use the MOUSE to look around",
        "Your drone is built with an indicator for the signal. Be careful not to go out of range or you'll lose control. Now try moving, use the W,A,S,D keys to move the drone",
        "Your altitude meter here gives you an idea of how far off the ground you are. Use SPACE key to Ascend, SHIFT key to Descend",
        "Alright, now that you've got a feel for it we'll need to configure your compass. Fly through the rings this will also give you a chance to get more familiar with the site",
        "One of the guys working pointed out an issue at one of the scaffolds. The point on your compass will lead you there.",
        "Hazards present different levels of danger within the site and therefore should potentially be treated with different priority, these danger levels are displayed on the drone’s compass as green, amber or red. Take a closer look at the scaffold. Use C to switch to first person",
        "Alright, like most cameras that drone needs to been in a good position for proper focus, get yourself in a good position, not too close, not too far, and you should be able to see it clearly. If a hazard is in your view but your range is incorrect, the viewfinder will appear red, once in the correct position the viewfinder will turn green. Anyway, see that bolt there? Doesn’t look quite screwed in all the way, that could leave the whole thing unstable, get someone over here to fix it  [you can call in a worker to fix the hazard by clicking]",
        "Make sure you’re screwing that in the right way, get it nice and tight before this thing falls apart"};

    void Awake()
    {
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
                tutCounter += Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime;

                if (tutCounter > 1) { StopSentence(); }
                break;
            //WASD movement
            case 1:
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
                tutMarker.gameObject.SetActive(true);

                if(tutMarker.GetRingsDone()) { StopSentence(); }
                break;
            //Compass
            case 4:
                gameManager.UIManager.compass.gameObject.SetActive(true);
                if (Vector3.Distance(tutHazard.gameObject.transform.position, gameManager.droneController.gameObject.transform.position) < 20) { StopSentence(); }
                break;
            //camera switch
            case 5:
                if (Input.GetButtonDown("ToggleCam")) { StopSentence(); }
                break;
            //examine hazard
            case 6:
                tutHazard.enabled = true;

                if(tutHazard.GetComponent<MonoBehaviour>().enabled == true) { StopSentence(); }
                break;
            //fix hazard
            case 7:
                if(gameManager.hazardManager.hazardSliderRef.activeSelf == false) { StopSentence(); }
                break;
            default:
                if (Input.GetKeyDown(KeyCode.Return)) { StopSentence(); }
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
            yield return new WaitForSeconds(0.05f);
            DisplayParagraph(paraNum, amountOfLetters, true);
        }
    }

    public void StopSentence()
    {
        if (dialogue.text.Length > paragraphs[sentenceNumber].Length)
        {
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
}
