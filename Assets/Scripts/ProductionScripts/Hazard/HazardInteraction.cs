using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardInteraction : MonoBehaviour
{ 
    //public Material materialToUse;
    //Material originalMaterial;
    public bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        //originalMaterial = this.GetComponent<MeshRenderer>().material;
    }

    private void OnMouseEnter()
    {
        //this.GetComponent<MeshRenderer>().material = materialToUse;
        this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    private void OnMouseDown()
    {
        isClicked = true;
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        //this.GetComponent<MeshRenderer>().material = originalMaterial;
    }
}
