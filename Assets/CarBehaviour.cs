using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private float carSpeed = 0;

    private Rigidbody rb;

    public enum Lane
    {
        Center,
        Left,
        Right
    }

    public Lane carLane;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(StartEngine());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * carSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndOfRoad")
        {
            transform.eulerAngles += new Vector3(0, 180, 0);
        }

        if (other.gameObject.tag == "CrossRoad" && carLane == Lane.Left)
        {
            transform.eulerAngles += new Vector3(0, 270, 0);
        }

        if (other.gameObject.tag == "CrossRoad" && carLane == Lane.Right)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
    }

    IEnumerator StartEngine()
    {
        float secondsToWait = Random.Range(1, 3f);

        yield return new WaitForSeconds(secondsToWait);

        int randomSpeed = Random.Range(2000, 3000);

        carSpeed = randomSpeed;
    }
}
