using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject source;

    private float timer;

    private void Start()
    {
        timer = lifetime;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Configure(float bulletDamage, GameObject damageSource)
    {
        damage = bulletDamage;
        source = damageSource;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
        {
            return;
        }

        if (collision.collider.TryGetComponent(out IDamageable damageable))
        {
            DamageContext context = new DamageContext(
                damage,
                source != null ? source : gameObject,
                collision.contacts.Length > 0 ? collision.contacts[0].point : collision.transform.position,
                collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector3.up);
            damageable.ApplyDamage(in context);
        }

        Destroy(gameObject);
    }
}
