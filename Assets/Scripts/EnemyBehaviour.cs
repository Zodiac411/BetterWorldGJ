using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform primaryTarget;
    [SerializeField] private Transform secondaryTarget;
    [SerializeField] private Transform player;

    [Space][Header("Behaviour")]
    [SerializeField] private Transform newTarget;
    [SerializeField] private Transform currentTarget;
    [SerializeField, Range(0.0f, 10.0f)] private float playerInProximity;
    [SerializeField, Range(0.0f, 15.0f)] private float generatorProximity;    //proximity to make generator a priority
    [SerializeField, Range(0.0f, 20.0f)] private float attackRange = 5.0f;
    
    public bool Attacking
    {
        get
        {
            return attack;
        }
    }

    public Transform CurrentTarget
    {
        get
        {
            return currentTarget;   
        }
    }

    private NavMeshAgent agent;
    private Animator animate;
    public bool attack;
    public GameObject hand;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animate = GetComponent<Animator>();
        //hand.SetActive(false); 

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
            primaryTarget = baseObject.transform;

        if (agent != null && primaryTarget != null)
            agent.SetDestination(primaryTarget.position);
    }


    private void Update()
    {
        if (agent == null || animate == null)
            return;

        if(!attack)
        {
            newTarget = UpdateTarget();
            animate.SetBool("IsWalking", true);
        }

        if (currentTarget != newTarget)
        {
            ChangeTarget(newTarget);
        }

        attack = CanAttack();

        animate.SetBool("Attacking", attack);


        if(attack)
        {
            TryFacingAttackee();
        }


        if (currentTarget == primaryTarget)
            agent.stoppingDistance = attackRange * 0.95f;
        else
            agent.stoppingDistance = attackRange * 0.8f;

    }



    private Transform UpdateTarget()
    {
        Transform fallbackTarget = GetFallbackTarget();
        if (fallbackTarget == null)
            return null;

        Transform closePlayerTarget = GetPlayerTargetInRange();
        if (closePlayerTarget != null)
            return closePlayerTarget;

        Transform closestGeneratorTarget = GetClosestGeneratorTarget();
        if (closestGeneratorTarget != null)
            return closestGeneratorTarget;

        return fallbackTarget;
    }

    private Transform GetFallbackTarget()
    {
        if (primaryTarget != null)
            return primaryTarget;

        return player;
    }

    private Transform GetPlayerTargetInRange()
    {
        if (player == null)
            return null;

        if (Vector3.Distance(transform.position, player.position) < playerInProximity)
        {
            //print("Attack player");
            return player;
        }

        return null;
    }

    private Transform GetClosestGeneratorTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, generatorProximity);
        List<Transform> potentialTargets = GetGeneratorTargets(hitColliders);
        return ClosestTarget(potentialTargets);
    }

    private List<Transform> GetGeneratorTargets(Collider[] hitColliders)
    {
        List<Transform> potentialTargets = new List<Transform>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Generator"))
            {
                potentialTargets.Add(hitColliders[i].transform);
            }
        }

        return potentialTargets;
    }

    private void ChangeTarget(Transform newTarget)
    {
        if (newTarget == null)
        {
            agent.ResetPath();
            currentTarget = null;
            return;
        }

        agent.SetDestination(newTarget.position);

        currentTarget = newTarget;
    }


    private bool CanAttack()
    {
        bool readyToAttack = false;

        if(currentTarget != null)
        {
            if(Vector3.Distance(transform.position, currentTarget.position) < attackRange)
            {
                readyToAttack = true;
            }
        }

        return readyToAttack;
    }


    private Transform ClosestTarget(List<Transform> value)
    {
        if (value == null || value.Count == 0)
            return null;

        Transform closest = null;

        float distance = Mathf.Infinity;

            foreach (Transform t in value)
            {
                if (t == null)
                    continue;

                float tDistance = Vector3.Distance(transform.position, t.position);
                if (tDistance < distance)
                {
                    closest = t;
                    distance = tDistance;
                }
            }


        return closest;
    }

    private void TryFacingAttackee()
    {
        Vector3 directionTowards = currentTarget.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, directionTowards, 0.2f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerInProximity);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, generatorProximity);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
