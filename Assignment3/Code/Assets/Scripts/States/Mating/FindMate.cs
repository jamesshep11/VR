using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMate : StateMachineBehaviour
{
    Animal stats;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        stats = animator.GetComponent<Animal>();
        stats.target = null;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stats.target == null) {
            GameObject newTarget =  stats.Find(1 << animator.gameObject.layer);
            if (newTarget.GetComponent<Animal>().gender != stats.gender)
                stats.target = newTarget;
        } else
            animator.SetTrigger("Chase");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger("Chase");
    }

}
