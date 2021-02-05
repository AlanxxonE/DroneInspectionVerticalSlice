using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementOld : MonoBehaviour
{
    protected Vector3 movementHorizontalVelocity;
    protected Vector3 movementVerticalVelocity;
    protected Vector3 originalVelocity;

    [SerializeField]
    protected int movementSpeed = 2;
    [SerializeField]
    protected float smoothVelocity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            MovementInput();
        }

        if (GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(originalVelocity, Vector3.zero, ref originalVelocity, smoothVelocity);
        }
    }

    void MovementInput()
    {

        if (Input.GetButton("Ascend"))
        {
            movementVerticalVelocity = new Vector3(0, Input.GetAxisRaw("Ascend"), 0) * movementSpeed;
            originalVelocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(originalVelocity, movementVerticalVelocity, ref originalVelocity, smoothVelocity);
        }
        else
        {
            movementHorizontalVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * movementSpeed;
            originalVelocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(originalVelocity, movementHorizontalVelocity, ref originalVelocity, smoothVelocity);
        }
    }
}
