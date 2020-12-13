using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreScript : MonoBehaviour
{
    public GameObject scoreTextRef;
    public int score;

    public List<Image> isFixedImageList = new List<Image>();
    private List<bool> isFixedBoolList = new List<bool>();

    public GameObject cardsOverlayRef;

    private PointerEventData pointerData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> pointerHitList = new List<RaycastResult>();

    // Start is called before the first frame update
    void Start()
    {
        cardsOverlayRef.SetActive(false);

        score = LevelManager.scoreValue;
        scoreTextRef.GetComponent<Text>().text = "Your score is: " + score.ToString();

        isFixedBoolList.Add(LevelManager.isScaffoldFixed);
        isFixedBoolList.Add(LevelManager.isCraneFixed);
        
        for (int i = 0; i < isFixedImageList.Count; i++)
        {
            if (isFixedBoolList[i] == true)
            {
                isFixedImageList[i].color = Color.green;
            }
            else if (isFixedBoolList[i] == false)
            {
                isFixedImageList[i].color = Color.red;
            }
        }
    }

    private void Update()
    {
        pointerData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointerData, pointerHitList);

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
                if (pointerHitList[i].gameObject.CompareTag("ScaffoldHazard"))
                {
                    if (cardsOverlayRef.activeSelf == false)
                    {
                        cardsOverlayRef.SetActive(true);
                        cardsOverlayRef.GetComponentsInChildren<Text>()[0].text = "NAME: Scaffold;";
                        cardsOverlayRef.GetComponentsInChildren<Text>()[1].text = "HAZARD: Uncrewed Bolt;";
                        cardsOverlayRef.GetComponentsInChildren<Text>()[2].text = "DANGER: Amber;";
                    }
                    else
                    {
                        cardsOverlayRef.transform.position = new Vector2(pointerData.position.x, Screen.height / 2);
                    }
                }

                if (pointerHitList[i].gameObject.CompareTag("CraneHazard"))
                {
                    if (cardsOverlayRef.activeSelf == false)
                    {
                        cardsOverlayRef.SetActive(true);
                        cardsOverlayRef.GetComponentsInChildren<Text>()[0].text = "NAME: Crane;";
                        cardsOverlayRef.GetComponentsInChildren<Text>()[1].text = "HAZARD: Torn Wire;";
                        cardsOverlayRef.GetComponentsInChildren<Text>()[2].text = "DANGER: Red;";
                    }
                    else
                    {
                        cardsOverlayRef.transform.position = new Vector2(pointerData.position.x, Screen.height / 2);
                    }
                }

                if (pointerHitList[i].gameObject.CompareTag("Untagged"))
                {
                    cardsOverlayRef.SetActive(false);
                }
            }
        }
    }
}
