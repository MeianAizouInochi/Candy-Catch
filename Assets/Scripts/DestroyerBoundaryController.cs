using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerBoundaryController : MonoBehaviour
{
    /*
     * The trigger Function which is invoked if any candy falls through without colliding with the Player game object,
     * hence going out of bounds to collide with Boundary, which destroys them and decreases the lives of the player.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("NormalCandy"))
        {
            GameManager.gameManager.LifeDecrementor();

            Destroy(collision.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
