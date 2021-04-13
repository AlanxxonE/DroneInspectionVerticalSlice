using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    /// <summary>
    /// Static class used to hold/manage various score values
    /// </summary>
    
    public static bool isScaffoldFixed, isCraneFixed, isAcrowFixed, isPropaneTankFixed, isHoistBucketFixed; //Booleans for wether a hazard is fixed or not

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
                return (40, -20, 100);
            case "PropaneTank":
                return (30, -30, 70);
            case "HoistBucket":
                return (40, -20, 70);
            default:
                return (0, 0, 0);
        }        
    }
    
    //Sets wether or not a hazard has been fixed
    public static void SetFixedBooleans(string hazardName, bool isFixed, bool resetBools)
    {               
        if(resetBools)
        {
            isScaffoldFixed = isCraneFixed = isAcrowFixed = isPropaneTankFixed = isHoistBucketFixed = !resetBools;
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
                case "PropaneTank":
                    isPropaneTankFixed = isFixed;
                    break;
                case "HoistBucket":
                    isHoistBucketFixed = isFixed;
                    break;
                default:
                    break;
            }
        }
    }

    public static bool AreAllHazardsFixed()
    {
        if(isScaffoldFixed && isCraneFixed && isAcrowFixed && isPropaneTankFixed)
        {
            endMessage = "WELL DONE SPOTTING THOSE HAZARDS!";
            return true;
        }

        return false;
    }
}