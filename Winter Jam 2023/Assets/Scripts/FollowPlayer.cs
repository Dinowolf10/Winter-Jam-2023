using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

/// <summary>
/// Modifies the camera to follow to player
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    // reference to player object
    [SerializeField]
    private GameObject player;

    // amount the camera shows ahead of the player
    [SerializeField]
    private float offset;

    // speed at which the camera moves toward its offset position
    [SerializeField]
    private float offsetSmoothing;

    private Vector3 playerPosition;
    private SpriteRenderer playerSpriteRenderer;

    private void Start()
    {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        // calculate offset camera position
        if (playerSpriteRenderer.flipX == false)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, transform.position.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, transform.position.z);
        }

        // lerp camera toward its desired location
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}
