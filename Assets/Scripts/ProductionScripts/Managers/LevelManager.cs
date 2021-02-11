using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;   //Sets cursor visible
        Cursor.lockState = CursorLockMode.None;  //Locks cursor

        if (SceneManager.GetActiveScene().buildIndex == 2)  //If main level
        {
            ////Sets these variables when the level is loaded
            //scoreValue = 0;   
            //isScaffoldFixed = false;
            //isCraneFixed = false;
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
