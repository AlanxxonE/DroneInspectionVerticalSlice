using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGenerator : MonoBehaviour
{
    /// <summary>
    /// Class that generates a fence around the construction site
    /// </summary>

    /// <summary>
    /// User-defined type that determines the fence direction of spawn
    /// </summary>
    private enum Directions
    {
        Vertical,   //The vertical direction of spawn
        Horizontal  //The horizontal direction of spawn
    };

    [SerializeField]
    private Directions typeOfDirection; //The directions variable that denominates the direction of spawn of the fence

    [SerializeField]
    private GameObject childFence; //The gameobject that needs to be generated

    // Start is called before the first frame update
    void Start()
    {
        //A loop that generates as many fences as the construction site needs
        for (int i = 0; i < 50; i++)
        {

            GameObject childFenceClone = Instantiate(childFence); //The clone that will be created based on the selected gameobject

            //Based on the type of direction, the position of the fence clone will be assigned or rotated accordingly, creating a coherent border
            if (typeOfDirection == Directions.Vertical)
            {
                childFenceClone.transform.position = new Vector3(childFence.transform.position.x, childFence.transform.position.y, childFence.transform.position.z + (4 * i));
            }
            else if (typeOfDirection == Directions.Horizontal && i < 49)
            {
                childFenceClone.transform.localEulerAngles = new Vector3(0, 90, 0);
                childFenceClone.transform.position = new Vector3(childFence.transform.position.x + (4 * i), childFence.transform.position.y, childFence.transform.position.z);
            }
        }
    }
}
