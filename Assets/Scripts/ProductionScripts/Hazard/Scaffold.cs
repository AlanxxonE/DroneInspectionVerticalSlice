using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : HazardMechanics
{
    //General Variables     
    private string hazardName = "Scaffold";
    
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
        Mechanics();
    }

    public void Mechanics()
    {
        float temp = 10 * Time.deltaTime; ///Change for the actual mechanics   
        RunHazard(hazardName, temp);       
    }
}
