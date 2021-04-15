using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Level manager is responsible for selecting the correct scene
    /// </summary>
    public static bool tutorialEnabled = true;
    private static float volume = 1;

    void Start()
    {
        Cursor.visible = true;   //Sets cursor visible
        Cursor.lockState = CursorLockMode.None;  //Locks cursor

        AudioListener.volume = volume;
        GameObject v = GameObject.FindGameObjectWithTag("VolumeSlider");
        if (v != null)
        {
            v.GetComponent<Slider>().value = volume;
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 0)  //If main level
        {
            tutorialEnabled = true;
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

    public void TutorialEnabled()
    {
        tutorialEnabled = !tutorialEnabled;
    }

    public void SetVolume(float temp)
    {
        volume = temp;
        AudioListener.volume = volume;
    }

    /// <summary>
    /// Method to quit the game
    /// </summary>
    public void QuitMethod()
    {
        Application.Quit();
    }
}
