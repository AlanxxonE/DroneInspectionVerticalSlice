using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static bool isScaffoldFixed, isCraneFixed;

    public static void ResetScore()
    {
        isScaffoldFixed = false;
        isCraneFixed = false;
    }

    public static (int satisfaction, int dissatisfaction, int score) GetScore(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                return (40, 20, 50);
            case "Crane":
                return (30, 20, 60);
            default:
                return (0, 0, 0);
        }        
    }
    
    public static void SetFixedBooleans(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                isScaffoldFixed = true;
                break;
            case "Crane":
                isCraneFixed = true;
                break;
            default:
                break;
        }       
    }
}

