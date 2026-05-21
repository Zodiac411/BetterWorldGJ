using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private WaveDefinition waveDefinition;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private BaseScript baseScript;
    [SerializeField, Range(0f, 200f)] private float outerSpawnRadius = 10f;
    [SerializeField] private float innerSpawnRadius = 5f;
    [SerializeField] private bool adjustInnerRadiusWithBase = true;
    [SerializeField, Range(0f, 100f)] private float upwardOffset = 50f;

    private int currentWave;
    private int currentSpawnCount;
    private float spawnTimer;
    private bool spawnBurstInProgress;

    public int WaveNumber
    {
        get => waveNumber;
        set => waveNumber = value;
    }

    private void Start()
    {
        if (baseTransform != null)
        {
            transform.position = baseTransform.position + new Vector3(0f, 2f, 0f);
        }

        RefreshInnerRadius();
        RecalculateSpawnCount();
    }

    private void Update()
    {
        if (currentWave != waveNumber)
        {
            currentWave = waveNumber;
            RecalculateSpawnCount();
        }

        RefreshInnerRadius();

        float interval = waveDefinition != null ? waveDefinition.spawnInterval : 2f;
        spawnTimer += Time.deltaTime;
        if (spawnTimer > interval && !spawnBurstInProgress)
        {
            StartCoroutine(SpawnBurst());
            spawnTimer = 0f;
        }
    }

    private IEnumerator SpawnBurst()
    {
        spawnBurstInProgress = true;
        int burstCount = Mathf.Max(1, currentSpawnCount);
        float delay = waveDefinition != null ? waveDefinition.delayBetweenSpawnsInBurst : 0.7f;

        for (int i = 0; i < burstCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(delay);
        }

        spawnBurstInProgress = false;
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            return;
        }

        Vector3 spawnLocation = CalculateSpawnLocation();
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);

        if (NavMesh.SamplePosition(spawnLocation, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            spawnedEnemy.transform.position = hit.position;
        }

        spawnedEnemy.transform.SetParent(transform, true);

        if (!spawnedEnemy.TryGetComponent<EnemyRegistration>(out _))
        {
            spawnedEnemy.AddComponent<EnemyRegistration>();
        }
    }

    private Vector3 CalculateSpawnLocation()
    {
        float locationOffsetX = Random.Range(innerSpawnRadius, outerSpawnRadius);
        float locationOffsetZ = Random.Range(innerSpawnRadius, outerSpawnRadius);

        int offsetXDir = Random.Range(-1, 2);
        int offsetZDir = Random.Range(-1, 2);
        if (offsetXDir == 0 && offsetZDir == 0)
        {
            if (Random.Range(0, 100) % 2 == 0)
            {
                offsetXDir = 1;
            }
            else
            {
                offsetZDir = 1;
            }
        }

        locationOffsetX *= offsetXDir;
        locationOffsetZ *= offsetZDir;

        Vector3 locationOffset = new Vector3(locationOffsetX, 0f, locationOffsetZ);
        Vector3 spawnLocation = locationOffset + transform.position;

        if (Vector3.Distance(spawnLocation, transform.position) > outerSpawnRadius)
        {
            float damping = 1.5f;
            float extraDistance = (Vector3.Distance(spawnLocation, transform.position) - outerSpawnRadius) + damping;
            Vector3 directionToCenter = transform.position - spawnLocation;
            spawnLocation += directionToCenter.normalized * extraDistance;
        }

        return new Vector3(spawnLocation.x, spawnLocation.y + upwardOffset, spawnLocation.z);
    }

    private void RefreshInnerRadius()
    {
        if (!adjustInnerRadiusWithBase || baseScript == null)
        {
            return;
        }

        innerSpawnRadius = (baseScript.placementRadius * 100f) / 5f;
    }

    private void RecalculateSpawnCount()
    {
        int minEnemies = waveDefinition != null ? waveDefinition.minEnemiesPerBurst : 1;
        int a;
        int b;

        if (minEnemies % 2 == 1)
        {
            a = (minEnemies - 1) / 2;
            b = a + 1;
        }
        else
        {
            a = b = minEnemies / 2;
        }

        currentSpawnCount = a * waveNumber + b;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerSpawnRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, innerSpawnRadius);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, upwardOffset, 0f));
    }
}
