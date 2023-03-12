using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;


    int Score = 0;

    //bool GameOver = false;

    public TextMeshProUGUI ScoreText;

    private void Awake()
    {
        gameManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementScore() 
    {
        Score++;

        ScoreText.text = Score.ToString();

        print(Score);
    }

}
