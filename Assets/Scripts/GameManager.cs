using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int Score; //Score of the Player

    private int Lives; //Lives left of the Player

    private float ElapsedTime; //Elapsed Time the Player had stayed alive.

    private bool Game_Over = false; //Boolean to Allow and DisAllow Functionalities, once player dies.

    public TextMeshProUGUI ScoreText; //Takes the Reference to ScorePanel Score text for UI to show Score on screen

    public TextMeshProUGUI LivesText; //Takes the Reference to LivesPanel Lives Left text for UI to show Lives Left on screen

    public static GameManager gameManager; //Static object for referencing this class.

    public GameObject GameOverUI; //Reference to GameOver panel UI.


    private void Awake()
    {
        gameManager = this;

        Score = 0;

        Lives = 10;

        ScoreText.text = Score.ToString();

        LivesText.text = Lives.ToString();

        ElapsedTime = 0.0f;
    }


    private void Start()
    {
        StartTimer(); //Starting the Timer Coroutine.
    }

    /*
     * Coroutine to Continously keep track of Elapsed time.
     */
    IEnumerator GameTime() 
    {
        while (true) 
        {
            ElapsedTime+= Time.deltaTime;

            yield return null;
        }
    }

    /*
     * Function to start the coroutine above.
     */
    public void StartTimer() 
    {
        StartCoroutine("GameTime");
    }

    /*
     * Function to stop the coroutine above.
     */
    public void StopTimer() 
    {
        StopCoroutine("GameTime");
    }

    /*
     * Function to increment the Score of the Player.
     */
    public void ScoreIncrementor() 
    {
        if (!Game_Over) //checking the game state.
        {
            Score++;

            ScoreText.text = Score.ToString();

            if (Score % 5 == 0) //The Player gains 1 Life when Score reaches a multiple of 5.
            {
                Lives++;

                LivesText.text = Lives.ToString();

                if (Score - 20 == 0 || Score - 50 == 0)
                {
                    if (Score < 50)
                    {
                        CandySpawner.candySpawner.MaxExposedCandyIndex = 6; //Unlocking the Speed Candy
                    }
                    else 
                    {
                        CandySpawner.candySpawner.MaxExposedCandyIndex = 7; //Unlocking the Size Candy
                    }
                }
            }
        }   
    }

    /*
     * Function to decrement lives of the player
     */
    public void LifeDecrementor() 
    {
        if (Lives > 0)
        {
            Lives--;

            LivesText.text = Lives.ToString();
        }
        else 
        {
            GameOver(); //Called Game Over if the Player lost 1 more candy while the lives are at 0.
        }
    }

    private void GameOver()
    {
        Game_Over = true; //Setting the Game_Over to true, to stop any Score Incrementation.

        StopTimer(); //Stopping the Timer Coroutine.

        CandySpawner.candySpawner.StopSpawningCandy(); //Stopping the Candy Spawner Coroutine.

        GameObject.Find("Player").GetComponent<PlayerController>().StopSpeedUp(); //Stopping the SpeedUp Coroutine.

        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false; //Stopping the Movement of the Player.

        TextMeshProUGUI[] Texts = GameOverUI.GetComponentsInChildren<TextMeshProUGUI>(); //Getting the Required Objects where the Current Player data is stored.




        //Displaying the data in the Game Over Panel UI.
        foreach (TextMeshProUGUI Value in Texts)
        {
            if (Value.name.Equals("AliveTimeText"))
            {
                string Unit = "SEC";

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

                Value.text = Math.Round(ElapsedTime,4).ToString() + " " + Unit;
            }
            else if(Value.name.Equals("TotalScoreText"))
            {
                Value.text = Score.ToString() ;
            }
        }

        GameOverUI.SetActive(true); //Setting the Ui active for visibility.
    }

    /*
     * Function For Restarting the Game again.
     */
    public void RestartGame() 
    {
        SceneManager.LoadScene("Candy-Catch");
    }

    /*
     * Function for Entering the Main Menu.
     */
    public void GoToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    /*
     * Function to Save Score.
     */
    public void SaveScore() 
    {
        /*
         * Need to create a Folder PlayerData storage
         * Need to check if 3 Files with the Prefix PlayerData Exists or Not,
         * If Exists, then ask user to delete one, then try saving.
         * Else Just save and Change Sprite of the Button to Green
         * 
         * Provide a Option in Main Menu to Interact with the Saved Player Data.
         */
        PlayerGameData playergamedata = new PlayerGameData(Score, ElapsedTime);

        BinaryFormatter binaryFormatter= new BinaryFormatter();

        DateTime dateTime = DateTime.Now;

        string Date_Time = dateTime.ToString("MM-dd-yyyy-hh-mm-ss-tt");

        print(Date_Time);

        string filename = "PlayerData" + Date_Time + ".dat";

        using (FileStream f = new FileStream(filename, FileMode.Create))
        {
            binaryFormatter.Serialize(f, playergamedata);
        }
    }
}
