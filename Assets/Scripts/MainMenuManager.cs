using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject mainMenu;

    public GameObject InfoPanel;

    public GameObject ScorePanel;

    private string PlayerDataFolderPath = "Player-Data-Folder";

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

    public void ExitScorePanel()
    {
        ScorePanel.SetActive(false);

        mainMenu.SetActive(true);
    }

    public void ShowScorePanel() 
    {
        ScorePanel.SetActive(true);

        mainMenu.SetActive(false);

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        DirectoryInfo directoryInfo = new DirectoryInfo(PlayerDataFolderPath+"/");

        try
        {
            if (directoryInfo.Exists)
            {
                FileInfo[] filesinfo = directoryInfo.GetFiles();

                if (filesinfo.Length == 1)
                {
                    PlayerGameData playerGameData;

                    using (FileStream filestream = filesinfo[0].Open(FileMode.Open, FileAccess.Read))
                    {
                        playerGameData = (PlayerGameData)binaryFormatter.Deserialize(filestream);
                    }

                    TextMeshProUGUI[] ScorePanelTexts = ScorePanel.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (TextMeshProUGUI TextComponent in ScorePanelTexts)
                    {
                        if (TextComponent.name.Equals("ScoreText"))
                        {
                            TextComponent.text = playerGameData.TotalScore.ToString();
                        }
                        else if (TextComponent.name.Equals("AliveTimeText"))
                        {
                            /*
                             * Finding the unity of time, for displaying in Human understandable manner.
                             */
                            string Unit = "SEC";

                            float ElapsedTime = playerGameData.TotalAliveTime;

                            if (ElapsedTime > 60.0f)
                            {
                                ElapsedTime /= 60.0f;

                                Unit = "MIN";

                                if (ElapsedTime > 60.0f)
                                {
                                    ElapsedTime /= 60.0f;

                                    Unit = "HOURS";
                                }
                            }
                            
                            TextComponent.text = Math.Round(ElapsedTime, 4).ToString() + " " + Unit;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            print(e.Message);

            print("Encountered Error in Best Score Panel");

        } 
    }
}
