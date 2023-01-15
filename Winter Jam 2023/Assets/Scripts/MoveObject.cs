using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    // speed at which the enemy moves
    [SerializeField]
    private float moveSpeed;

    // magnitude of the arc made by the enemy during movement (primarily for bats)
    [SerializeField]
    [Range(0f, 1f)]
    private float arcMagnitude;

    // enemy movement vector with the arc applied
    private Vector2 arcVector;

    // locations the the enemy patrols between
    [SerializeField]
    private List<Transform> moveSpots;

    // index of move spot currently being moved toward
    private int moveSpotIdx;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        moveSpotIdx = 0;

        SetAnimatorState();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            return;
        }

        if (moveSpots.Count == 2)
        {
            if (moveSpots[moveSpotIdx].position.x - transform.position.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            // move the enemy toward the next move spot and apply the desired arc amount
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[moveSpotIdx].position, moveSpeed * Time.deltaTime);
            arcVector = new Vector2(transform.position.x, transform.position.y + (Mathf.Sin(Time.deltaTime) * arcMagnitude));
            transform.position = arcVector;

            // if the enemy has reached a move spot, update it to move toward the next move spot
            if (Vector2.Distance(transform.position, moveSpots[moveSpotIdx].position) < 0.1f)
            {
                moveSpotIdx = (moveSpotIdx == 0) ? 1 : 0;
            }
        }
    }

    public IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", false);

        yield return new WaitForSeconds(1.2f);

        isAttacking = false;
        animator.SetBool("isAttacking", false);
        SetAnimatorState();
    }

    private void SetAnimatorState()
    {
        if (moveSpots.Count == 2)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isIdle", true);
        }
    }
}
