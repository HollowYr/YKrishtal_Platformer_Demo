using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Idle : StateMachineBehaviour
{

    GameObject player;
    public int distance;
    EnemyContoller enemyController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyController = animator.GetComponent<EnemyContoller>();
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {

        if (!enemyController.isDead())
        {
            if (Vector2.Distance(animator.transform.position, player.transform.position) < distance)
            {
                enemyController.Attack();
            }
            else
            {
                animator.SetBool("IsIdle", false);
            }
        }
        else enemyController.Die();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
