using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    // Enum for direction boulder should move
    public enum BoulderDirection
    {
        right,
        left,
    }

    [SerializeField]
    private BoulderDirection direction;
    [SerializeField]
    private Vector2 boulderVelocity;
    [SerializeField]
    private float startingBoulderSpeed;
    [SerializeField]
    private float maxBoulderSpeed;
    [SerializeField]
    private float boulderAccelerationRate;
    [SerializeField]
    private float cooldownDuration;
    [SerializeField]
    private float cooldownTimer;
    [SerializeField]
    private GameObject boulderProjectile;
    [SerializeField]
    private Transform boulderProjectileTransform;
    [SerializeField]
    private Transform dropPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to boulderProjectile if there is none
        if (!boulderProjectileTransform || !boulderProjectile)
        {
            boulderProjectileTransform = transform.Find("BoulderProjectile");
            boulderProjectile = boulderProjectileTransform.gameObject;
        }
        boulderProjectile.SetActive(false);

        // Get reference to dropPoint if there is none and set position
        if (!dropPoint)
        {
            dropPoint = transform.Find("DropPoint");
        }
        dropPoint.position = (Vector2)transform.position + Vector2.down;

        // Set up dropPoint position and boulder velocity based on the BoulderDirection enum selected
        if (direction == BoulderDirection.right)
        {
            boulderVelocity = Vector2.right;
        }
        else if (direction == BoulderDirection.left)
        {
            boulderVelocity = Vector2.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update cooldown timer, if it reaches zero fire the projectile and reset timer
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            DropBoulder();
            cooldownTimer = cooldownDuration;
        }
    }

    /// <summary>
    /// Resets and drops the boulder projectile
    /// </summary>
    private void DropBoulder()
    {
        // Reset the boulder projectile
        boulderProjectile.SetActive(true);
        boulderProjectileTransform.position = dropPoint.position;

        // Update the variables of the boulder projectile
        boulderProjectile.GetComponent<BoulderProjectile>().SetProjectile(boulderVelocity, startingBoulderSpeed, maxBoulderSpeed, boulderAccelerationRate, cooldownDuration - .1f);
    }
}
