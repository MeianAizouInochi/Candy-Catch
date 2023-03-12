using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /*
     * Exposed Play Function.
     */
    public void Play() 
    {
        //Destroys Current Scene and Loads The Candy-Catch Scene which contains the Main Game.
        SceneManager.LoadScene("Candy-Catch");
    }

    /*
     * Exposed Exit Function.
     */
    public void Exit() 
    {
        //Exiting Application.
        Application.Quit();
    }
}
