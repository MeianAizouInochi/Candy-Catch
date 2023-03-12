using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     * This Exposes the private field to the inspector.
     */
    [SerializeField]
    float MoveSpeed; //MoveSpeed of Player

    [SerializeField]
    float maxPos; //Max Position the player can move in Both Directions in X- axis.

    /*
     * Restricting Movement of player by this Variable value.
     * Exposed it to other classes for changing it.
     */
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Checking the state of the Movement condition variable.
        if (canMove)
        {
            Move();
        }

    }

    /*
     * Move Function to Move player.
     */
    private void Move()
    {
        /*
         * Gets the Input from KeyBoard Arrow Keys for Horizontal Axis.
         */
        float InputX = Input.GetAxis("Horizontal");

        /*
         * Time.deltaTime is the amount of time passed from the frame in concern untill now.
         * Vector3.right means a vector whose value is 1,0,0
         * 
         * transform.position updates a new position to our player.
         */
        transform.position += InputX * MoveSpeed * Time.deltaTime * Vector3.right;

        //Clamping the Position to restrict the user from going out of bounds.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxPos, maxPos), transform.position.y, transform.position.z);
            

    }
}
