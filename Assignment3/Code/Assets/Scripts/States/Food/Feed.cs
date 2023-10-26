using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feed : StateMachineBehaviour
{
    Animal stats;
    LivingEntity targetStats;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        stats = animator.GetComponent<Animal>();
        if (stats.target != null)
            targetStats = stats.target.GetComponent<LivingEntity>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Feed
        if (stats.target != null) {
            stats.Feed(targetStats);
        }
        
        // Change States
        if (stats.hunger <= 30) {
            animator.SetBool("Hungry", false);
            animator.SetTrigger("Fed");
        } else
            animator.SetTrigger("Fed");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger("Fed");
    }
}
