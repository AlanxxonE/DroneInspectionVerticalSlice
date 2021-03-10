using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMechanics : MonoBehaviour
{
    //Class references
    [HideInInspector]public HazardManager hazardManager;

    //General variables
    protected List<Transform> targetTransforms = new List<Transform>();  //Transform of the target game object, as it's protected it will be unique for any class that derives from hazard mechanics
    protected List<bool> targetFixed = new List<bool>();
    protected Transform cameraFocalPoint;
    protected int hazardIndex;
    protected int targetIndex = 0;
    protected bool checkCameraPosition = false;   //Boolean used to determine if the CheckCameraPosition() method should run

    /// <summary>
    /// Sets current script inactive and sets it's unique target transform
    /// </summary>
    /// <param name="nameOfTargetGameObject"></param>
    protected void OnWake()
    {
        GetComponent<MonoBehaviour>().enabled = false;

        foreach(Transform child in transform)
        {
            if (child.CompareTag("Target"))
            {
                targetTransforms.Add(child.transform);
                targetFixed.Add(false);
            }
        }

        hazardManager.hazardTransforms.Add(transform);
        hazardIndex = hazardManager.hazardTransforms.LastIndexOf(transform);        
    }

    /// <summary>
    /// Initiates unique variables for the script the calls this method
    /// </summary>
    protected void InitiateVariables()
    {
        checkCameraPosition = true;
        ReturnAverageOfTransforms(targetTransforms);
    }

    protected void RunHazard(float sliderProgress, Transform cameraFocalPoint, int index)
    {
        if (checkCameraPosition)
        {
            if (CheckCameraPosition(cameraFocalPoint))
            {
                checkCameraPosition = false;
                hazardManager.gameManager.droneController.droneCamera.interpolationTime = 0;
            }
        }

        else
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

    protected bool CheckCursorState()
    {
        Ray ray;
        RaycastHit hit;
        ray = hazardManager.gameManager.droneController.thirdPersonCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        bool checkIfComplete = true;

        if (Physics.Raycast(ray , out hit))
        {
            switch (hit.collider.tag)
            {
                case "Target":
                    targetIndex = targetTransforms.IndexOf(hit.collider.transform);
                    targetTransforms[targetIndex].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        targetTransforms[targetIndex].tag = "Fixed";
                        targetFixed[targetIndex] = true;
                        targetTransforms[targetIndex].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    }
                    break;

                default:
                    targetTransforms[targetIndex].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    break;
            }                  
        }
       
        foreach (bool check in targetFixed)
        {
            if (check == false)
            {
                checkIfComplete = false;
            }
        }

        if (checkIfComplete == true)
        {
            return true;
        }
        return false;
    }

    private bool CheckCameraPosition(Transform cameraFocalPoint)
    {        
        if(checkCameraPosition) ///maybe take out
        {
            if (hazardManager.gameManager.droneController.droneCamera.FocusOnHazard(cameraFocalPoint, false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

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