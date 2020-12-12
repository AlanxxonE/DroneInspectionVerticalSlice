using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardEffect : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem hazardEffectRef; //Reference to the particle system warning the player about fixing the hazard
    public Transform hazardEffectTarget; //Where the particle effect needs to spawn based on the hazard
    public ParticleSystem particleClone;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<HazardMechanics>().checkEffect = true;
        particleClone = Instantiate(hazardEffectRef);
        particleClone.transform.position = hazardEffectTarget.position;
        particleClone.transform.localScale *= 3;
        particleClone.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(particleClone.isPlaying)
        {
            GetComponent<HazardMechanics>().enabled = true;
        }
        else
        {
            GetComponent<HazardMechanics>().enabled = false;
        }

        if(GetComponent<HazardMechanics>().checkEffect == false)
        {
            particleClone.gameObject.SetActive(false);
            GetComponent<HazardMechanics>().enabled = false;
        }
    }
}
