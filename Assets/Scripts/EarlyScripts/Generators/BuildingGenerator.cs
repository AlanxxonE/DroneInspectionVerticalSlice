using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    /// <summary>
    /// Class that generates the background city
    /// </summary>
    
    [SerializeField]
    private List<GameObject> buildingToCloneList; //A list of buildings that the generators will use in order to have various assets to fetch upon

    [SerializeField]
    private int numberOfBuildings = 0; //The number the generator will use to determine how many buildings need to be created per generator

    private GameObject buildingClone; //The clone that will be created based on the list of gameobjects

    private float buildingHeight = 0; //When used, determines what height scale will the new generated building will have after being spawned

    // Start is called before the first frame update
    void Start()
    {
        //A multi-dimensional array that generates a specific amount of buildings in an ordered grid
        GameObject[,] buildingMultiArray = new GameObject[numberOfBuildings, numberOfBuildings];

        //A loop for the rows of the multi-dimensional array grid
        for (int i = 0; i < buildingMultiArray.GetLength(0); i++) 
        {
            //A loop for the columns of the multi-dimensional array grid
            for (int j = 0; j < buildingMultiArray.GetLength(1); j++) 
            {
                int z = Random.Range(0, buildingToCloneList.Count); //A Random value that will determine which building will be cloned

                //Randomly assign a new height value based on the building models on the gameobject list
                //if (z == 0 || z == 2)
                //{
                //    buildingHeight = Random.Range(0.01f, 0.02f);
                //}
                //else if(z == 1)
                //{
                //    buildingHeight = Random.Range(18, 30);
                //}

                buildingClone = Instantiate(buildingToCloneList[z]); //Based on the index, a specific building is cloned from the list of gameobjects

                buildingClone.transform.position = buildingToCloneList[0].transform.position; //Changes the position of the newly generated clone to the building on the first element of the list

                //buildingClone.transform.localScale = new Vector3(buildingToCloneList[z].transform.localScale.x, buildingHeight, buildingToCloneList[z].transform.localScale.z); //When activated, it will stretch the height of the newly generated building

                buildingClone.transform.localScale *= 2;

                buildingClone.transform.position = new Vector3(buildingToCloneList[0].transform.position.x + (i * 30), buildingToCloneList[z].transform.position.y, buildingToCloneList[0].transform.position.z + (j * 25)); //Change the building position inside the grid based on the original start position and the number of rows and columns avoiding overlapping
            }
        }
        
        //Deactivates the first element of the gameobject list to clone
        buildingToCloneList[0].SetActive(false);
    }
}
