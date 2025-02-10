using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public bool facingRight = true;
    private bool isWall = false;
    public Transform wallCheck;
    public LayerMask whatIsWall;

    public int health = 1;
    private EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        isWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, whatIsWall);

        if (isWall)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? .4f : -.4f, .4f, .4f); // Flip sprite
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        { 
            // Check if the player landed on the slime's head
            if (collision.contacts[0].normal.y < -0.5f)
            {
                
                collision.collider.GetComponent<PlayerController>().Bounce();  // Make the player bounce
                TakeDamage();
            }
            else
            {
                // Logic for when the slime damages the player
                collision.collider.GetComponent<PlayerController>().TakeDamage();
            }
        }
    }

    void OnTriggerEnter2D (Collider2D collision) {
        if(collision.CompareTag("Bullet")) {
            TakeDamage();
            Debug.Log("gverfrsw");   
        }
    }
    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        enemySpawner.enemiesDestroyed++;
        Debug.Log("Enemies defeated for the night: " + enemySpawner.enemiesDestroyed);
    }
}
