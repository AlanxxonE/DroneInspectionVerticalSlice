using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMechanics : MonoBehaviour
{
    /// <summary>
    /// Class to hold variables and methods that all hazard classes inherit from
    /// </summary>
    
    //Class references
    public HazardManager hazardManager; //Reference to the hazard manager class

    //General variables
    protected List<Transform> targetTransforms = new List<Transform>();  //Transform of the target game object, as it's protected it will be unique for any class that derives from hazard mechanics
    protected List<bool> targetFixed = new List<bool>(); //List t o determine if specific hazard elements are fixed
    protected Transform cameraFocalPoint;  //Position the camera focuses on
    protected int hazardIndex;  //Index for the list of hazard transforms
    protected int targetIndex = 0; //Target index to distinguish between targets on a hazard
    protected bool checkCameraPosition = false;   //Boolean used to determine if the CheckCameraPosition() method should run

    /// <summary>
    /// Method to handle on wake functions of each hazard class
    /// </summary>
    protected void OnWake()
    {
        GetComponent<MonoBehaviour>().enabled = false; //Disables class

        foreach(Transform child in transform)
        {
            if (child.CompareTag("Target"))
            {
                targetTransforms.Add(child.transform);  //Adds transforms of each child target of each hazard to it's own list
                targetFixed.Add(false); //Sets the fixed state of each of these to false
            }
        }

        hazardManager.hazardTransforms.Add(transform);  //Adds the transform of this hazard to the list of hazard transforms in hazard manager
        hazardIndex = hazardManager.hazardTransforms.LastIndexOf(transform); //Holds the index of this hazard as it's added to the list of hazard transforms in hazard manager      
    }

    /// <summary>
    /// Initiates unique variables for the script that calls this method
    /// </summary>
    protected void InitiateVariables()
    {
        checkCameraPosition = true;  
        ReturnAverageOfTransforms(targetTransforms);  
    }

    /// <summary>
    /// Manages the hazard mechanics as it runs. Takes in a float for porgress, the point the camera should focus on when a hazard is interacted with and the index for this 
    /// hazard in the list of hazard transforms in hazard manager 
    /// </summary>
    /// <param name="sliderProgress"></param>
    /// <param name="cameraFocalPoint"></param>
    /// <param name="index"></param>
    protected void RunHazard(float sliderProgress, Transform cameraFocalPoint, int index)
    {
        if (checkCameraPosition) //If camera is not in position, i.e. a hazard has just been interacted with or just finished the minigame and the camera must move
        {
            if (CheckCameraPosition(cameraFocalPoint)) //Calls the check camera position method, if it returns true....
            {
                checkCameraPosition = false; //Stops calling this method
                hazardManager.gameManager.droneController.droneCamera.interpolationTime = 0; //Resets interpolation time timer
            }
        }

        else //If camera is in position, minigame will start
        {
            if (hazardManager.hazardSlider.value >= 100)  //Calls the finish hazard method in the hazard manager script if the minigame is won and passes through these variables
            {
                hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).satisfaction, Score.GetScore(hazardManager.hazardName).score, true, cameraFocalPoint, index);
            }
            else if (hazardManager.hazardSlider.value <= 0)  //Calls the finish hazard method in the hazard manager script if the minigame is lost and passes through these variables
            {
                hazardManager.FinishHazard(Score.GetScore(hazardManager.hazardName).dissatisfaction, 0, false, cameraFocalPoint, index);
            }

            else if(hazardManager.hazardSlider.value < 100 && hazardManager.hazardSlider.value > 0)
            {
                hazardManager.hazardSlider.value += sliderProgress - (hazardManager.hazardProgressDropRate * Time.deltaTime);  //Sets the rate at which the hazard timer counts down ////Don't move this
            }            
        }        
    }

    /// <summary>
    /// Method to check current status of the cursor and return true if each hazard target has been interacted with correctly
    /// </summary>
    /// <returns></returns>
    protected bool CheckCursorState()
    {
        Ray ray;   //Holds reference to the raycast
        RaycastHit hit; //Holds reference to what the raycast hits
        ray = hazardManager.gameManager.droneController.thirdPersonCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);  //Shoots raycast from TPP camera to mouse position

        bool checkIfComplete = true;  //Bool to determine if hazard minigame is complete, i.e. has each target been interacted with correctly. Sets true each time method is run

        if (Physics.Raycast(ray, out hit)) // If raycast hits something
        {
            switch (hit.collider.tag) //Compares tag of what raycast hit
            {
                case "Target":  
                    targetIndex = targetTransforms.IndexOf(hit.collider.transform);  //Gets index of which target is being interacted with
                    targetTransforms[targetIndex].GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); //Sets the glow effect of that target
                    
                    if (Input.GetMouseButtonDown(0)) //If mouse 1 is clicked
                    {
                        targetTransforms[targetIndex].tag = "Fixed"; //Changes tag of target to fixed
                        targetFixed[targetIndex] = true; //Sets the boolean for that particular target in the targetFixed boolean list equal to true
                        targetTransforms[targetIndex].GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); //Stops the glow effect of that target
                    }
                    break;

                default:
                    targetTransforms[targetIndex].GetComponent<Renderer>().material.DisableKeyword("_EMISSION"); //Stops the glow effect of the last target interacted with if raycast is no longer hitting a target
                    break;
            }                  
        }
       
        foreach (bool check in targetFixed) //Loops through targetFixed boolean list to check if each target has been fixed
        {
            if (check == false)  //If even one is not fixed 
            {
                checkIfComplete = false; //Sets checkIfComplete boolean to false
            }
        }

        if (checkIfComplete == true)  //If each element of targetFixed is true
        {
            return true; //Returns true, i.e. minigame is complete
        }
        return false;
    }

    /// <summary>
    /// Method to return true/false dependent on the camera's position. Takes in a transform for the focal point of the camera for that particular hazard
    /// </summary>
    /// <param name="cameraFocalPoint"></param>
    /// <returns></returns>
    private bool CheckCameraPosition(Transform cameraFocalPoint)
    {                
        if (hazardManager.gameManager.droneController.droneCamera.FocusOnHazard(cameraFocalPoint, false))
        {
            return true; //If focus on hazard returns true, this returns true
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method that takes in a list of transforms and returns the average of each as one new transform by creating a new temporary game object that will be deleted.
    /// New transform used as camera focal point.
    /// </summary>
    /// <param name="targetTransforms"></param>
    private void ReturnAverageOfTransforms(List<Transform> targetTransforms)
    {
        float count = targetTransforms.Count;       
        cameraFocalPoint = new GameObject().transform;

        foreach (Transform t in targetTransforms)
        {
            cameraFocalPoint.position = cameraFocalPoint.position + t.position / count;
            cameraFocalPoint.eulerAngles += t.rotation.eulerAngles / count;
            cameraFocalPoint.localScale += t.localScale / count;
        }
    }
}