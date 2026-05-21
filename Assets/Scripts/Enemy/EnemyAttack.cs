using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAttack : MonoBehaviour
{
    [FormerlySerializedAs("attackRange")]
    [SerializeField, Range(0f, 20f)] private float attackRange = 5f;

    public float AttackRange => attackRange;

    public bool IsAttacking { get; private set; }

    private Animator animator;
    private EnemyTargeting targeting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        targeting = GetComponent<EnemyTargeting>();
    }

    public bool TickAttack(Transform target)
    {
        IsAttacking = CanAttack(target);
        if (animator != null)
        {
            animator.SetBool("Attacking", IsAttacking);
        }

        if (IsAttacking && target != null)
        {
            FaceTarget(target);
        }

        return IsAttacking;
    }

    private bool CanAttack(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        return Vector3.Distance(transform.position, target.position) < attackRange;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 directionTowards = target.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, directionTowards, 0.2f, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
