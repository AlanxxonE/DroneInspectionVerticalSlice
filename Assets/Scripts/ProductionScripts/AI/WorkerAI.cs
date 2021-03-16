using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerAI : MonoBehaviour
{
	private AIManager AIManagerRef;

	private NavMeshAgent worker; //Nav Mesh Agent reference

	private Transform lastTarget;
	private Transform currentTarget;

	private bool timeToMove = false;

    private void Start()
    {
		AIManagerRef = GetComponentInParent<AIManager>();
		worker = GetComponent<NavMeshAgent>();
		currentTarget = AIManagerRef.targetLocations[0];
		StartCoroutine(TimeToWait());
	}

    private void Update()
	{
		//int randomNo = Random.Range(0, 1); //Instantiate random number generator

		if (Vector3.Distance(this.transform.position, currentTarget.transform.position) < 1 && timeToMove)
		{
			timeToMove = false;
		}

		if(!timeToMove)
        {
			Debug.Log("settarget");
			SetTarget();
			timeToMove = true;
		}
	}

	private void SetTarget()
    {
		lastTarget = currentTarget;

		int randomLocation = Random.Range(0, AIManagerRef.targetLocations.Count + 1); //generates a random reference to a member of the target locations list

        while (AIManagerRef.targetLocations[randomLocation] == lastTarget)
        {
            randomLocation = Random.Range(0, AIManagerRef.targetLocations.Count);
        }

        currentTarget = AIManagerRef.targetLocations[randomLocation];

		StartCoroutine(TimeToWait());
	}

	IEnumerator TimeToWait()
    {
		
		int randomTime = Random.Range(5, 10);

		yield return new WaitForSeconds(randomTime);

		worker.SetDestination(currentTarget.transform.position); //Creates path to target for agent to follow
	}
}
