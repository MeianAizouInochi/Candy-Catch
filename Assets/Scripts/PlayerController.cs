using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float PlayerMoveSpeed;

    [SerializeField]
    float MaxXPos;

    [SerializeField]
    float MaxYPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //Change the Value of Serialized field here before deployment.
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        
    }

    /*
     * The Movement Function for the Player.
     */
    private void Move() 
    {
        //Taking the Horizontal Input A, D, and Right and left Arrwo
        float HorizontalX = Input.GetAxis("Horizontal");

        //Taking Vertical/Front and Back input W, S and Front And Back Arrow.
        float VerticalY = Input.GetAxis("Vertical");

        //Creating the Vector with the values along with the MoveSpeed and deltaTime.
        Vector3 Value = PlayerMoveSpeed * Time.deltaTime * new Vector3(HorizontalX, VerticalY,0);

        //Changing the Position of the Player.
        transform.position += Value;

        //Clamp the Position of the Player to restrict OutOfBounds Condition.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -MaxXPos, MaxXPos), Mathf.Clamp(transform.position.y, -MaxYPos, MaxYPos), 0);
    }
}
