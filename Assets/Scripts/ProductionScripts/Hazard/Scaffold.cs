using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    public Transform target;
    private bool keepCheckingCameras = true;

    private void Awake()
    {
        this.GetComponent<MonoBehaviour>().enabled = false;        
    }

    private void Update()
    {
        if(keepCheckingCameras)
        {
            CheckCameras();
        }        
        else
        {
            RunHazard(Mechanics(), target);            
        }        
    }

    private void CheckCameras()
    {
        if (CheckCameraPosition(target, keepCheckingCameras))
        {
            keepCheckingCameras = false;
            hazardManager.gameManager.droneController.droneCamera.interpolationTime = 0;

        }
        else if (!CheckCameraPosition(target, keepCheckingCameras))
        {
            CheckCameraPosition(target, keepCheckingCameras);
        }
    }

    private float Mechanics()
    {
        float progress = 10 * Time.deltaTime; ///Change for the actual mechanics          
        return progress;
    }
}
