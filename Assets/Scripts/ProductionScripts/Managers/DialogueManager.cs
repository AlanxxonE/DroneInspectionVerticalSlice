using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueSystem;
    public Transform tutMarker;
    public Collider droneCollider;
    Text dialogue;
    int currentAmount = 0;
    float mouseMovement = 0f;

    Vector3 previousMousePosition;
    bool isRunning = false;
    bool stop = false;

    bool tutCheck1 = false;
    bool tutCheck2 = false;
    bool tutCheck3 = false;
    bool tutCheck4 = false;
    bool moveOnCheck = false;

    int sentenceNumber = 0;

    string[] paragraphs = { 
        "Welcome to the construstion site worker!", 
        "Okey, first thing, try to get the feel of the Drone's controls",
        "Use the MOUSE to look around",
        "Move around using the W,A,S,D keys", 
        "Now make sure the propellers are working as well",
        "Use SPACE key to Ascend, SHIFT key to Descend",
        "Lets get close to those Escavators",
        "Try using the C key so you can look better at things",
        "This way you can analyze and inspect hazards",
        "A worker reported something seemed off near the main big building!",
        "Check the scaffolds using the LEFT MOUSE BUTTON and see if you notice something out of place"};

    void Awake()
    {
        dialogue = dialogueSystem.GetComponentInChildren<Text>();
        //dialogueSystem.SetActive(false);
        StartCoroutine(StartIduction(sentenceNumber));
    }

    void Update()
    {
        switch (sentenceNumber)
        {
            case 2:
                tutCheck2 = true;
                tutCheck3 = true;
                tutCheck4 = true;

                mouseMovement += Mathf.Abs(Input.GetAxis("Mouse X")) * Time.deltaTime;

                if (mouseMovement > 1){tutCheck1 = true;}
                break;
            case 3:
                if(Input.GetKeyDown(KeyCode.W)){tutCheck1 = true;}
                if(Input.GetKeyDown(KeyCode.A)){tutCheck2 = true;}
                if(Input.GetKeyDown(KeyCode.S)){tutCheck3 = true;}
                if(Input.GetKeyDown(KeyCode.D)){tutCheck4 = true;}
                break;
            case 5:
                tutCheck3 = true;
                tutCheck4 = true;

                if (Input.GetKeyDown(KeyCode.Space)) { tutCheck1 = true; }
                if (Input.GetKeyDown(KeyCode.LeftShift)) { tutCheck2 = true; }
                break;
            case 6:
                tutCheck2 = true;
                tutCheck3 = true;
                tutCheck4 = true;

                if (Vector3.Distance(tutMarker.position, droneCollider.gameObject.transform.position) < 10){tutCheck1 = true;}
                break;
            case 7:
                tutCheck2 = true;
                tutCheck3 = true;
                tutCheck4 = true;

                if (Input.GetKeyDown(KeyCode.C)) { tutCheck1 = true; }
                break;
            default:
                tutCheck2 = true;
                tutCheck3 = true;
                tutCheck4 = true;

                if (Input.GetKeyDown(KeyCode.Return)){tutCheck1 = true;}
                break;
        }

        if (tutCheck1 && tutCheck2 && tutCheck3 && tutCheck4) {StopSentence();}
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
            moveOnCheck = true;
        }
    }

    IEnumerator StartIduction(int sentNum)
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
            StartCoroutine(StartIduction(sentenceNumber));
            tutCheck1 = false;
            tutCheck2 = false;
            tutCheck3 = false;
            tutCheck4 = false;
        }
        else
        {
            dialogue.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }
}
