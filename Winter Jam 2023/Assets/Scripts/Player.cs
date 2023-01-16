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
    [SerializeField]
    private Collider2D groundCheckCollider;
    [SerializeField]
    private Collider2D stompCheckCollider;

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
    private bool isDying = false;

    // For opening cutscene
    public bool inCutscene = false;
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private GameObject crown;
    [SerializeField]
    private GameObject cutsceneCanvas;
    [SerializeField]
    private GameObject firstText;
    [SerializeField]
    private GameObject secondText;
    [SerializeField]
    private GameObject timerCanvas;
    [SerializeField]
    private GameObject winCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        levelManager = gameManager.GetComponent<LevelManager>();
        collectableManager = gameManager.GetComponent<CollectableManager>();
        timer = gameManager.GetComponent<Timer>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            playerInput.enabled = false;
            Camera cam = Camera.main;
            cam.GetComponent<FollowPlayer>().playerDead = true;
            inCutscene = true;
            StartCoroutine(OpeningCutscene());
        }
        /*else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            playerInput.enabled = false;
            inCutscene = true;
        }*/
    }

    // Update is called once per frame
    private void Update()
    {
        if (inCutscene)
        {
            return;
        }

        HandleFootsteps();
    }

    private void FixedUpdate()
    {
        if (inCutscene)
        {
            return;
        }

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
        if (isDying)
        {
            return;
        }

        // If the player hits a trap, restart the level
        if (collision.gameObject.tag == "Spikes" || collision.gameObject.tag == "Dart" || collision.gameObject.tag == "Boulder")
        {
            StartCoroutine(HandleDeath());
        }

        // If the player hits an enemy, determine who should be destroyed
        if (collision.gameObject.tag == "Enemy")
        {
            MoveObject m = collision.gameObject.GetComponent<MoveObject>();
            m.StartCoroutine(m.AttackPlayer(transform.position));
            StartCoroutine(HandleDeath());   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying)
        {
            return;
        }

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
                playerInput.enabled = false;
                openedDoor = true;
                levelManager.StopAllCoroutines();
                if (SceneManager.GetActiveScene().name == "Level11")
                {
                    WinGame();
                }
                else
                {
                    levelManager.StartCoroutine("LoadLevelTransition");
                }
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
    public IEnumerator HandleDeath()
    {
        audioManager.Play("DeathGrunt");
        playerAnimator.SetBool("isIdle", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isDying", true);
        playerInput.enabled = false;
        isDying = true;
        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        groundCheckCollider.enabled = false;
        stompCheckCollider.enabled = false;
        Camera cam = Camera.main;
        cam.GetComponent<FollowPlayer>().playerDead = true;
        yield return new WaitForSeconds(2);
        audioManager.StopAllEnvironmentSounds();

        if (!openedDoor)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator OpeningCutscene()
    {
        Door door = GameObject.Find("Door").GetComponent<Door>();

        yield return new WaitForSeconds(1f);

        Vector2 targetPosition = new Vector2(-4.5f, -1.5f);
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2.5f * Time.deltaTime);
            playerAnimator.SetBool("isRunning", true);
            playerAnimator.SetBool("isIdle", false);
            yield return null;
        }

        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isIdle", true);
        audioManager.Play("CollectItem");
        cutsceneCanvas.SetActive(true);
        yield return new WaitForSeconds(7f);

        audioManager.Play("OpenDoor");
        crown.SetActive(false);
        key.SetActive(true);
        door.LockDoor();
        rb.AddForce(bounceForce, ForceMode2D.Impulse);
        playerAnimator.SetBool("isIdle", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", true);
        cutsceneCanvas.SetActive(false);

        yield return new WaitForSeconds(2f);
        cutsceneCanvas.SetActive(true);
        firstText.SetActive(false);
        secondText.SetActive(true);

        yield return new WaitForSeconds(8f);

        Camera cam = Camera.main;
        cam.GetComponent<FollowPlayer>().playerDead = false;
        inCutscene = false;
        timer.StartTicking();
        timerCanvas.SetActive(true);
        cutsceneCanvas.SetActive(false);
        playerInput.enabled = true;
    }

    private void WinGame()
    {
        winCanvas.SetActive(true);
    }

    /*private IEnumerator OpeningCutsceneMenu()
    {
        playerAnimator.SetBool("isIdle", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", true);
        audioManager.PlayUnique("Jump");
        rb.AddForce(new Vector2 (5.0f, bounceForce.y), ForceMode2D.Impulse);

        yield return new WaitForSeconds(2f);
    }*/
}
