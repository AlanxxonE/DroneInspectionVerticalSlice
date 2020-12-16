using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardEffect : MonoBehaviour
{
    /// <summary>
    /// The Class that manages the juggling between the hazards
    /// </summary>

    [SerializeField]
    private GameObject hazardEffectRef; //Reference to the particle system warning the player about fixing the hazard
    public Transform hazardEffectTarget; //Where the particle effect needs to spawn based on the hazard
    public GameObject particleClone; //The particle gameobject clone based on the reference
    public DroneUI satisfactionRef; //The satisfaction value reference from the UI
    public bool startEffect = false; //A boolean variable to check if the effect has started
    public bool endEffect = false; //A boolean variable to check if the effect has finished
    public float timeToGenerate = 0f; //A variable that determines how much time it needs to pass in order to generate a cloned particle

    // Start is called before the first frame update
    void Start()
    {
        particleClone = Instantiate(hazardEffectRef); //The method that generates a clone of the particle gameobject
        particleClone.transform.position = hazardEffectTarget.position; //Moves the new generated particle to the previously declared target position
        particleClone.transform.localScale *= 3; //Scales up the clone particle size

        /// <summary>
        /// A switch that makes the particle generates at different rates based on the danger level of the hazard itself
        /// </summary>     
        switch (GetComponent<HazardMechanics>().hazardDangerLevel)
        {
            case HazardMechanics.LevelsOfDangers.Green:
                timeToGenerate = 25f;
                break;
            case HazardMechanics.LevelsOfDangers.Amber:
                timeToGenerate = 15f;
                break;
            case HazardMechanics.LevelsOfDangers.Red:
                timeToGenerate = 5f;
                break;
        }

        StartCoroutine(GenerateEffect()); //Starts the coroutine that will play the particle effect
    }

    // Update is called once per frame
    void Update()
    {
        if (startEffect == true) //Checks if the cloned particles has started playing 
        {
            //Checks if the particle is still active and up and running, 
            //then if the player fixed/failed a minigame or too much time has elapsed the boolean variable confirms the end of the effect
            if (particleClone.activeSelf == true && particleClone.GetComponent<ParticleSystem>().isPlaying)
            {
                this.GetComponent<HazardMechanics>().checkEffect = true;
            }
            else if (!particleClone.GetComponent<ParticleSystem>().isPaused)
            {
                endEffect = true;
            }

            //Checks when the effect as terminated, deactivating the cloned particle, and making the hazard not targetable
            if (endEffect == true)
            {
                satisfactionRef.satisfactionValue -= 10f;

                particleClone.SetActive(false);
                this.GetComponent<HazardMechanics>().checkEffect = false;
                Destroy(this);
            }
        }
    }

    /// <summary>
    /// The Coroutine that makes the particle play while also cofnirming that the effect has started by chaning the boolean variable, after a specific amount of time
    /// </summary>
    IEnumerator GenerateEffect()
    {
        yield return new WaitForSeconds(timeToGenerate);

        particleClone.GetComponent<ParticleSystem>().Play();

        startEffect = true;
    }
}
