using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    public Animator animator;

    private SpriteRenderer sr; // Assuming the player uses a SpriteRenderer
    private Color originalColor;

    public int batteryAmount = 0;

    public int health = 3;
    public float bounceForce = 50f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        Physics2D.IgnoreLayerCollision(7, 9, true);
    }

    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Update() {

        animator.SetFloat("Speed", Mathf.Abs(moveInput * speed));

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if(isGrounded) {
            animator.SetBool("IsJumping", false);
        }

        if(moveInput > 0){
            transform.eulerAngles = new Vector3(0,0,0);
        } else if(moveInput < 0){
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space)) {
            isJumping = true;
            animator.SetBool("IsJumping", true);
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if(Input.GetKey(KeyCode.Space) && isJumping == true) {
            if(jumpTimeCounter > 0) {
                rb.velocity = Vector2.up * jumpForce;
                animator.SetBool("IsJumping", true);
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
            
        if(Input.GetKeyUp(KeyCode.Space)) {
            isJumping = false;
        }
    }

    public void Bounce()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, bounceForce);  // Bounce the player upward
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            // Handle player death
            Debug.Log("Player Died");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hi!");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(7, 9, true);
        Debug.Log(LayerMask.NameToLayer("Enemy"));
        Debug.Log(LayerMask.NameToLayer("Player"));
    }
}
