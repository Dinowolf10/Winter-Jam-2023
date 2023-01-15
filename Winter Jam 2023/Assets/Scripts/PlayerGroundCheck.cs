using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Hit Something", collision.gameObject);

        // If the player is jumping or falling and collides with the floor, the current jump is over
        // Shoot a raycast down towards the floor to see if player is standing on top of floor
        if (collision.gameObject.tag == "Floor")
        {
            // If the player is standing on the floor, update jump variables
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(player.transform.position, Vector2.down, 0.4f))
            {
                //Debug.Log("Hit Floor?", hit.collider.gameObject);
                if (hit.collider.gameObject.tag == "Floor")
                {
                    player.isJumping = false;
                    player.canJump = true;
                    player.playerAnimator.SetBool("isJumping", false);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player leaves the floor, update jump variables
        if (collision.gameObject.tag == "Floor")
        {
            //Debug.Log("Left Floor", collision.gameObject);

            player.isJumping = true;
            player.canJump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the player is jumping or falling and collides with the floor, the current jump is over
        // Shoot a raycast down towards the floor to see if player is standing on top of floor
        if (collision.gameObject.tag == "Floor")
        {
            // If the player is standing on the floor, update jump variables
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(player.transform.position, Vector2.down, 0.4f))
            {
                //Debug.Log("Hit Floor?", hit.collider.gameObject);

                if (hit.collider.gameObject.tag == "Floor")
                {
                    player.isJumping = false;
                    player.canJump = true;
                    player.playerAnimator.SetBool("isJumping", false);
                }
            }
        }
    }
}
