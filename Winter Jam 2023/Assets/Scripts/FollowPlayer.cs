using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

/// <summary>
/// Modifies the camera to follow to player
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    // x-axis camera offset
    [SerializeField]
    private float xOffset;

    // y-axis camera offset
    [SerializeField]
    private float yOffset;

    [SerializeField]
    private float maxYThreshold;

    [SerializeField]
    private float minYThreshold;

    // speed at which the camera moves toward its offset position
    [SerializeField]
    private float offsetSmoothing;

    private GameObject player;
    private Vector3 playerPosition;
    private SpriteRenderer playerSpriteRenderer;
    private float playerY;
    public bool playerDead;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDead)
        {
            return;
        }

        // determine if the camera should move on the y-axis or not
        playerY = player.transform.position.y;
        if (playerY > maxYThreshold)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, transform.position.z);
        }
        else if (playerY < minYThreshold)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y - yOffset, transform.position.z);
        }
        else
        {
            playerPosition = new Vector3(player.transform.position.x, 0, transform.position.z);
        }
        

        // calculate offset camera position
        if (playerSpriteRenderer.flipX == false)
        {
            playerPosition = new Vector3(playerPosition.x + xOffset, playerPosition.y, transform.position.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - xOffset, playerPosition.y, transform.position.z);
        }

        // lerp camera toward its desired location
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}
