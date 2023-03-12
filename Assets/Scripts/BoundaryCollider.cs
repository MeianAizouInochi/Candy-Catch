using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollider : MonoBehaviour
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
     * Defining OnTriggerEnter2D function for Boundary Collider.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If The Candy Collided with Boundary, means Player Missed the Candy.
        if (collision.gameObject.tag.Equals("Candy"))
        {
            /*
             * Destroy Candy.
             * Decrease Lives of Player.
             */
            GameManager.gameManager.DecreaseLives();

            Destroy(collision.gameObject);
        }
        else 
        {
            //do nothing.
            //Need Error Here.
        }
    }
}
