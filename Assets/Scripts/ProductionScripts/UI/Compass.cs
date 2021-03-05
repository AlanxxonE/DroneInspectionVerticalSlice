using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    //Class reference 
    [Tooltip("Reference to the UIManager class")]
    public UIManager UIManager;

    //References
    private RawImage compassCoords;
    private List<Transform> hazardTransforms;
    [HideInInspector]public List<GameObject> hazardMarkers;

    //Variables
    private float compassUnit;
    private bool markersInstantiated;

    private void Awake()
    {
        compassCoords = GetComponent<RawImage>();
        StartCoroutine(InstantiateHazardMarkers(Time.deltaTime));
        compassUnit = compassCoords.rectTransform.rect.width / 360;
    }

    private void Update()
    {
        compassCoords.uvRect = new Rect(UIManager.gameManager.droneController.transform.localEulerAngles.y / 360f, 0f, 1f, 1f);
        hazardTransforms = UIManager.gameManager.droneController.gameManager.hazardManager.GetHazardTransforms();
        if (markersInstantiated)
        {
            foreach (GameObject marker in hazardMarkers)
            {               
                marker.GetComponent<RawImage>().rectTransform.anchoredPosition = GetHazardMarkerPositionOnCompass(hazardTransforms[hazardMarkers.IndexOf(marker)]);
                float scale = GetHazardMarkerScale(hazardTransforms[hazardMarkers.IndexOf(marker)]);
                marker.GetComponent<RawImage>().rectTransform.localScale = new Vector3(scale, scale, scale);                
            }
        }                  
    }

    private Vector2 GetHazardMarkerPositionOnCompass(Transform marker)
    {
        Vector3 targetVector = marker.position - UIManager.gameManager.droneController.transform.position;
        Vector3 forwardVector = UIManager.gameManager.droneController.transform.forward;

        targetVector.y = 0; forwardVector.y = 0;

        float angle = Vector3.Angle(targetVector, forwardVector);
        angle *= Mathf.Sign(Vector3.Cross(forwardVector, targetVector).y);       
        return new Vector2(compassUnit * angle, 0f);
    }

    private float GetHazardMarkerScale(Transform marker)
    {
        float distance = Vector3.Distance(marker.position, UIManager.gameManager.droneController.transform.position);
        if(distance > UIManager.hazardMarkerScaleRange.y)
        {
            return UIManager.hazardMarkerScale.x; 
        }

        else if(distance < UIManager.hazardMarkerScaleRange.x)
        {
            return UIManager.hazardMarkerScale.y;
        }

        else
        {
            float temp = (distance - UIManager.hazardMarkerScaleRange.x )/ UIManager.hazardMarkerScaleRange.y;
            float returnValue = UIManager.hazardMarkerScale.y -(temp * (UIManager.hazardMarkerScale.y - UIManager.hazardMarkerScale.x));
            return returnValue;
        }
    }

    IEnumerator InstantiateHazardMarkers(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        for(int i = 0; i < hazardTransforms.Count; i++)
        {          
            GameObject marker = Instantiate(UIManager.hazardMarkerPrefab, transform);
            hazardMarkers.Add(marker);            
        }
        markersInstantiated = true;
    }
}