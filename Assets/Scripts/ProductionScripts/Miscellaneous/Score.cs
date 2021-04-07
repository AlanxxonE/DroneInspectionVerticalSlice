using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    /// <summary>
    /// Static class used to hold/manage various score values
    /// </summary>
    
    public static bool isScaffoldFixed, isCraneFixed, isAcrowFixed; //Booleans for wether a hazard is fixed or not

    public static string endMessage;

    //Returns score values for specific hazards. Takes in the hazard name as input
    public static (int timeGained, int timeLost, int score) GetScore(string hazardName)
    {
        switch (hazardName)
        {
            case "Scaffold":
                return (40, -20, 50);
            case "Crane":
                return (30, -20, 60);
            case "Acrow":
                return (20, -20, 100);
            default:
                return (0, 0, 0);
        }        
    }
    
    //Sets wether or not a hazard has been fixed
    public static void SetFixedBooleans(string hazardName, bool isFixed, bool resetBools)
    {               
        if(resetBools)
        {
            isScaffoldFixed = isCraneFixed = isAcrowFixed = !resetBools;
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
                case "Acrow":
                    isAcrowFixed = isFixed;
                    break;
                default:
                    break;
            }
        }
    }

    public static bool AreAllHazardsFixed()
    {
        if(isScaffoldFixed && isCraneFixed && isAcrowFixed)
        {
            endMessage = "WELL DONE SPOTTING THOSE HAZARDS!";
            return true;
        }

        return false;
    }
}