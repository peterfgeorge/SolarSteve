using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public bool facingRight = true;
    private bool isWall = false;
    public Transform wallCheck;
    public LayerMask whatIsWall;
    public Transform[] branches;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
