using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    Boid boid;
    Animal stats;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boid = animator.GetComponentInParent<Boid>();
        stats = animator.GetComponent<Animal>();

        if (boid)
            boid.flocking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stats.Idle();

        // Change states
        if (stats.hunger > 70)
            animator.SetBool("Hungry", true);
        else if (stats.sexDrive > 75)
            animator.SetBool("Repro", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boid)
            boid.flocking = false;
    }

}
