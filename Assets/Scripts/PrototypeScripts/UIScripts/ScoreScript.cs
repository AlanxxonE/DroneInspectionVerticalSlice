using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject scoreTextRef;
    public int score;

    public List<Image> isFixedImageList = new List<Image>();
    private List<bool> isFixedBoolList = new List<bool>();
    
    // Start is called before the first frame update
    void Start()
    {
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
}
