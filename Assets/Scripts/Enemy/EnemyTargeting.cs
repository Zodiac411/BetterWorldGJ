using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyTargeting : MonoBehaviour
{
    [FormerlySerializedAs("primaryTarget")]
    [SerializeField] private Transform primaryTarget;

    [FormerlySerializedAs("secondaryTarget")]
    [SerializeField] private Transform secondaryTarget;

    [FormerlySerializedAs("player")]
    [SerializeField] private Transform player;

    [FormerlySerializedAs("playerInProximity")]
    [SerializeField, Range(0f, 10f)] private float playerInProximity = 3f;

    [FormerlySerializedAs("generatorProximity")]
    [SerializeField, Range(0f, 15f)] private float generatorProximity = 5f;

    public Transform CurrentTarget { get; private set; }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            primaryTarget = baseObject.transform;
        }

        CurrentTarget = GetFallbackTarget();
    }

    public Transform SelectTarget()
    {
        Transform fallbackTarget = GetFallbackTarget();
        if (fallbackTarget == null)
        {
            CurrentTarget = null;
            return null;
        }

        Transform closePlayerTarget = GetPlayerTargetInRange();
        if (closePlayerTarget != null)
        {
            CurrentTarget = closePlayerTarget;
            return CurrentTarget;
        }

        Transform closestGeneratorTarget = GetClosestGeneratorTarget();
        if (closestGeneratorTarget != null)
        {
            CurrentTarget = closestGeneratorTarget;
            return CurrentTarget;
        }

        CurrentTarget = fallbackTarget;
        return CurrentTarget;
    }

    public bool IsPrimaryTarget(Transform target)
    {
        return target == primaryTarget;
    }

    private Transform GetFallbackTarget()
    {
        if (primaryTarget != null)
        {
            return primaryTarget;
        }

        return player;
    }

    private Transform GetPlayerTargetInRange()
    {
        if (player == null)
        {
            return null;
        }

        if (Vector3.Distance(transform.position, player.position) < playerInProximity)
        {
            return player;
        }

        return null;
    }

    private Transform GetClosestGeneratorTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, generatorProximity);
        List<Transform> potentialTargets = new List<Transform>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Generator"))
            {
                potentialTargets.Add(hitColliders[i].transform);
            }
        }

        return ClosestTarget(potentialTargets);
    }

    private Transform ClosestTarget(List<Transform> value)
    {
        if (value == null || value.Count == 0)
        {
            return null;
        }

        Transform closest = null;
        float distance = Mathf.Infinity;

        foreach (Transform candidate in value)
        {
            if (candidate == null)
            {
                continue;
            }

            float candidateDistance = Vector3.Distance(transform.position, candidate.position);
            if (candidateDistance < distance)
            {
                closest = candidate;
                distance = candidateDistance;
            }
        }

        return closest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerInProximity);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, generatorProximity);
    }
}
