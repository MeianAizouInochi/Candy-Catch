using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Defining the OnTriggerEnter2D function to Provide functionality when AThe Candy Collided with the player (Both Player and Candy are RigidBody)
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If Collision is Made, then destroying the Candy Game Object, and Increasing the Score.
        if (collision.gameObject.tag.Equals("Player"))
        {
            /*
             * Increse Score.
             * Destroy Object.
             */
            GameManager.gameManager.IncrementScore();

            Destroy(gameObject);

        }
        else 
        {
            //Do nothing here
        }
    }
}
