using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyTargeting enemyTargeting;
    [SerializeField, Range(0f, 50f)] private float damage = 20f;

    private void Awake()
    {
        if (enemyAttack == null)
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }

        if (enemyTargeting == null)
        {
            enemyTargeting = GetComponent<EnemyTargeting>();
        }
    }

    public void DealDamage()
    {
        if (enemyAttack == null || !enemyAttack.IsAttacking)
        {
            return;
        }

        Transform target = enemyTargeting != null ? enemyTargeting.CurrentTarget : null;
        if (target == null)
        {
            return;
        }

        if (target.TryGetComponent(out IDamageable directDamageable))
        {
            directDamageable.ApplyDamage(new DamageContext(damage, gameObject));
            return;
        }

        if (target.parent != null && target.parent.TryGetComponent(out IDamageable parentDamageable))
        {
            parentDamageable.ApplyDamage(new DamageContext(damage, gameObject));
            return;
        }

        Debug.Log("Target doesn't have an IDamageable component.");
    }
}
