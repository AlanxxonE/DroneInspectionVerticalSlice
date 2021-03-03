using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    //Class Reference 
    public DroneController droneController;

    //General variables
    private RawImage compassCoords;
    public List<Transform> hazardTransforms;
    public GameObject hazardMarkerPrefab;
    public List<GameObject> hazardMarkers;
    private float compassUnit;
    private bool markersInstantiated;
    [Tooltip("Sets the minimum and maximum scale of the compass marker")]
    public Vector2 hazardMarkerScale;
    [Tooltip("Sets the range for the minimum and maximum compass marker scale value in metres")]
    public Vector2 hazardMarkerScaleRange;

    private void Awake()
    {
        compassCoords = GetComponent<RawImage>();
        StartCoroutine(InstantiateHazardMarkers(Time.deltaTime));
        compassUnit = compassCoords.rectTransform.rect.width / 360;
    }

    private void Update()
    {
        compassCoords.uvRect = new Rect(droneController.transform.localEulerAngles.y / 360f, 0f, 1f, 1f);
        hazardTransforms = droneController.gameManager.hazardManager.GetHazardTransforms();
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
        Vector3 targetVector = marker.position - droneController.transform.position;
        Vector3 forwardVector = droneController.transform.forward;

        targetVector.y = 0; forwardVector.y = 0;

        float angle = Vector3.Angle(targetVector, forwardVector);
        angle *= Mathf.Sign(Vector3.Cross(forwardVector, targetVector).y);       
        return new Vector2(compassUnit * angle, 0f);
    }

    private float GetHazardMarkerScale(Transform marker)
    {
        float distance = Vector3.Distance(marker.position, droneController.transform.position);
        if(distance > hazardMarkerScaleRange.y)
        {
            return hazardMarkerScale.x; 
        }

        else if(distance < hazardMarkerScaleRange.x)
        {
            return hazardMarkerScale.y;
        }

        else
        {
            float temp = (distance - hazardMarkerScaleRange.x )/ hazardMarkerScaleRange.y;
            float returnValue = hazardMarkerScale.y -(temp * (hazardMarkerScale.y - hazardMarkerScale.x));
            return returnValue;
        }
    }

    IEnumerator InstantiateHazardMarkers(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        for(int i = 0; i < hazardTransforms.Count; i++)
        {          
            GameObject marker = Instantiate(hazardMarkerPrefab, transform);
            hazardMarkers.Add(marker);            
        }
        markersInstantiated = true;
    }
}