using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [HideInInspector] public List<Transform> targetLocations; //Reference to list of target locations

    public GameObject workerPrefab; 

    // Start is called before the first frame update
    void Start()
    {
        int targets = this.transform.childCount;

        for (int numOfTargets = 1; numOfTargets <= targets; numOfTargets++)
        {
            targetLocations.Add(GetComponentsInChildren<Transform>()[numOfTargets].transform);
            CloneWorker(numOfTargets);
        }
    }

    private void CloneWorker(int index)
    {
        GameObject cloneWalker = Instantiate(workerPrefab);
        cloneWalker.transform.parent = this.transform;
        cloneWalker.transform.position = targetLocations[index - 1].position;
    }
}
