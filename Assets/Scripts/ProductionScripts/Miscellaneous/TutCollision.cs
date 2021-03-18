using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutCollision : MonoBehaviour
{
    public TutRings ring;

   void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ring")
        {
            ring.UpdateRingCount();
        }
    }
}
