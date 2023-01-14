using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifeDuration;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Move projectile
        transform.Translate(velocity * speed * Time.deltaTime);

        // Check if projectile should be deactivated
        lifeDuration -= Time.deltaTime;
        if (lifeDuration <= 0)
        {
            DeactiveProjectile();
        }
    }

    /// <summary>
    /// Set the projectile variables when fired
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="speed"></param>
    /// <param name="maxLife"></param>
    public void SetProjectile(Vector2 velocity, float speed, float maxLife)
    {
        this.velocity = velocity;
        this.speed = speed;
        lifeDuration = maxLife;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Deactivate projectile when colliding into something
        DeactiveProjectile();
    }

    /// <summary>
    /// Deactivates the projectile
    /// </summary>
    public void DeactiveProjectile()
    {
        velocity = Vector2.zero;
        speed = 0;
        gameObject.SetActive(false);
    }
}
