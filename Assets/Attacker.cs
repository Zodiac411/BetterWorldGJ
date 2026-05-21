using UnityEngine;

[RequireComponent(typeof(TurretLogic))]
public class Attacker : MonoBehaviour
{
    [SerializeField] private TurretData turretDefinition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject head;

    private float attackRange = 10f;
    private float fireRate = 1f;
    private float bulletSpeed = 20f;
    private float bulletDamage = 10f;
    private float fireCooldown;

    private void Awake()
    {
        ApplyDefinition(turretDefinition);
    }

    public void ApplyDefinition(TurretData definition)
    {
        turretDefinition = definition;
        if (definition == null)
        {
            return;
        }

        if (definition.bulletPrefab != null)
        {
            bulletPrefab = definition.bulletPrefab;
        }

        attackRange = definition.attackRange;
        fireRate = definition.fireRate;
        bulletSpeed = definition.bulletSpeed;
        bulletDamage = definition.bulletDamage;
    }

    private void Update()
    {
        Transform nearestEnemy = EnemyRegistry.FindNearest(transform.position, attackRange);
        if (nearestEnemy == null)
        {
            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }

            return;
        }

        RotateTowards(nearestEnemy.position);

        if (fireCooldown <= 0f)
        {
            ShootAt(nearestEnemy);
            fireCooldown = fireRate > 0f ? 1f / fireRate : 1f;
        }

        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private void RotateTowards(Vector3 target)
    {
        if (head != null)
        {
            head.transform.LookAt(target);
        }
    }

    private void ShootAt(Transform enemy)
    {
        if (bulletPrefab == null || shootingPoint == null)
        {
            return;
        }

        GameObject spawnedBullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        if (spawnedBullet.TryGetComponent(out bullet projectile))
        {
            projectile.Configure(bulletDamage, gameObject);
        }

        if (spawnedBullet.TryGetComponent(out Rigidbody bulletRb))
        {
            Vector3 direction = (enemy.position - shootingPoint.position).normalized;
            bulletRb.velocity = direction * bulletSpeed;
        }
    }
}
