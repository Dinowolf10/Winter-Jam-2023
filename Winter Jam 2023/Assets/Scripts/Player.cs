using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xMovement = 0f;
    [SerializeField]
    private float moveSpeed = 5f;

    private bool isJumping = false;
    private bool canJump = false;
    [SerializeField]
    private Vector2 jumpForce;
    [SerializeField]
    private Vector2 bounceForce;
    [SerializeField]
    private Vector2 stompDetection;

    private CollectableManager collectableManager;
    private Timer timer;

    // Start is called before the first frame update
    private void Start()
    {
        collectableManager = GameObject.Find("GameManager").GetComponent<CollectableManager>();
        timer = GameObject.Find("GameManager").GetComponent<Timer>();
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

        // Update animator bools
        if (xMovement == 0)
        {
            playerAnimator.SetBool("isIdle", true);
            playerAnimator.SetBool("isRunning", false);
        }
        else
        {
            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isRunning", true);
        }
    }

    /// <summary>
    /// Move player based on player input
    /// </summary>
    /// <param name="value"></param>
    public void OnMovement(InputValue value)
    {
        xMovement = value.Get<float>();

        if (xMovement == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (xMovement == -1)
        {
            spriteRenderer.flipX = true;
        }
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

        // If the player hits a trap, restart the level
        if (collision.gameObject.tag == "Spikes" || collision.gameObject.tag == "Dart" || collision.gameObject.tag == "Boulder")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // If the player hits an enemy, determine who should be destroyed
        if (collision.gameObject.tag == "Enemy")
        {
            if (EnemyStomped(collision))
            {
                Destroy(collision.gameObject);
                rb.AddForce(bounceForce, ForceMode2D.Impulse);
            } 
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with a key fragment, destroy it and update the collectables
        if (collision.gameObject.tag == "KeyFragment")
        {
            Destroy(collision.gameObject);
            collectableManager.UpdateCollectables();
        }

        // If the player collides with a door, check if it is unlocked
        if (collision.gameObject.tag == "Door")
        {
            Door door = collision.gameObject.GetComponent<Door>();
            if (door.IsUnlocked())
            {
                // TODO: IMPLEMENT WIN CONDITION
                timer.StopTicking();
                Debug.Log("Level Complete!");
            }
        }
    }

    /// <summary>
    /// Determines if an enemy was stomped on during this collision
    /// </summary>
    /// <param name="collision">collision being checked</param>
    /// <returns>true if an enemy was stomped on, false otherwise</returns>
    bool EnemyStomped(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, stompDetection, 0, Vector2.down);
        return hit.collider == collision.collider;
    }
}
