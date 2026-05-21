using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyLocomotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyTargeting targeting;
    private EnemyAttack attack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        targeting = GetComponent<EnemyTargeting>();
        attack = GetComponent<EnemyAttack>();
    }

    public void TickMovement(Transform target, bool isAttacking)
    {
        if (agent == null || animator == null)
        {
            return;
        }

        if (!isAttacking)
        {
            animator.SetBool("IsWalking", true);
        }

        if (target != null && attack != null)
        {
            float attackRange = attack.AttackRange;
            if (targeting != null && targeting.IsPrimaryTarget(target))
            {
                agent.stoppingDistance = attackRange * 0.95f;
            }
            else
            {
                agent.stoppingDistance = attackRange * 0.8f;
            }

            if (!isAttacking)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    public void SetInitialDestination(Transform target)
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
