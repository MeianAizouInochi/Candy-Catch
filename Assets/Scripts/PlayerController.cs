using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     * PLayer Movement Speed variable
     */
    private float PlayerMoveSpeed;

    /*
     * Max Positional boundaries in x and y axis.
     */
    [SerializeField]
    private float MaxXPos;

    [SerializeField]
    private float MaxYPos;


    /*
     * To Allow and DisAllow Movement.
     */
    public bool CanMove = true;



    /*
     * After Object is Instantiated, Awake is called once throughout its lifecycle.
     */
    private void Awake()
    {
        PlayerMoveSpeed = 20;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Change the Value of Serialized field here before deployment.
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Move();

        }
        
    }


    /*
     * The Movement Function for the Player.
     * Currently the player can move faster, if they move diagonally.
     */
    private void Move() 
    {
        float HorizontalX = Input.GetAxis("Horizontal"); //Taking the Horizontal Input A, D, and Right and left Arrwo
        
        float VerticalY = Input.GetAxis("Vertical"); //Taking Vertical/Front and Back input W, S and Front And Back Arrow.

        //Creating the Vector with the values along with the MoveSpeed and deltaTime.
        Vector3 Value = PlayerMoveSpeed * Time.deltaTime * new Vector3(HorizontalX, VerticalY,0);

        transform.position += Value; //Changing the Position of the Player.

        //Clamp the Position of the Player to restrict OutOfBounds Condition.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -MaxXPos, MaxXPos), Mathf.Clamp(transform.position.y, -MaxYPos, MaxYPos), 0);
    }

    /*
     * If the Object this script is attached to (i.e. Player), collides with another Object (i.e. Candy), THis Function is called.
     */
    private void OnTriggerEnter2D(Collider2D collision) //The particular collision that called it, is passed as parameter to this function.
    {
        //The case to be called depends on what tag of the Candy player collided with.
        switch (collision.gameObject.tag)  
        {
            case "NormalCandy":
                {
                    GameManager.gameManager.ScoreIncrementor();

                    Destroy(collision.gameObject);

                    break;
                }

            case "BombCandy":
                {
                    GameManager.gameManager.LifeDecrementor();
                    
                    Destroy(collision.gameObject);

                    break;
                }
            case "SpeedCandy":
                {
                    StopSpeedUp(); //Stop the coroutine if its already running.

                    StartSpeedUp(); //Start it again.

                    Destroy(collision.gameObject);

                    break;
                }
            case "SizeCandy":
                {
                    //Increase Size of Players collider.
                    GameManager.gameManager.ScoreIncrementor(); //temporary calling.

                    Destroy(collision.gameObject);

                    break;
                }
        }
    }

    /*
     * Coroutine for SpeedUp feature.
     */
    IEnumerator StartTimeForSpeedUp() 
    {
        PlayerMoveSpeed *= 2f; //Increasing Player Movement Speed by 2 times.

        float TimePassed = 0.0f;

        while (TimePassed < 10.0f) //the increment stays for 10 seconds.
        {
            TimePassed+= Time.deltaTime;

            yield return null;
        }

        PlayerMoveSpeed /= 2f;
    }

    /*
     * Function to start the above Coroutine.
     */
    public void StartSpeedUp() 
    {
        StartCoroutine("StartTimeForSpeedUp");
    }

    /*
     * Function to stop the above coroutine.
     */
    public void StopSpeedUp() 
    {
        StopCoroutine("StartTimeForSpeedUp");

        if (PlayerMoveSpeed > 20.0f)
        {
            PlayerMoveSpeed = 20.0f;
        }
    }
}
