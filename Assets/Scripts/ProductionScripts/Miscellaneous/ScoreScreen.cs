using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    public Text endText;

    public Image scaffoldCard;

    private string endMessage;

    // Start is called before the first frame update
    void Start()
    {
        endMessage = Score.endMessage;
        endText.text = endMessage;

        if(Score.isScaffoldFixed)
        {
            scaffoldCard.color = Color.green;
        }
        else
        {
            scaffoldCard.color = Color.red;
        }
    }
}
