using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackBehavior : StateMachineBehaviour
{
    private Transform playerPos;
    private Vector3 startingPos;
    public float speed;
    private float delayBeforeAttack = 0.75f;  // Delay in seconds
    private float elapsedTime = 0f;  // Timer for the delay

    private bool isDelayComplete = false;
    private bool gotPlayerLocation = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        

        elapsedTime = 0f;  // Reset the timer
        isDelayComplete = false;  // Reset the delay state
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isDelayComplete)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= delayBeforeAttack)
            {
                isDelayComplete = true;  // Delay is complete, proceed with the attack

                if(!gotPlayerLocation) {
                    startingPos = playerPos.position;
                }
            }
            return;  // Exit early until delay is complete
        }

        // Move towards the player's initial position
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, startingPos, speed * Time.deltaTime);

        float distanceToPlayer = Vector2.Distance(animator.transform.position, startingPos);
        if (distanceToPlayer <= 0.1f)
        {
            animator.SetBool("isAttacking", false);  // Exit attack state
            gotPlayerLocation = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional cleanup or reset logic if needed
    }
}
