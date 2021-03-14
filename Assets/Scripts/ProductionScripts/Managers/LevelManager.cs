using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{    
    /// <summary>
    /// Level manager is responsible for selecting the correct scene
    /// </summary>
    
    void Start()
    {
        Cursor.visible = true;   //Sets cursor visible
        Cursor.lockState = CursorLockMode.None;  //Locks cursor

        if (SceneManager.GetActiveScene().buildIndex == 0)  //If main level
        {
            Score.SetFixedBooleans(null,false,true);            
        }
    }

    /// <summary>
    /// Method used to select scenes based on inputted int i
    /// </summary>
    /// <param name="i"></param>
    public void SceneSelectMethod(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Single);  //Loads scene from build index 
        Time.timeScale = 1;  //Sets time scale equal to 1 when a level is loaded 
    }

    /// <summary>
    /// Method to quit the game
    /// </summary>
    public void QuitMethod()
    {
        Application.Quit();
    }
}
