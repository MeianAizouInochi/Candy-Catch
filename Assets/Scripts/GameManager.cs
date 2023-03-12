using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
     * Creating a static Instance for reference from outside the class.
     */
    public static GameManager gameManager;

    //Private Variables
    int Score = 0; //Game Player Score

    bool gameOver = false; //Game State.

    int Lives = 10; //Lives Left, or Starting with.

    /*
     * Public Objects, for Initialising from Unity Editor.
     */
    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI LivesText;

    public GameObject GameOverUI;

    private void Awake()
    {
        gameManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LivesText.text = Lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //nothing here
    }

    /*
     * Exposed function to Increase Score.
     */
    public void IncrementScore() 
    {
        //Checking Game State Before Increment.
        if (!gameOver)
        {
            Score++;

            ScoreText.text = Score.ToString();
        }
        
    }

    /*
     * Exposed function to decrease Lives of Player
     */
    public void DecreaseLives() 
    {
        //Checking Current Lives Count.
        if (Lives > 0)
        {
            Lives--; //Decrementing Lives.

            //Changing the text of the LivesText Object that is Attached from the ScorePanel on canvas.
            LivesText.text = Lives.ToString();

            //debugging Statement.
            //print(Lives);
        }
        else 
        {
            //This section is Called if Lives were already 0.
            gameOver = true;

            GameOver(); //Calling GameOver Function.
        }
    }

    /*
     * Exposed Game Over Function
     */
    public void GameOver() 
    {
        /*
         * Using the static instance of Candy Generator to Call StopSpawningCandies function.
         */
        CandyGenerator.candyGenerator.StopSpawningCandies();

        /*
         * Changing the Ability to move the player to false.
         */
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = false;

        /*
         * Activating the Game Over UI.
         */
        GameOverUI.SetActive(true);
    }

    /*
     * Exposed Restart Game Function
     */
    public void RestartGame() 
    {
        SceneManager.LoadScene("Candy-Catch");
    }

    /*
     * Exposed Go To Menu Function.
     */
    public void GoToMenu() 
    {
        SceneManager.LoadScene("Menu");
    }
}
