using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutRings : MonoBehaviour
{
    public GameObject[] rings;
    int ringCount = 0;
    bool ringsDone = false;

    private void Start()
    {
        rings = GameObject.FindGameObjectsWithTag("Ring");
        foreach(GameObject ring in rings)
        {
            ring.SetActive(false);
        }
    }
    void Awake()
    {
        rings[0].SetActive(true);
    }

    public void UpdateRingCount()
    {
        ringCount++;
        rings[ringCount - 1].SetActive(false);
        if (ringCount < rings.Length)
        {
            rings[ringCount].SetActive(true);
        }
        else
        {
            ringsDone = true;
        }
    }

    public bool GetRingsDone()
    {
        return ringsDone;
    }
}
