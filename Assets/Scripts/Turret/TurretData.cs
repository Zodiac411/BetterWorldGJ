using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "BetterWorldGJ/Turret Definition")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public Sprite icon;
    [SerializeField] private Sprite iconFallback;

    public Sprite DisplayIcon => icon != null ? icon : iconFallback;
    public int id;
    public GameObject prefab;
    public GameObject hologramPrefab;
    [Min(0)] public int buildCost = 200;
    [Min(0f)] public float maxHealth = 100f;
    [Min(0f)] public float attackRange = 10f;
    [Min(0f)] public float fireRate = 1f;
    [Min(0f)] public float bulletSpeed = 20f;
    [Min(0f)] public float bulletDamage = 10f;
    public GameObject bulletPrefab;
    public TurretData upgradeTarget;
}
