using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightsGenerator : MonoBehaviour
{
    private enum Directions
    {
        Vertical,
        Horizontal
    };
    private enum SpawnDirection
    {
        Left,
        Right,
        Forward,
        Backward
    };

    [SerializeField]
    private Directions typeOfDirection;

    [SerializeField]
    private SpawnDirection typeOfSpawn;

    [SerializeField]
    private GameObject childLight;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 11; i++)
        {

            GameObject childStreetLight = Instantiate(childLight);

            if (typeOfDirection == Directions.Vertical)
            {
                if(typeOfSpawn == SpawnDirection.Forward)
                {
                    childStreetLight.transform.position = new Vector3(childLight.transform.position.x, childLight.transform.position.y, childLight.transform.position.z + (20 * i));
                }
                else if(typeOfSpawn == SpawnDirection.Backward)
                {
                    childStreetLight.transform.position = new Vector3(childLight.transform.position.x, childLight.transform.position.y, childLight.transform.position.z + (-20 * i));
                }
                
            }
            else if (typeOfDirection == Directions.Horizontal && i < 10)
            {
                if (typeOfSpawn == SpawnDirection.Left)
                {
                    childStreetLight.transform.position = new Vector3(childLight.transform.position.x + (-21 * i), childLight.transform.position.y, childLight.transform.position.z);
                }
                else if(typeOfSpawn == SpawnDirection.Right)
                {
                    childStreetLight.transform.position = new Vector3(childLight.transform.position.x + (21 * i), childLight.transform.position.y, childLight.transform.position.z);
                }
                
            }
        }
    }
}
