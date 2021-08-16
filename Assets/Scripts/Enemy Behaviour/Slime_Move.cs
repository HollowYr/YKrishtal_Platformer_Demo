using UnityEngine;

public class Slime_Move : StateMachineBehaviour
{
    EnemyContoller enemyController;
    GameObject player;
    Transform currentPatrolPoint;
    int patrolPointIndex = 0;
    public float distance, speed;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyController = animator.GetComponent<EnemyContoller>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentPatrolPoint = enemyController.patrolPoints[patrolPointIndex];
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemyController.isDead() && !player.GetComponent<PlayerScript>().heartsHealthVisual.IsDead())
        {
            float distanceToPlayer = Vector2.Distance(animator.transform.position, player.transform.position);
            if (distanceToPlayer > distance)
            {
                float distanceToPatrolPoint = Vector2.Distance(enemyController.transform.position, currentPatrolPoint.position);

                if (distanceToPatrolPoint > distance)
                {
                    enemyController.MoveTo(currentPatrolPoint, speed);
                }
                else if (distanceToPatrolPoint > distanceToPlayer)
                {
                    enemyController.MoveTo(player.transform, speed);
                }
                else
                {
                    if (patrolPointIndex >= enemyController.patrolPoints.Length - 1)
                    {
                        patrolPointIndex = 0;
                        animator.SetBool("IsIdle", true);
                    }
                    else
                    {
                        patrolPointIndex++;
                    }

                    currentPatrolPoint = enemyController.patrolPoints[patrolPointIndex];
                }

            }
            else if (distanceToPlayer < distance)
            {
                animator.SetBool("IsIdle", true);
            }
        }
    }
}
