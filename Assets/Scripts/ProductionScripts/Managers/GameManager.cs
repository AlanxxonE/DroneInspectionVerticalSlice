using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Game manager class
    /// </summary>

    //Class References
    [Header("Class References")]
    public DroneController droneController; 
    public HazardManager hazardManager;
    public LevelManager levelManager;
    public DialogueManager dialogueManager;
    public UIManager UIManager;

    //General References
    [Header("General References")]
    [Tooltip("Reference for the pause menu popup")]
    public GameObject pauseUIRef;  

    //General Variables
    private bool isPaused = false;   //Boolean to determine pause state

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 2:
                    isPaused = !isPaused;  //Swaps the pause boolean           
                    Pause(isPaused);  //Calls the pause method
                    break;
                case 4:
                    isPaused = !isPaused;  //Swaps the pause boolean           
                    Pause(isPaused);  //Calls the pause method
                    break;
                default:
                    break;
            }
        }

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2);
    }

    /// <summary>
    /// Method used to pause/unpause the game
    /// </summary>
    /// <param name="isPaused"></param>
    void Pause(bool isPaused)
    {
        pauseUIRef.SetActive(isPaused); //Sets pause menu active/inactive based on the boolean        
        droneController.isPaused = isPaused;  //Sets the isPaused boolean in the drone controller script

        if (isPaused)
        {           
            Cursor.lockState = CursorLockMode.None;  
            Time.timeScale = 0;            
        }
        else if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
}
