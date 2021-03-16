using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerAI : MonoBehaviour
{
	private AIManager AIManagerRef;
	private List<Transform> possibleTargets;

	private NavMeshAgent worker; //Nav Mesh Agent reference

	private Transform lastTarget;
	public Transform currentTarget;

	private bool timeToMove = false;

    private void Start()
    {
		AIManagerRef = GetComponentInParent<AIManager>();
		worker = GetComponent<NavMeshAgent>();
		possibleTargets = AIManagerRef.targetLocations;
		//StartCoroutine(TimeToWait());
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
			Debug.Log("settarget" + " " + this.gameObject.name);
			SetTarget();
			timeToMove = true;
		}
	}

	private void SetTarget()
    {
		lastTarget = currentTarget;

		//possibleTargets = AIManagerRef.targetLocations;
		//Debug.Log(possibleTargets.Count);
		possibleTargets.Remove(lastTarget);
		//Debug.Log(possibleTargets.Count);

		int randomLocation = Random.Range(0, possibleTargets.Count); //generates a random reference to a member of the target locations list

		//while (possibleTargets[randomLocation] == lastTarget)
  //      {
  //          randomLocation = Random.Range(0, possibleTargets.Count);
  //      }

        currentTarget = possibleTargets[randomLocation];

		possibleTargets.Add(lastTarget);

		StartCoroutine(TimeToWait());
	}

	IEnumerator TimeToWait()
    {
		
		int randomTime = Random.Range(5, 10);

		yield return new WaitForSeconds(randomTime);

		worker.SetDestination(currentTarget.transform.position); //Creates path to target for agent to follow
	}
}
