using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenu;

    public GameObject InfoPanel;

    public void StartPlayingGame() 
    {
        SceneManager.LoadScene("Candy-Catch");
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void ShowInfo() 
    {
        mainMenu.SetActive(false);

        InfoPanel.SetActive(true);
    }

    public void ExitInfo() 
    {
        InfoPanel.SetActive(false);

        mainMenu.SetActive(true);
    }
}
