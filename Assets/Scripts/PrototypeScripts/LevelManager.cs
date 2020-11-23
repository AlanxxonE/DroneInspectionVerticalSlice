using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button startButtonRef;

    public Button controlsButtonRef;

    public Button backButtonRef;

    public Button escButtonRef;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }
    }

    public void StartButtonMethod()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void ControlsButtonMethod()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void BackButtonMethod()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void EscButtonMethod()
    {
        Application.Quit();
    }
}
