using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    public void Configure(float maxHealth)
    {
        health = maxHealth;
    }

    public void ApplyDamage(in DamageContext context)
    {
        health -= context.TotalDamage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(float damage)
    {
        ApplyDamage(new DamageContext(damage, null));
    }
}
