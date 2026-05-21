using UnityEngine;

[RequireComponent(typeof(EnemyRegistration))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField, Range(0.0f, 1000.0f)] private float maxHealth = 10f;
    [SerializeField] private float currentHealth;

    public float MaxHealth
    {
        set
        {
            maxHealth = value;
            currentHealth = maxHealth;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(in DamageContext context)
    {
        currentHealth -= context.TotalDamage;
        if (currentHealth <= 0f)
        {
            Death();
        }
    }

    public void TakeDamage(int value)
    {
        ApplyDamage(new DamageContext(value, null));
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
