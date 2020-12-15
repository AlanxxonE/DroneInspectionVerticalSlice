﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Button References
    public Button startButtonRef;  //Reference to the start button
    public Button controlsButtonRef;  //Reference to the controls button
    public Button backButtonRef;  //Reference to the back button
    public Button escButtonRef;  //Reference to the escape button

    public GameObject PauseUIRef;   //Reference to the pause menu

    //Score Variables
    static public int scoreValue = 0;   //Sets score equal to zero initially
    public static bool isScaffoldFixed = false, isCraneFixed = false;    //Booleans to determine if hazards have been 

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (startButtonRef != null)
        {
            startButtonRef.onClick.AddListener(StartButtonMethod);
        }

        if (controlsButtonRef != null)
        {
            controlsButtonRef.onClick.AddListener(ControlsButtonMethod);
        }

        if (backButtonRef != null)
        {
            backButtonRef.onClick.AddListener(BackButtonMethod);
        }

        if (escButtonRef != null)
        {
            escButtonRef.onClick.AddListener(EscButtonMethod);
        }

        if (PauseUIRef != null && PauseUIRef.activeSelf == true)
        {
            PauseUIRef.GetComponent<Image>().enabled = false;

            PauseUIRef.GetComponentsInChildren<Image>()[1].enabled = false;
            PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = false;
            
            PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = false;
            PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = false;
            PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = false;            
            
            PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = false;
            PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = false;       
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Input.GetButtonDown("Pause"))
            {
                if(PauseUIRef != null && PauseUIRef.GetComponent<Image>().enabled == false)
                {
                    Cursor.lockState = CursorLockMode.None;

                    PauseUIRef.GetComponentsInChildren<Image>()[1].enabled = true;
                    PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = true;

                    PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = true;
                    PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = true;
                    PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = true;                   

                    PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = true;
                    PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = true;

                    PauseUIRef.GetComponent<Image>().enabled = true;
                    PauseUIRef.GetComponent<PauseBehaviour>().activePause = true;
                    Time.timeScale = 0;
                }
                else
                {
                    PauseUIRef.GetComponentsInChildren<Image>()[1].enabled = false;
                    PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = false;

                    PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = false;
                    PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = false;
                    PauseUIRef.GetComponentsInChildren<Button>()[0].enabled = false;                   

                    PauseUIRef.GetComponentsInChildren<Image>()[2].enabled = false;
                    PauseUIRef.GetComponentsInChildren<Button>()[1].enabled = false;

                    Time.timeScale = 1;
                    PauseUIRef.GetComponent<PauseBehaviour>().activePause = false;
                    PauseUIRef.GetComponent<Image>().enabled = false;

                    Cursor.lockState = CursorLockMode.Locked;
                }               
            }
        }
        else
        {            
            Time.timeScale = 1;
        }
    }

    public void StartButtonMethod()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        scoreValue = 0;
    }

    public void ControlsButtonMethod()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void BackButtonMethod()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void SceneSelectMethod(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }

    public void EscButtonMethod()
    {
        Application.Quit();
    }
}
