using UnityEngine;

public class Damagable : MonoBehaviour, IDamageable
{
    [SerializeField] private float initialHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = initialHealth;
    }

    public void ApplyDamage(in DamageContext context)
    {
        if (currentHealth <= 0f)
        {
            return;
        }

        currentHealth -= context.TotalDamage;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(float damage)
    {
        ApplyDamage(new DamageContext(damage, gameObject));
    }
}
