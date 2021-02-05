using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Game manager class
    /// </summary>
    
    //References
    public GameObject pauseUIRef;   //Reference to the pause menu
    public DroneController droneController; //Reference to the drone controller script
    public HazardManager hazardManager;
    private bool isPaused = false;   //Boolean to determine pause state

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
        {          
            if (Input.GetButtonDown("Pause")) 
            {               
                isPaused = !isPaused;  //Swaps the pause boolean           
                Pause(isPaused);  //Calls the pause method
            }
        }
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
