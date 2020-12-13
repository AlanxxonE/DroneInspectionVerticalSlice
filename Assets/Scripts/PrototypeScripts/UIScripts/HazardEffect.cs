﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardEffect : MonoBehaviour
{

    [SerializeField]
    private GameObject hazardEffectRef; //Reference to the particle system warning the player about fixing the hazard
    public Transform hazardEffectTarget; //Where the particle effect needs to spawn based on the hazard
    public GameObject particleClone;
    public DroneUI satisfactionRef;
    public bool endEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        particleClone = Instantiate(hazardEffectRef);
        particleClone.transform.position = hazardEffectTarget.position;
        particleClone.transform.localScale *= 3;
        particleClone.GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(particleClone.activeSelf == true && particleClone.GetComponent<ParticleSystem>().isPlaying)
        {
            this.GetComponent<HazardMechanics>().checkEffect = true;
        }
        else if(!particleClone.GetComponent<ParticleSystem>().isPaused)
        {
            endEffect = true;
        }

        if(endEffect == true)
        {
            satisfactionRef.satisfactionValue -= 10f;
            particleClone.SetActive(false);
            this.GetComponent<HazardMechanics>().checkEffect = false;
            Destroy(this);
        }
    }
}
