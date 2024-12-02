using System;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float jumpForce = 100f;             // Force of the slime's jump
    public float jumpInterval = 2f;          // Time between jumps
    public Transform groundCheck;            // Ground check for slime
    public Transform wallCheck;              // Wall check for slime
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;             // Layer mask for wall detection
    public int health = 1;                   // Health of the slime

    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
    private bool isGrounded = false;
    private bool isWall = false;
    private float jumpTimer;
    private bool goingUp = false;

    private EnemySpawner enemySpawner;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpTimer = jumpInterval;

        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if(rb.velocity.y > 0) {
            animator.SetBool("goingUp", true);
        } else {
            animator.SetBool("goingUp", false);
        }
        // Update ground and wall detection
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
        isWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, whatIsWall);

        // Handle jumping logic
        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0 && isGrounded)
        {
            Jump();
            jumpTimer = jumpInterval;
        }

        // Flip the slime if it hits a wall
        if (isWall)
        {
            Flip();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(facingRight ? 2f : -2f, jumpForce);  // Jump with horizontal direction
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? .58f : -.58f, .58f, .58f); // Flip sprite
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
}
