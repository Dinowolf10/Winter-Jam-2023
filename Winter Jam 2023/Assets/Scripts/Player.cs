using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Vector2 moveVelocity;
    [SerializeField]
    private float xMovement = 0f;
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool canJump = false;
    [SerializeField]
    private Vector2 jumpForce;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Update jump variables
        if (rb.velocity.y == 0f)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        // Move player
        rb.velocity = new Vector2(xMovement * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// Move player based on player input
    /// </summary>
    /// <param name="value"></param>
    public void OnMovement(InputValue value)
    {
        xMovement = value.Get<float>();
    }

    /// <summary>
    /// Performs a jump for the player
    /// </summary>
    /// <param name="value"></param>
    public void OnJump(InputValue value)
    {
        // Only jump if the player is not currently jumping and if the player can jump (touching ground)
        if (!isJumping && canJump)
        {
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
            canJump = false;
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is jumping and collides with the floor, the current jump is over
        if (isJumping)
        {
            if (collision.gameObject.tag == "Floor")
            {
                isJumping = false;
            }
        }
    }
}
