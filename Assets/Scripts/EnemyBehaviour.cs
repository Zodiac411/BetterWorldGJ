using UnityEngine;

[RequireComponent(typeof(EnemyTargeting))]
[RequireComponent(typeof(EnemyLocomotion))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyBehaviour : MonoBehaviour
{
    private EnemyTargeting targeting;
    private EnemyLocomotion locomotion;
    private EnemyAttack attack;

    public bool Attacking => attack != null && attack.IsAttacking;
    public Transform CurrentTarget => targeting != null ? targeting.CurrentTarget : null;

    private void Awake()
    {
        targeting = GetComponent<EnemyTargeting>();
        locomotion = GetComponent<EnemyLocomotion>();
        attack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        Transform initialTarget = targeting.SelectTarget();
        locomotion.SetInitialDestination(initialTarget);
    }

    private void Update()
    {
        Transform selectedTarget = targeting.SelectTarget();
        bool isAttacking = attack.TickAttack(selectedTarget);
        locomotion.TickMovement(selectedTarget, isAttacking);
    }
}
