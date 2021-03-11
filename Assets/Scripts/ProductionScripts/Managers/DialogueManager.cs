using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueSystem;
    public Collider droneCollider;
    Text dialogue;
    int currentAmount = 0;
    bool isRunning = false;

    int sentenceNumber = 0;

    string[] paragraphs = { 
        "Welcome to the construstion site worker!", 
        "Okey, first thing, try to get the feel of the Drone's controls",
        "Use the Mouse to look around",
        "Move around using the W,A,S,D keys", 
        "Now make sure the propellers are working as well",
        "Use Space key to Ascend, Shift key to Descend",
        "Lets get close to those Escavators",
        "Try using the C key so you can look better at things",
        "This way you can analyze and inspect hazards",
        "A worker reported something seemed off near the main big building!",
        "Check the scaffolds and see if you notice something out of place"};

    void Awake()
    {
        dialogue = dialogueSystem.GetComponentInChildren<Text>();
        //dialogueSystem.SetActive(false);
        StartCoroutine(StartIduction(sentenceNumber));
    }

    public void DisplayParagraph(int paraNum, int amountOfLetters)
    {
        if (!isRunning)
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

        if (dialogue.text.Length <= paragraphs[paraNum].Length && !isRunning)
        {
            currentAmount++;
            StartCoroutine(LetterDelay(paraNum, currentAmount));
        }
        else
        {
            isRunning = false;
        }
    }

    IEnumerator LetterDelay(int paraNum, int amountOfLetters)
    {
        yield return new WaitForSeconds(0.05f);
        DisplayParagraph(paraNum, amountOfLetters);
    }

    public void StopSentence()
    {
        if (currentAmount != 0)
        {
            isRunning = true;
        }
    }

    IEnumerator StartIduction(int sentNum)
    {
        DisplayParagraph(sentenceNumber,1);
        yield return new WaitForSeconds(10f);

        if (sentenceNumber < paragraphs.Length - 1)
        {
            sentenceNumber++;
            StartCoroutine(StartIduction(sentenceNumber));
        }
        else
        {
            dialogue.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }
}
