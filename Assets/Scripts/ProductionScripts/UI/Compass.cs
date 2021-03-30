using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    /// <summary>
    /// Responsible for handling the compass mechanics
    /// </summary>
    
    //Class reference 
    [Tooltip("Reference to the UIManager class")]
    public UIManager UIManager;

    //References
    private RawImage compassCoords;  //Image ref for the compass coordinates, i.e. North, West, East and South
    private List<Transform> hazardTransforms; //List of transforms for each hazard
    public List<GameObject> hazardMarkers; //List of markers used overlayed on the compass indicating position of hazards   

    //Variables
    private float compassUnit; //Unit used to determine how much the compass should rotate by
    private bool markersInstantiated; //Determines if markers have been instantited

    private void Awake()
    {
        compassCoords = GetComponent<RawImage>(); 
        StartCoroutine(InstantiateHazardMarkers(Time.deltaTime)); //Waits one frame after Awake() to instantiate the hazard markers(prevents null referneces)
        compassUnit = compassCoords.rectTransform.rect.width / 360; //Sets the compass unit
    }

    private void Update()
    {
        compassCoords.uvRect = new Rect(UIManager.gameManager.droneController.transform.localEulerAngles.y / 360f, 0f, 1f, 1f); //Sets the size of the compass
        hazardTransforms = UIManager.gameManager.droneController.gameManager.hazardManager.GetHazardTransforms(); //Sets the list of hazard transforms

        if (markersInstantiated) //If markers have been instantiated
        {
            foreach (GameObject marker in hazardMarkers)
            {
                if (GetHazardHeightDifference(hazardTransforms[hazardMarkers.IndexOf(marker)]) < UIManager.hazardArrowHeightRange.x)
                {                    
                    marker.GetComponent<RawImage>().texture = UIManager.hazardMarkerUpArrow.GetComponent<RawImage>().texture;
                }

                else if (GetHazardHeightDifference(hazardTransforms[hazardMarkers.IndexOf(marker)]) > UIManager.hazardArrowHeightRange.y)
                {                    
                    marker.GetComponent<RawImage>().texture = UIManager.hazardMarkerDownArrow.GetComponent<RawImage>().texture;
                }

                else if (GetHazardHeightDifference(hazardTransforms[hazardMarkers.IndexOf(marker)]) > UIManager.hazardArrowHeightRange.x && GetHazardHeightDifference(hazardTransforms[hazardMarkers.IndexOf(marker)]) < UIManager.hazardArrowHeightRange.y)
                {
                    marker.GetComponent<RawImage>().texture = UIManager.hazardMarkerPrefab.GetComponent<RawImage>().texture;
                }

                marker.GetComponent<RawImage>().rectTransform.anchoredPosition = GetHazardMarkerPositionOnCompass(hazardTransforms[hazardMarkers.IndexOf(marker)]); //Gets/Sets position of hazard marker
                float scale = GetHazardMarkerScale(hazardTransforms[hazardMarkers.IndexOf(marker)]); //Gets new scale for hazard marker
                marker.GetComponent<RawImage>().rectTransform.localScale = new Vector3(scale, scale, scale);  //Sets new scale for hazard marker              
            }           
        }                  
    }

    /// <summary>
    /// Translates the relative position of a hazard compared to the drone to a coordinate on the compass. Takes in the transform of that hazard as input.
    /// </summary>
    /// <param name="marker"></param>
    /// <returns></returns>
    private Vector2 GetHazardMarkerPositionOnCompass(Transform marker)
    {
        Vector3 targetVector = marker.position - UIManager.gameManager.droneController.transform.position; //Vector of relative position between marker and drone
        Vector3 forwardVector = UIManager.gameManager.droneController.transform.forward; //Forward vector of drone

        targetVector.y = 0;
        forwardVector.y = 0;  

        //Gets and returns the angle between relative vector and forward vector of the drone
        float angle = Vector3.Angle(targetVector, forwardVector); 
        angle *= Mathf.Sign(Vector3.Cross(forwardVector, targetVector).y);  
        return new Vector2(compassUnit * angle, 0f);
    }

    /// <summary>
    /// Returns new value for the scale of the hazard marker dependent on distance from drone to hazard.
    /// </summary>
    /// <param name="marker"></param>
    /// <returns></returns>
    private float GetHazardMarkerScale(Transform marker)
    {
        Vector3 hazardPos = new Vector3(marker.position.x, 0 , marker.position.z);
        Vector3 dronePos = new Vector3(UIManager.gameManager.droneController.transform.position.x, 0, UIManager.gameManager.droneController.transform.position.z);

        float distance = Vector3.Distance(hazardPos, dronePos); //Gets horizontal distance between drone/hazard

        if(distance > UIManager.hazardMarkerScaleRange.y) //If distance > max distance
        {
            return UIManager.hazardMarkerScale.x; //Returns min scale
        }

        else if(distance < UIManager.hazardMarkerScaleRange.x) //If distance < min distance
        {
            return UIManager.hazardMarkerScale.y; //Returns max scale
        }

        else //Returns a linearly interpolated value between min/max scale based on distance between min/max distance
        {
            float temp = (distance - UIManager.hazardMarkerScaleRange.x )/ UIManager.hazardMarkerScaleRange.y;
            float returnValue = UIManager.hazardMarkerScale.y - (temp * (UIManager.hazardMarkerScale.y - UIManager.hazardMarkerScale.x));
            return returnValue;
        }
    }

    private float GetHazardHeightDifference(Transform arrow)
    {
        float heightDifference = UIManager.gameManager.droneController.transform.position.y - arrow.position.y;
        print(heightDifference);
        return heightDifference;
    }
    
    /// <summary>
    /// Instantiates list of hazard markers
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
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