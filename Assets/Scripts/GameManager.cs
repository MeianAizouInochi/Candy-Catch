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
    /*----------------------------------------------------------------------Private Variables--------------------------------------------------------------------------*/
    private int Score; //Score of the Player

    private int Lives; //Lives left of the Player

    private float ElapsedTime; //Elapsed Time the Player had stayed alive.

    private bool Game_Over = false; //Boolean to Allow and DisAllow Functionalities, once player dies.

    private bool IsPaused = false; //Bool to Allow Game to be Paused.

    private List<Vector2> Velocities; //Storage for the Candy Velocities.

    /*----------------------------------------------------------------------Public Variables--------------------------------------------------------------------------*/

    public TextMeshProUGUI ScoreText; //Takes the Reference to ScorePanel Score text for UI to show Score on screen

    public TextMeshProUGUI LivesText; //Takes the Reference to LivesPanel Lives Left text for UI to show Lives Left on screen

    public static GameManager gameManager; //Static object for referencing this class.

    public GameObject GameOverUI; //Reference to GameOver panel UI.

    public GameObject PauseMenuUI; //Reference to PauseMenuPanel UI.

    /*---------------------------------------------------------------------Functions-----------------------------------------------------------------------------------*/

    private void Awake() //First thing to do by this object when it gets instantiated.
    {
        gameManager = this; //Appliying static instance

        Score = 0; //Setting Score to 0.

        Lives = 10; //Setting Lives to 10.

        ScoreText.text = Score.ToString(); //Setting the Score to The Text Score Panel UI.

        LivesText.text = Lives.ToString(); //Setting the Lives to The Text Lives Panel UI.

        ElapsedTime = 0.0f; //Setting Elapsed TIme to 0.0f marking start of time.
    }


    private void Start() //Called Before the First Frame Update Occurs in Unity.
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
            ElapsedTime+= Time.deltaTime; //Adding to Variable elapsed time.

            print(ElapsedTime); //Debugging Purpose.

            yield return null; //to make it pass out control for the moment.
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

    /*
     * Game Over Functionality.
     * This Function has the Process which should occur after the player dies i.e. game is over.
     */
    private void GameOver()
    {
        Game_Over = true; //Setting the Game_Over to true, to stop any Score Incrementation.

        StopTimer(); //Stopping the Timer Coroutine.

        CandySpawner.candySpawner.StopSpawningCandy(); //Stopping the Candy Spawner Coroutine.

        GameObject.Find("Player").GetComponent<PlayerController>().StopSpeedUp(); //Stopping the SpeedUp Coroutine.

        GameObject.Find("Player").GetComponent<PlayerController>().StopSizeUp(); //Stopping the SizeUp Coroutine.

        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false; //Stopping the Movement of the Player.

        TextMeshProUGUI[] Texts = GameOverUI.GetComponentsInChildren<TextMeshProUGUI>(); //Getting the Required Objects where the Current Player data is stored.

        //Displaying the data in the Game Over Panel UI.
        foreach (TextMeshProUGUI Value in Texts)
        {
            if (Value.name.Equals("AliveTimeText"))
            {
                /*
                 * Finding the unity of time, for displaying in Human understandable manner.
                 */
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
        /*Setting the Time Scale which is shared among all scenes and is static,
         *i.e. wont get destroyed untill the application as a whole is closed.
         *it also doesnt resets or gets destroyed pr created even if a scene is unloaded or destroyed to load a different scene.
         */
        Time.timeScale = 1; 

        SceneManager.LoadScene("Candy-Catch"); //Loading a new Candy-catch scene, while destroying the previous one.
    }

    /*
     * Function for Entering the Main Menu.
     */
    public void GoToMainMenu() 
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene("MainMenu");
    }

    /*
     * Function to Save Score. work is still left here.
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
        PlayerGameData playergamedata = new PlayerGameData(Score, ElapsedTime); //Player data object is created.

        BinaryFormatter binaryFormatter= new BinaryFormatter(); //Binary Formatter object.

        DateTime dateTime = DateTime.Now; //Getting the current Date-Time.

        string Date_Time = dateTime.ToString("MM-dd-yyyy-hh-mm-ss-tt"); //Getting its string form.

        print(Date_Time); //Debugging purpose.

        string filename = "PlayerData" + Date_Time + ".dat"; //Creating the storage file name.

        //Storing the data after creating the above file.
        using (FileStream f = new FileStream(filename, FileMode.Create))
        {
            binaryFormatter.Serialize(f, playergamedata);
        }
    }

    /*
     * Function to Pause the Game.
     */
    public void PauseGame() 
    {
        //Checking if the game is already over, hence cannot be paused.
        if (!Game_Over)
        {
            if (!IsPaused) //Checking if it was previously paused.
            {
                IsPaused = true; //Changing the paused state to true.

                Time.timeScale = 0; //Stopping time from Increasing. Will remove the need to stop the Time Dependent Coroutines.

                CandySpawner.candySpawner.StopSpawningCandy(); //Stopping the Spawning of Candy.

                CandySpawner.candySpawner.InitialCandySpawnTime = 1.0f; //Changing the Initial Wait Time when Candy Spawner Starts again to 1.0f from 2.0f.

                GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false; //Stopping th Player Movement Capability.

                Velocities = new List<Vector2>(); //Storage for the Candy Velocities.

                foreach (GameObject Candy in CandySpawner.candySpawner.Candies) //Changing all Candies Velocity to 0,0
                {
                    Velocities.Add(Candy.GetComponent<Rigidbody2D>().velocity); //Storing the Candy Velocities.

                    Candy.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //changing the velocities to 0.
                }

                PauseMenuUI.SetActive(true); //Getting the PauseMenu Ui active.

            }
            else //Doing the opposite of the top section.
            {
                IsPaused = false;

                Time.timeScale = 1;

                int LooperVar = 0;

                foreach (GameObject Candy in CandySpawner.candySpawner.Candies)
                {
                    Candy.GetComponent<Rigidbody2D>().velocity = Velocities[LooperVar];

                    LooperVar++;
                }

                GameObject.Find("Player").GetComponent<PlayerController>().CanMove = true;

                CandySpawner.candySpawner.StartSpawningCandy();

                Velocities = null;

                CandySpawner.candySpawner.InitialCandySpawnTime = 2.0f;

                PauseMenuUI.SetActive(false);

            }
        }
        
        
    }
}
