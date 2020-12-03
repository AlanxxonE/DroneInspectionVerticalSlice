using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HazardManager : MonoBehaviour
{

    //References 
    public DroneUI droneUIScript;

    //Hazard Interaction Vars
    public RaycastHit hit;
    RaycastHit check;
    public Image uIRef;
    public bool stopMovement = false;
    public GameObject hazardRef;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && uIRef.color == Color.green)
        {
            ShootRaycast();
        }

        if (stopMovement == true)
        {
            FinishHazard();
        }
    }

    public void RaycastCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out check, 100f);
        if (check.collider != null && check.collider.GetComponentInChildren<HazardTestScript>() != null)
        {
            if (check.distance > check.collider.GetComponentInChildren<HazardTestScript>().optimalDistanceMin && check.distance < check.collider.GetComponentInChildren<HazardTestScript>().optimalDistanceMax)
            {
                uIRef.color = Color.green;
            }
            else if(check.distance < check.collider.GetComponentInChildren<HazardTestScript>().MaximumDistance)
            {
                uIRef.color = Color.red;
            }
        }
        else if (check.collider == null)
        {
            uIRef.color = Color.gray;
        }
    }

    public void ShootRaycast()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 100f);
        if (hit.collider != null && hit.collider.GetComponentInChildren<HazardTestScript>() != null)
        {
            if (uIRef.color == Color.green)
            {
                stopMovement = true;
                Debug.Log(hit.collider);
                hazardRef = hit.collider.GetComponent<HazardTestScript>().HazardPopUpRef;
                hazardRef.SetActive(true);
                hit.collider.GetComponentInChildren<HazardTestScript>().hazardTag = hazardRef.tag;
                Debug.Log(hit.collider.GetComponentInChildren<HazardTestScript>().hazardTag);
                hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", true);
            }
        }
    }

    public void FinishHazard()
    {
        if (hazardRef.GetComponentInChildren<Slider>().value >= 100)
        {
            hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);
            stopMovement = false;
            droneUIScript.satisfactionValue += 40f; //////This value needs changed
            LevelManager.scoreValue += 10;
        }
        else if (hazardRef.GetComponentInChildren<Slider>().value != 0)
        {
            hazardRef.GetComponentInChildren<Slider>().value -= Time.deltaTime * 5f;
        }
        else
        {
            hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);
            droneUIScript.satisfactionValue -= 40f;
            stopMovement = false;
        }
    }

    
}
