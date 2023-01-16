using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationRate;
    [SerializeField]
    private float lifeDuration;
    private bool onFloor = false;
    [SerializeField]
    private Animator animator;

    private AudioManager audioManager;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        levelManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.gamePaused)
        {
            audioManager.StopSound("Boulder");
            return;
        }

        // Boulder will only move in the x direction if on the floor
        if (onFloor)
        {
            audioManager.PlayUnique("Boulder");
            // Update boulder speed
            if (speed < maxSpeed)
            {
                // Increase speed using acceleration rate
                speed += speed * accelerationRate * Time.deltaTime;

                // Check if boulder went over max speed
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                animator.speed = (2 * speed) / maxSpeed;
            }

            // Move boulder projectile
            transform.Translate(velocity * speed * Time.deltaTime);
        }

        // Check if boulder projectile should be deactivated
        lifeDuration -= Time.deltaTime;
        if (lifeDuration <= 0)
        {
            DeactiveProjectile();
        }
    }

    /// <summary>
    /// Sets variables for the boulder projectile
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="startingSpeed"></param>
    /// <param name="maxSpeed"></param>
    /// <param name="accelerationRate"></param>
    /// <param name="maxLife"></param>
    public void SetProjectile(Vector2 velocity, float startingSpeed, float maxSpeed, float accelerationRate, float maxLife)
    {
        this.velocity = velocity;
        speed = startingSpeed;
        this.maxSpeed = maxSpeed;
        this.accelerationRate = accelerationRate;
        lifeDuration = maxLife;
        onFloor = false;
    }

    /// <summary>
    /// Deactivates the boulder projectile
    /// </summary>
    public void DeactiveProjectile()
    {
        velocity = Vector2.zero;
        speed = 0;
        maxSpeed = 0;
        accelerationRate = 0;
        onFloor = false;
        gameObject.SetActive(false);
        audioManager.StopSound("Boulder");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks if boulder is on the floor
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = true;
            animator.SetBool("isRolling", true);
            animator.speed = (2 * speed) / maxSpeed;
        }
        // Otherwise, deactivate projectile when colliding into something besides the floor
        else
        {
            DeactiveProjectile();
        }
    }
}
