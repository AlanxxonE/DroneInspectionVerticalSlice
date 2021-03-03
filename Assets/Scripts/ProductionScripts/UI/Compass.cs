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
    private List<Transform> hazardTransforms;
    public GameObject hazardMarkerPrefab;
    [HideInInspector]public List<GameObject> hazardMarkers;
    private float compassUnit;
    public bool markersInstantiated;

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
                marker.GetComponent<RawImage>().rectTransform.anchoredPosition = GetHazardMarkerPositionOnCompass(marker);
            }
        }                  
    }

    private Vector2 GetHazardMarkerPositionOnCompass(GameObject marker)
    {
        Vector2 drone2hazardVec = new Vector2(marker.transform.position.x - droneController.transform.position.x, marker.transform.position.z - droneController.transform.position.z);
        float theta = Mathf.Tan(drone2hazardVec.x / drone2hazardVec.y);
        float alpha = droneController.transform.rotation.eulerAngles.y;
        float gamma = Mathf.Round(theta + alpha);
        Debug.Log(gamma);
        return new Vector2(compassUnit * gamma, 0f);
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