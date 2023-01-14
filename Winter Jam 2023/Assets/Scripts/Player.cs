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
    private AudioManager audioManager;

    // Start is called before the first frame update
    private void Start()
    {
        collectableManager = GameObject.Find("GameManager").GetComponent<CollectableManager>();
        timer = GameObject.Find("GameManager").GetComponent<Timer>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
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

        HandleFootsteps();
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

        audioManager.PlayUnique("Jump");
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
            StartCoroutine(HandleDeath());
        }

        // If the player hits an enemy, determine who should be destroyed
        if (collision.gameObject.tag == "Enemy")
        {
            if (EnemyStomped(collision))
            {
                if (collision.gameObject.name.Contains("Spider"))
                {
                    audioManager.Play("Spider");
                }
                else
                {
                    audioManager.Play("Bat");
                }
                Destroy(collision.gameObject);
                rb.AddForce(bounceForce, ForceMode2D.Impulse);
            } 
            else
            {
                StartCoroutine(HandleDeath());
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
            audioManager.Play("CollectItem");
        }

        // If the player collides with a door, check if it is unlocked
        if (collision.gameObject.tag == "Door")
        {
            Door door = collision.gameObject.GetComponent<Door>();
            if (door.IsUnlocked())
            {
                // TODO: IMPLEMENT WIN CONDITION
                timer.StopTicking();
                audioManager.Play("OpenDoor");
                Debug.Log("Level Complete!");
            }
        }
    }

    /// <summary>
    /// Determines if an enemy was stomped on during this collision
    /// </summary>
    /// <param name="collision">collision being checked</param>
    /// <returns>true if an enemy was stomped on, false otherwise</returns>
    private bool EnemyStomped(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, stompDetection, 0, Vector2.down);
        return hit.collider == collision.collider;
    }

    /// <summary>
    /// Starts and stops footstep audio depending on the players actions
    /// </summary>
    private void HandleFootsteps()
    {
        if (xMovement == 0 || isJumping)
        {
            audioManager.StopSound("Steps");
        }
        else
        {
            audioManager.PlayUnique("Steps");
        }
    }

    /// <summary>
    /// Puts the player in a dead state and reloads the current level
    /// </summary>
    private IEnumerator HandleDeath()
    {
        audioManager.Play("DeathGrunt");
        spriteRenderer.enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        audioManager.StopAllEnvironmentSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
