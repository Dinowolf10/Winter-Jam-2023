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
    private PlayerInput playerInput;
    public Animator playerAnimator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float xMovement = 0f;
    [SerializeField]
    private float moveSpeed = 5f;

    public bool isJumping = false;
    public bool canJump = false;
    [SerializeField]
    private Vector2 jumpForce;
    [SerializeField]
    private Vector2 bounceForce;

    private LevelManager levelManager;
    private CollectableManager collectableManager;
    private Timer timer;
    private AudioManager audioManager;

    private bool openedDoor = false;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
        collectableManager = gameManager.GetComponent<CollectableManager>();
        timer = gameManager.GetComponent<Timer>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    private void Update()
    {
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
            // Check if there is something right above the player, if there is don't jump
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position, Vector2.up, 0.7f))
            {
                Debug.Log("No Jump");
                return;
            }

            // Otherwise, add force to simulate player jump
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
            canJump = false;
            isJumping = true;

            playerAnimator.SetBool("isIdle", false);
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("isJumping", true);
            audioManager.PlayUnique("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player hits a trap, restart the level
        if (collision.gameObject.tag == "Spikes" || collision.gameObject.tag == "Dart" || collision.gameObject.tag == "Boulder")
        {
            StartCoroutine(HandleDeath());
        }

        // If the player hits an enemy, determine who should be destroyed
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(HandleDeath());   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player collides with a key fragment, destroy it and update the collectables
        if (collision.gameObject.tag == "KeyFragment")
        {
            collectableManager.UpdateCollectables(collision.gameObject);
        }

        // If the player collides with a door, check if it is unlocked
        if (collision.gameObject.tag == "Door")
        {
            Door door = collision.gameObject.GetComponent<Door>();
            if (door.IsUnlocked() && !openedDoor)
            {
                // TODO: IMPLEMENT WIN CONDITION
                //playerInput.enabled = false;
                openedDoor = true;
                levelManager.StartCoroutine("LoadLevelTransition");
                timer.StopTicking();
                audioManager.Play("OpenDoor");
                Debug.Log("Level Complete!");
            }
        }
    }

    /// <summary>
    /// Handles the event that a player jumps on an enemy
    /// </summary>
    /// <param name="collision">collision with information about the player and enemy</param>
    public void StompEnemy(Collider2D collision)
    {
        Destroy(collision.gameObject);
        rb.AddForce(bounceForce, ForceMode2D.Impulse);

        if (collision.gameObject.tag.Contains("Bat"))
        {
            audioManager.Play("Bat");
        }
        else
        {
            audioManager.Play("Spider");
        }
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
        playerInput.enabled = false;
        spriteRenderer.enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        audioManager.StopAllEnvironmentSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
