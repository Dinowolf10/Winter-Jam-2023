using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    // Enum for direction dart should move
    public enum DartDirection
    {
        right,
        left,
        up,
        down
    }

    [SerializeField]
    private DartDirection direction;
    [SerializeField]
    private Vector2 dartVelocity;
    [SerializeField]
    private float dartSpeed;
    [SerializeField]
    private float cooldownDuration;
    [SerializeField]
    private float cooldownTimer;
    [SerializeField]
    private GameObject dartProjectile;
    [SerializeField]
    private Transform dartProjectileTransform;
    [SerializeField]
    private Transform firePoint;

    // Start is called before the first frame update
    private void Start()
    {
        // Get reference to dartProjectile if there is none
        if (!dartProjectileTransform || !dartProjectile)
        {
            dartProjectileTransform = transform.Find("DartProjectile");
            dartProjectile = dartProjectileTransform.gameObject;
        }
        dartProjectile.SetActive(false);

        // Get reference to firePoint if there is none
        if (!firePoint)
        {
            firePoint = transform.Find("FirePoint");
        }

        // Set up firePoint position and dart velocity based on the DartDirection enum selected
        if (direction == DartDirection.right)
        {
            firePoint.position = (Vector2)transform.position + Vector2.right;
            dartVelocity = Vector2.right;
        }
        else if (direction == DartDirection.left)
        {
            firePoint.position = (Vector2)transform.position + Vector2.left;
            dartVelocity = Vector2.left;
        }
        else if (direction == DartDirection.up)
        {
            firePoint.position = (Vector2)transform.position + Vector2.up;
            dartVelocity = Vector2.up;
        }
        else
        {
            firePoint.position = (Vector2)transform.position + Vector2.down;
            dartVelocity = Vector2.down;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Update cooldown timer, if it reaches zero fire the projectile and reset timer
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            FireProjectile();
            cooldownTimer = cooldownDuration;
        }
    }

    /// <summary>
    /// Resets and fires the dart projectile
    /// </summary>
    private void FireProjectile()
    {
        // Reset the dart projectile
        dartProjectile.SetActive(true);
        dartProjectileTransform.position = firePoint.position;

        // Update the variables of the dart projectile
        dartProjectile.GetComponent<DartProjectile>().SetProjectile(dartVelocity, dartSpeed, cooldownDuration - .1f);
    }
}