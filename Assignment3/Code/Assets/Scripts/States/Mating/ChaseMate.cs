using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMate : StateMachineBehaviour
{
    GameObject gameObject;
    Animal stats;

    string curState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        gameObject = animator.gameObject;
        stats = gameObject.GetComponent<Animal>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stats.target == null) {
            curState = "LostTarget";
            animator.SetTrigger(curState);
        } else if (stats.Chase()) {
            if (stats.RequestMating(layerIndex))
                curState = "Mate";
            else
                curState = "LostTarget";
            animator.SetTrigger(curState);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger(curState);
    }

}
