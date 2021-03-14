using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    /// <summary>
    /// Static class used to hold/manage various score values
    /// </summary>
    
    public static bool isScaffoldFixed, isCraneFixed; //Booleans for wether a hazard is fixed or not

    //Returns score values for specific hazards. Takes in the hazard name as input
    public static (int satisfaction, int dissatisfaction, int score) GetScore(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                return (40, -20, 50);
            case "Crane":
                return (30, -20, 60);
            default:
                return (0, 0, 0);
        }        
    }
    
    //Sets wether or not a hazard has been fixed
    public static void SetFixedBooleans(string hazardName, bool isFixed, bool resetBools)
    {               
        if(resetBools)
        {
            isScaffoldFixed = isCraneFixed = !resetBools;
        }

        else
        {
            switch (hazardName)
            {
                case "Scaffold":
                    isScaffoldFixed = isFixed;
                    break;
                case "Crane":
                    isCraneFixed = isFixed;
                    break;
                default:
                    break;
            }
        }
    }
}