using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixScript : MonoBehaviour
{
    [SerializeField]
    protected List<Transform> helixChildren;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < 5; i++)
        {
            helixChildren.Add(GetComponentsInChildren<Transform>()[i]);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            helixChildren[0].transform.Rotate(0, 10, 0);
            helixChildren[1].transform.Rotate(0, 10, 0);
            helixChildren[2].transform.Rotate(0, 10, 0);
            helixChildren[3].transform.Rotate(0, 10, 0);
        }
    }
}
