using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private List<Transform> moveSpots;

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

        if (Vector2.Distance(transform.position, moveSpots[moveSpotIdx].position) < 0.1f)
        {
            moveSpotIdx = (moveSpotIdx == 0) ? 1 : 0;
        }
    }
}
