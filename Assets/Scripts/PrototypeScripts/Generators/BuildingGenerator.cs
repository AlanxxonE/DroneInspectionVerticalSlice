using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingToClone;

    [SerializeField]
    private int numberOfBuildings = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[,] buildingMultiArray = new GameObject[numberOfBuildings, numberOfBuildings];

        for (int i = 0; i < buildingMultiArray.GetLength(0); i++) 
        {
            for(int j=0; j<buildingMultiArray.GetLength(1);j++)
            {
                GameObject buildingClone = Instantiate(buildingToClone);
                buildingClone.transform.position = buildingToClone.transform.position;
                buildingClone.transform.position = new Vector3(buildingToClone.transform.position.x + (i * 25), buildingToClone.transform.position.y, buildingToClone.transform.position.z + (j * 20));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
