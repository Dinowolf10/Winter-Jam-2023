using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    // speed at which the enemy moves
    [SerializeField]
    private float moveSpeed;

    // locations the the enemy patrols between
    [SerializeField]
    private List<Transform> moveSpots;

    // index of move spot currently being moved toward
    private int moveSpotIdx;

    // Start is called before the first frame update
    void Start()
    {
        moveSpotIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[moveSpotIdx].position, moveSpeed * Time.deltaTime);

        // if the enemy has reached a move spot, update it to move toward the next move spot
        if (Vector2.Distance(transform.position, moveSpots[moveSpotIdx].position) < 0.1f)
        {
            moveSpotIdx = (moveSpotIdx == 0) ? 1 : 0;
        }
    }
}
