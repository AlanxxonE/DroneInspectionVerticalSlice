using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerAI : MonoBehaviour
{
	private AIManager AIManagerRef;
	private List<Transform> possibleTargets;
	private Animator animatorRef;

	[HideInInspector] public NavMeshAgent worker; //Nav Mesh Agent reference

	[HideInInspector] public float originalSpeed;

	private Transform lastTarget;
	[HideInInspector] public Transform currentTarget;

	private bool timeToMove = false;

    private void Start()
    {
		AIManagerRef = GetComponentInParent<AIManager>();
		worker = GetComponent<NavMeshAgent>();
		animatorRef = GetComponent<Animator>();
		possibleTargets = AIManagerRef.targetLocations;
		originalSpeed = worker.speed;
	}

    private void Update()
	{
		if (Vector3.Distance(this.transform.position, currentTarget.transform.position) < 1 && timeToMove)
		{
			timeToMove = false;
		}

		if(!timeToMove)
        {
			SetTarget();
			timeToMove = true;
		}
	}

	private void SetTarget()
    {
		lastTarget = currentTarget;

		possibleTargets.Remove(lastTarget);

		int randomLocation = Random.Range(0, possibleTargets.Count); //generates a random reference to a member of the target locations list

        currentTarget = possibleTargets[randomLocation];

		possibleTargets.Add(lastTarget);

		StartCoroutine(TimeToWait());
	}

	IEnumerator TimeToWait()
    {
		animatorRef.SetBool("Walking", false);

		int randomTime = Random.Range(5, 10);

		yield return new WaitForSeconds(randomTime);

		animatorRef.SetBool("Walking", true);

		worker.SetDestination(currentTarget.transform.position); //Creates path to target for agent to follow
	}
}
