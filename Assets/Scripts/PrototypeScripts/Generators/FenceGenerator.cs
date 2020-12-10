using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGenerator : MonoBehaviour
{
    private enum Directions
    {
        Vertical,
        Horizontal
    };

    [SerializeField]
    private Directions typeOfDirection;

    [SerializeField]
    private GameObject childFence;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 50; i++)
        {

            GameObject childFenceClone = Instantiate(childFence);

            if (typeOfDirection == Directions.Vertical)
            {
                childFenceClone.transform.position = new Vector3(childFence.transform.position.x, childFence.transform.position.y, childFence.transform.position.z + (4 * i));
            }
            else if (typeOfDirection == Directions.Horizontal && i < 49)
            {
                childFenceClone.transform.localEulerAngles = new Vector3(0, 90, 0);
                childFenceClone.transform.position = new Vector3(childFence.transform.position.x + (4 * i), childFence.transform.position.y, childFence.transform.position.z);
            }
        }
    }
}
