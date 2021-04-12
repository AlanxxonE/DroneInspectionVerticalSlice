using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private GameManager gameManager;
    
    private Text dialogue;
    int currentAmount = 0;
    public int sentenceNumber = 0;

    bool moveOnCheck = false;
    bool sentenceFinished;
    string[] paragraphs;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        dialogue = GameObject.FindGameObjectWithTag("DialogueSystem").GetComponent<Text>();
    }

    public int GetParagraphsLength()
    {
        return paragraphs.Length;
    }

    public void DisplayParagraph(int paraNum, int amountOfLetters)
    {
        currentAmount = amountOfLetters;
        dialogue.text = " ";

        int lettersToDisplay = 0;
        
        foreach (char c in paragraphs[paraNum])
        {
            if (lettersToDisplay < currentAmount)
            {
                dialogue.text += c;
            }

            lettersToDisplay++;
        }

        if (dialogue.text.Length <= paragraphs[paraNum].Length)
        {
            currentAmount++;
            StartCoroutine(LetterDelay(paraNum, currentAmount));
        }
    }

    IEnumerator LetterDelay(int paraNum, int amountOfLetters)
    {
        yield return new WaitForFixedUpdate();
        DisplayParagraph(paraNum, amountOfLetters);
    }

    public void StopSentence()
    {
        if (IsSentenceFinished())
        {
            moveOnCheck = true;
        }
    }

    IEnumerator StartDialogue()
    {
        moveOnCheck = false;
        DisplayParagraph(sentenceNumber,1);

        while(!moveOnCheck)
        {
            yield return true;
        }

        if (sentenceNumber < paragraphs.Length - 1)
        {
            sentenceNumber++;
            StartCoroutine(StartDialogue());
        }
        else
        {
            dialogue.transform.parent.gameObject.SetActive(false);
        }
    }
    public void UpdateParagraphs(string[] newParagraphs)
    {
        paragraphs = System.Array.Empty<string>();
        paragraphs = newParagraphs;
    }

    public void RunDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    public int GetSentenceNumber()
    {
        return sentenceNumber;
    }

    public bool IsSentenceFinished()
    {
        if (dialogue.text.Length > paragraphs[sentenceNumber].Length)
        {
            sentenceFinished = true;
        }
        else
        {
            sentenceFinished = false;
        }

        return sentenceFinished;
    }
}
