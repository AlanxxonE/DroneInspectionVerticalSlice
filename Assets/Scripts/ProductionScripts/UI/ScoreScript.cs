using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreScript : MonoBehaviour
{
    /// <summary>
    /// Class to hold data on the score of the game and which hazards were fixed
    /// </summary>
    
    //References
    public GameObject scoreTextRef;   //Reference to the score text
    public int score;  //Integer to hold the score

    public List<Image> isFixedImageList = new List<Image>();  //List of images of which hazards were/weren't fixed
    private List<bool> isFixedBoolList = new List<bool>();  //List of booleans of which hazards were fixed

    public GameObject cardsOverlayRef;  //Reference to the overlay of the hazard cards
     
    private PointerEventData pointerData = new PointerEventData(EventSystem.current);   //Gets data for what the mouse is hovering over
    private List<RaycastResult> pointerHitList = new List<RaycastResult>(); //Holds data for what the mouse is hovering over

    // Start is called before the first frame update
    void Start()
    {
        cardsOverlayRef.SetActive(false);  //Sets card overlays inactive

        score = LevelManager.scoreValue;  //Sets score based on the score value in the level manager
        scoreTextRef.GetComponent<Text>().text = "Your score is: " + score.ToString();  //Writes score

        //Adds the booleans for which hazards are fixed to the isFixedBoolList from the level manager
        isFixedBoolList.Add(LevelManager.isScaffoldFixed); 
        isFixedBoolList.Add(LevelManager.isCraneFixed);
        
        for (int i = 0; i < isFixedImageList.Count; i++)
        {
            if (isFixedBoolList[i] == true)
            {
                isFixedImageList[i].color = Color.green;   //Sets colour of fixed hazards to green 
            }
            else if (isFixedBoolList[i] == false)
            {
                isFixedImageList[i].color = Color.red;   //Sets colour of not fixed hazards to red
            }
        }
    }

    private void Update()
    {
        pointerData.position = Input.mousePosition;   //Sets pointer data position equal to the mouse position
        EventSystem.current.RaycastAll(pointerData, pointerHitList);   

        HazardInfoCards();
    }

    /// <summary>
    /// Method to display to information overlay of the hazard cards
    /// </summary>
    void HazardInfoCards()
    {
        //Sets overlay cards inactive if the mouse cursor isn't above a hazard card
        if (pointerHitList.Count == 0)
        {
            if (cardsOverlayRef.activeSelf == true)
            {
                cardsOverlayRef.SetActive(false);
            }
        }

        else
        {
            for (int i = 0; i < pointerHitList.Count; i++)
            {
                if (cardsOverlayRef.activeSelf == false)
                {
                    string a = null, b = null, c = null;
                    bool cardActive = false;

                    //If the pointer is over a hazard card it sets the new overlay card text strings corresponding to that hazard and sets the cards state to true, if not the card state is set to false
                    switch (pointerHitList[i].gameObject.tag)
                    {
                        case "ScaffoldHazard":
                            a = "Scaffold"; b = "Uncrewed Bolt"; c = "Amber"; cardActive = true;
                            break;

                        case "CraneHazard":
                            a = "Crane"; b = "Torn Wire"; c = "Red"; cardActive = true;
                            break;

                        case "Untagged":
                            cardActive = false;
                            break;

                        default:
                            break;
                    }

                    cardsOverlayRef.SetActive(cardActive); //Sets the card active or inactive 

                    //Sets the text on the hazard overlay cards
                    cardsOverlayRef.GetComponentsInChildren<Text>()[0].text = "NAME: " + a + ";";
                    cardsOverlayRef.GetComponentsInChildren<Text>()[1].text = "HAZARD: " + b + ";";
                    cardsOverlayRef.GetComponentsInChildren<Text>()[2].text = "DANGER: " + c + ";";
                }
                else
                {
                    cardsOverlayRef.transform.position = new Vector2(pointerData.position.x, Screen.height / 2); //Sets the position of the overlay card 
                }
            }
        }
    }
}
