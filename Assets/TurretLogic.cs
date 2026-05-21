using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    [SerializeField] private TurretData turretDefinition;
    [SerializeField] private Attacker attacker;
    [SerializeField] private Damagable damageable;
    [SerializeField] private TurretHealth turretHealth;

    public TurretData Definition => turretDefinition;
    public int BuildCost => turretDefinition != null ? turretDefinition.buildCost : 0;
    public GameObject Prefab => turretDefinition != null ? turretDefinition.prefab : null;
    public GameObject HologramPrefab => turretDefinition != null ? turretDefinition.hologramPrefab : null;

    private void Awake()
    {
        if (attacker == null)
        {
            attacker = GetComponent<Attacker>();
        }

        if (damageable == null)
        {
            damageable = GetComponent<Damagable>();
        }

        if (turretHealth == null)
        {
            turretHealth = GetComponent<TurretHealth>();
        }

        ApplyDefinition();
    }

    public void ApplyDefinition(TurretData definition = null)
    {
        if (definition != null)
        {
            turretDefinition = definition;
        }

        if (turretDefinition == null)
        {
            return;
        }

        if (attacker != null)
        {
            attacker.ApplyDefinition(turretDefinition);
        }

        if (turretHealth != null)
        {
            turretHealth.Configure(turretDefinition.maxHealth);

            if (damageable != null)
            {
                Destroy(damageable);
                damageable = null;
            }
        }
    }
}
