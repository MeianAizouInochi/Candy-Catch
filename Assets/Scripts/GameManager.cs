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

    private string PlayerDataFolderPath = "Player-Data-Folder";

    private string PLayerDataFilePathName = "Player-Data";

    private bool canSave = true;

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

            //print(ElapsedTime); //Debugging Purpose.

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

                float Elapsed_Time = ElapsedTime;

                if (Elapsed_Time > 60.0f)
                {
                    Elapsed_Time /= 60.0f;

                    Unit = "MIN";

                    if (Elapsed_Time > 60.0f)
                    {
                        Elapsed_Time /= 60.0f;

                        Unit = "HOURS";
                    }
                }

                Value.text = Math.Round(Elapsed_Time,4).ToString() + " " + Unit;
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
        if (canSave) // If canSave is True, only then we proceeding, hence prohibiting spamming.
        {
            if (ManageFileFolder())
            {
                //Change Save Icon.
                print("Saved!");
            }
            else 
            {
                print("Error Occured!");
            }
        }
    }


    /*
     * Function for Managing The File and Folder of Player Data.
     */
    private bool ManageFileFolder() 
    {
        bool result = true; //For Checking if all operations were done successfully.

        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PlayerDataFolderPath + "/"); //Getting the path to the folder.

            BinaryFormatter binaryFormatter = new BinaryFormatter(); //Binary Formatter for data serialization and deserialization.

            //Checking Existence of the folder.

            if (directoryInfo.Exists) //if exists
            {
                FileInfo[] filesInfo = directoryInfo.GetFiles(); //Getting the files inside it.

                if (filesInfo.Length == 1) //Checking if the Number of files equals to 1.
                {
                    //Previous save is already there.

                    bool shouldUpdate = false; //For Checking if the file should be updated.

                    //Load from previous save.
                    using (FileStream filestream = filesInfo[0].Open(FileMode.Open, FileAccess.Read))
                    {
                        PlayerGameData playergamedata = (PlayerGameData)binaryFormatter.Deserialize(filestream); //deserialize the data from the load.

                        //store the cummulative datas.
                        float CummulativeFileData = playergamedata.TotalScore + playergamedata.TotalAliveTime;

                        float CummulativeCurrentData = Score + ElapsedTime;

                        //Compare the cummulative data.
                        if (CummulativeCurrentData > CummulativeFileData)
                        {
                            shouldUpdate = true; // update the shouldUpdate to true, since the new Run/Play was better than before, hence require Update.
                        }
                    }

                    //Checking the shouldUpdate Variable.
                    if (shouldUpdate) // if true
                    {
                        filesInfo[0].Delete(); //delete the previous saved file.

                        PlayerGameData playergamedata = new PlayerGameData(Score, ElapsedTime); // create the new object for serialization.

                        DateTime dateTime= DateTime.Now;

                        string date_time = dateTime.ToString("MM-dd-yy-hh-mm-ss-tt");

                        //store it by serialization..
                        using (FileStream filestream = new FileStream(PlayerDataFolderPath + "/" + PLayerDataFilePathName + date_time, FileMode.OpenOrCreate))
                        {
                            binaryFormatter.Serialize(filestream, playergamedata);
                        }

                        print("Redid the previous save."); //debugging purpose.
                    }

                    //set the result to true, since either of the above otcomes is s a success.
                    result = true;
                }
                else //if Number of files inside the folder is less than 1, i.e. 0.
                {
                    PlayerGameData playergamedata = new PlayerGameData(Score, ElapsedTime); // create the object for serialization.

                    DateTime dateTime = DateTime.Now;

                    string date_time = dateTime.ToString("MM-dd-yy-hh-mm-ss-tt");

                    //store the data using serialization.
                    using (FileStream filestream = new FileStream(PlayerDataFolderPath + "/" + PLayerDataFilePathName + date_time, FileMode.OpenOrCreate))
                    {
                        binaryFormatter.Serialize(filestream, playergamedata);
                    }

                    print("First Save");

                    //set the result to true, if the above succeeds.
                    result = true;
                }
            }
            else // If the folder was not there.
            {
                directoryInfo.Create(); //Create the Folder.

                PlayerGameData playergamedata = new PlayerGameData(Score, ElapsedTime); //create object for serialization.

                DateTime dateTime = DateTime.Now;

                string date_time = dateTime.ToString("MM-dd-yy-hh-mm-ss-tt");

                //store the object by serialization.
                using (FileStream filestream = new FileStream(PlayerDataFolderPath + "/" + PLayerDataFilePathName + date_time, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(filestream, playergamedata);
                }

                print("First Save and Created a folder");

                //set the result to true if the above executed properly.
                result = true;
            }
        }
        catch (Exception e) // If any exception Occurs above execute the following block.
        {
            print(e.Message);
            
            //set the result to false, for letting the player retry.
            result = false;
        }

        //update the value of canSave, to restrict the player from saving if its already successfull once.
        canSave = result ? false : canSave; //Update canSave depending on result.

        //returning result of the function.
        return result;
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
