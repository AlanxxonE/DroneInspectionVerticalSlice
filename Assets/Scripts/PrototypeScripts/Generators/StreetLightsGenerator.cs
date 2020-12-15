using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightsGenerator : MonoBehaviour
{
    /// <summary>
    /// A Class that generates the street lights on the edges of the streets
    /// </summary>

    /// <summary>
    /// User-defined type that determines the street light direction of spawn
    /// </summary>
    private enum Directions
    {
        Vertical,
        Horizontal
    };

    /// <summary>
    /// User-defined type that determines where the street light will facing at the moment of spawn
    /// </summary>
    private enum SpawnDirection
    {
        Left,
        Right,
        Forward,
        Backward
    };

    [SerializeField]
    private Directions typeOfDirection; //The variable that denominates the state of the direction of the street light

    [SerializeField]
    private SpawnDirection typeOfSpawn; //The variable that denominates the state of the street light based on where it has to face when generated

    [SerializeField]
    private GameObject childLight; //The gameobject that needs to be generated

    // Start is called before the first frame update
    void Start()
    {
        //A loop that generates as many street lights based on the lenght of the road
        for (int i = 0; i < 11; i++)
        {

            GameObject childStreetLight = Instantiate(childLight); //The clone that will be created based on the selected gameobject

            //Based on the type of direction, the position of the street light clone will be assigned accordingly, while also checking where the street light needs to face, creating a realistic street formation
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
