using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehavior : StateMachineBehaviour
{
    public float attackRange = 10f;  // Distance within which the bat can attack the player.
    public float flySpeed = 4f;     // Horizontal flying speed.
    public float verticalAmplitude = 0.5f;  // Amplitude for up and down movement.
    public float verticalFrequency = 1f;    // Frequency of the vertical movement.

    private Transform playerPos;
    private float initialY; // To store the initial Y position of the bat.
    private float elapsedTime; // To control the wave motion.

    private float checkTimer = 0f;  // Timer for checking the player's distance.
    private float checkInterval = 3f;  // Time interval for checking (3 seconds).

    private float idleTimer = 0f;  // Timer for checking the player's distance.
    private float idleInterval = 10f;  // Time interval for checking (3 seconds).
    private bool foundIdleLocation = false;
    private Vector2 targetPosition;

    private BatController batController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        initialY = animator.transform.position.y;
        elapsedTime = 0f;
        checkTimer = 0f;  // Reset the timer.

        batController = FindObjectOfType<BatController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Horizontal flying movement
        if (batController.facingRight)
        {
            animator.transform.Translate(Vector2.right * flySpeed * Time.deltaTime);
        }
        else
        {
            animator.transform.Translate(Vector2.left * flySpeed * Time.deltaTime);
        }

        if(foundIdleLocation) {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, targetPosition, flySpeed * Time.deltaTime);

            // Check if bat has reached the target
            if (Vector2.Distance(animator.transform.position, targetPosition) <= 0.1f) // Slightly smaller threshold
            {
                animator.transform.position = targetPosition;  // Snap to position to prevent jittering
                foundIdleLocation = false;
                animator.SetBool("isIdle", true);
            }
        } else {
            // Vertical sine wave movement.
            elapsedTime += Time.deltaTime;
            float newY = initialY + Mathf.Sin(elapsedTime * verticalFrequency) * verticalAmplitude;
            animator.transform.position = new Vector2(animator.transform.position.x, newY);

            // Timer for checking the player’s distance every 3 seconds
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkInterval)
            {
                checkTimer = 0f;  // Reset the timer
                CheckForAttack(animator);
            }

            idleTimer += Time.deltaTime;
            if (idleTimer >= idleInterval)
            {
                idleTimer = 0f;  // Reset the timer

                // Select a random branch from the array
                Transform selectedBranch = batController.branches[Random.Range(0, batController.branches.Length)];
                Vector2 branchPosition = selectedBranch.transform.position;
                double db = selectedBranch.transform.position.x;

                // Get branch width from BoxCollider2D in world space
                BoxCollider2D branchCollider = selectedBranch.GetComponent<BoxCollider2D>();
                float branchWidth = branchCollider.size.x * selectedBranch.lossyScale.x;

                // Random x position along the branch within the collider bounds
                float randomX = Random.Range(-branchWidth / 2f, branchWidth / 2f);
                targetPosition = new Vector2(branchPosition.x + randomX, branchPosition.y - 0.5f); // Slightly below branch

                foundIdleLocation = true;
            }
        }
    }

    private void CheckForAttack(Animator animator)
    {
        float distanceToPlayer = Vector2.Distance(animator.transform.position, playerPos.position);
        if (distanceToPlayer <= attackRange)
        {
            // 33% chance to attack
            if (Random.value <= 0.33f)
            {
                animator.SetBool("isAttacking", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: reset state variables if necessary.
    }
}