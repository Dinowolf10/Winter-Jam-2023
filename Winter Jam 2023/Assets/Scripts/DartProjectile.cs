using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
    }

    public void SetProjectile(Vector2 velocity, float speed)
    {
        this.velocity = velocity;
        this.speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DeactiveProjectile();
    }

    public void DeactiveProjectile()
    {
        velocity = Vector2.zero;
        speed = 0;
        gameObject.SetActive(false);
    }
}
