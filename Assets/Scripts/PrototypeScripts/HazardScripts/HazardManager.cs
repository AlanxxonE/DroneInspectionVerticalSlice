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
        RaycastDistanceCheck();

        if (Input.GetKeyDown(KeyCode.Mouse0) && stopMovement == false && uIRef.color == Color.green)
        {
            ShootRaycast();
        }       
    }

    public void RaycastDistanceCheck()
    {
        Physics.Raycast(transform.position, transform.forward, out check, 100f);
        if (check.collider != null && check.collider.GetComponentInChildren<HazardMechanics>() != null)
        {
            if (check.distance > check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMin && check.distance < check.collider.GetComponentInChildren<HazardMechanics>().optimalDistanceMax)
            {
                uIRef.color = Color.green;
            }
            else if(check.distance < check.collider.GetComponentInChildren<HazardMechanics>().MaximumDistance)
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
        if (hit.collider != null && hit.collider.GetComponentInChildren<HazardMechanics>() != null)
        {
            if (uIRef.color == Color.green)
            {
                stopMovement = true;
                hazardRef = hit.collider.GetComponent<HazardMechanics>().hazardPopUpRef;
                hazardRef.SetActive(true);
                //hit.collider.GetComponentInChildren<HazardMechanics>().hazardTag = hazardRef.tag;
                hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", true);
            }
        }
    }

    public void FinishHazard(int satisfaction,int score, bool isFixed)
    {
        Debug.Log("TestFinish");
        hazardRef.GetComponent<Animator>().SetBool("ActiveHazard", false);
        stopMovement = false;
        droneUIScript.satisfactionValue += satisfaction; //////This value needs changed
        LevelManager.scoreValue += score;
        hazardRef.SetActive(false);

        if (isFixed)
        {
            switch (hazardRef.tag)
            {
                case "ScaffoldHazard":
                    LevelManager.isScaffoldFixed = true;
                    break;
                case "CraneHazard":
                    LevelManager.isCraneFixed = true;
                    break;
                default:
                    break;                    
            }
        }       
    }   
}
