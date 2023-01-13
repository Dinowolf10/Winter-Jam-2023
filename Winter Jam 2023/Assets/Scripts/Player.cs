using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveVector;
    [SerializeField]
    private float xMovement = 0f;
    [SerializeField]
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(new Vector2(xMovement, 0) * moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move player based on player input
    /// </summary>
    /// <param name="value"></param>
    public void OnMovement(InputValue value)
    {
        xMovement = value.Get<float>();
    }
}
