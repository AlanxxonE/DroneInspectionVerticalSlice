using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScrpt : MonoBehaviour
{
    public GameObject scoreTextRef;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        score = LevelManager.scoreValue;
        scoreTextRef.GetComponent<Text>().text = "Your score is: " + score.ToString();
    }
}
