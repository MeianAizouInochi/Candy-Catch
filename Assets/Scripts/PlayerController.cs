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
    float MoveSpeed;

    [SerializeField]
    float maxPos;

    readonly bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            Move();
        }

    }

    private void Move()
    {
        float InputX = Input.GetAxis("Horizontal");

        /*
         * Time.deltaTime is the amount of time passed from the frame in concern untill now.
         * Vector3.right means a vector whose value is 1,0,0
         * 
         * transform.position updates a new position to our player.
         */
        transform.position += InputX * MoveSpeed * Time.deltaTime * Vector3.right;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxPos, maxPos), transform.position.y, transform.position.z);
            

    }
}
