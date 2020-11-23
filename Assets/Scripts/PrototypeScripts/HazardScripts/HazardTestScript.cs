using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardTestScript : MonoBehaviour
{
    public GameObject HazardPopUpRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //When the raycast hit the object, the following code gets executed
    private void OnEnable()
    {
        HazardPopUpRef.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
