using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : MonoBehaviour
{
    //Class Reference
    public HazardManager hazardManager;

    //General Variables 
    private int satisfaction = 40;
    private int dissatisfaction = 20;
    private int score = 50;
    [HideInInspector]public bool isFixed = false;

    private void Awake()
    {
        this.GetComponent<MonoBehaviour>().enabled = false;        
    }

    private void OnEnable()
    {
        Debug.Log("It works!!!!");
    }

    private void Update()
    {
        //Debug.Log("It is running!!!!");
        Mechanics();
    }

    public void Mechanics()
    {
        hazardManager.HazardProgress(satisfaction, dissatisfaction, score);
        hazardManager.hazardSlider.value += 10 * Time.deltaTime; ///Change for the actual mechanics
    }
}
