using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScrpt : MonoBehaviour
{
    public GameObject scoreTextRef;
    public int score;

    public List<Image> isFixedList = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        score = LevelManager.scoreValue;
        scoreTextRef.GetComponent<Text>().text = "Your score is: " + score.ToString();

        if(HazardTestScript.isScaffoldFixed == true)
        {
            isFixedList[0].color = Color.green;
        }
        else
        {
            isFixedList[0].color = Color.red;
        }

        if (HazardTestScript.isCraneFixed == true)
        {
            isFixedList[1].color = Color.green;
        }
        else
        {
            isFixedList[1].color = Color.red;
        }
    }
}
