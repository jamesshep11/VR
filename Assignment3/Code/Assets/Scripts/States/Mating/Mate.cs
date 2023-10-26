using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mate : StateMachineBehaviour
{
    GameObject gameObject;
    Animal stats;
    ParticleSystem hearts;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        gameObject = animator.gameObject;
        stats = gameObject.GetComponent<Animal>();
        hearts = gameObject.GetComponent<ParticleSystem>();

        stats.sexDrive = 0;

        hearts.Play();
        stats.waitThen(5, () => {
            hearts.Stop();

            animator.SetBool("Repro", false);
        });
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    
    }

}
