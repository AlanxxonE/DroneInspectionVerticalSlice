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



    string[] paragraphs = { "a", "Welcome to the construstion site worker!", "Today we are going to fix the pipe", "d" };

    void Awake()
    {
        dialogue = dialogueSystem.GetComponentInChildren<Text>();
        //dialogueSystem.SetActive(false);
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
}
