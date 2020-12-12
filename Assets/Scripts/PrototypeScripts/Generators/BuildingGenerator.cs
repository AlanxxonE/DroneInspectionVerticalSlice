using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildingToCloneList;

    [SerializeField]
    private int numberOfBuildings = 0;

    private GameObject buildingClone;
    private float buildingHeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[,] buildingMultiArray = new GameObject[numberOfBuildings, numberOfBuildings];

        for (int i = 0; i < buildingMultiArray.GetLength(0); i++) 
        {
            for (int j = 0; j < buildingMultiArray.GetLength(1); j++) 
            {
                int z = Random.Range(0, buildingToCloneList.Count);
                if (z == 0)
                {
                    buildingHeight = Random.Range(0.01f, 0.02f);
                }
                else if(z == 1)
                {
                    buildingHeight = Random.Range(18, 30);
                }
                else if(z == 2)
                {
                    buildingHeight = Random.Range(2, 5);
                }

                buildingClone = Instantiate(buildingToCloneList[z]);
                buildingClone.transform.position = buildingToCloneList[0].transform.position;
                buildingClone.transform.localScale = new Vector3(buildingToCloneList[z].transform.localScale.x, buildingHeight, buildingToCloneList[z].transform.localScale.z);
                buildingClone.transform.position = new Vector3(buildingToCloneList[0].transform.position.x + (i * 25), buildingToCloneList[z].transform.position.y, buildingToCloneList[0].transform.position.z + (j * 20));
            }
        }
            
        buildingToCloneList[0].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
