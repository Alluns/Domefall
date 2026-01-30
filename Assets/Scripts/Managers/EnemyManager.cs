using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private GameObject shelter;
    private GameObject enemyPrefab;
    public List<GameObject> enemyPrefabs;
    public List<SpawnPosition> spawnPositions;

    [SerializeField] private int enemyCount = 4, waveNumber;
    [SerializeField] private float waveDuration, waveInterval;
    private float waveIntervalTimer;
    private bool waveOngoing;

    [System.Serializable]
    public class SpawnPosition
    {
        public Vector3 position;
        public NavMeshPath path;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception("2 singleton of the same [GamesManager exist]");
        }
        Instance = this;
    }

    private void Start()
    {
        shelter = GameObject.FindGameObjectWithTag("Shelter");
        CalculatePaths();
    }

    private void FixedUpdate()
    {
        if (!waveOngoing && waveNumber > 5)
        {
            GameManager.Instance.SwitchState(GameManager.GameState.Win);
        }
        if (waveIntervalTimer <= 0)
        {
            if (!waveOngoing)
            {
                enemyCount = CalcEnemyCount();
                StartCoroutine(SpawnEnemies());
                waveNumber++;

            }
        }
        else
            waveIntervalTimer -= Time.fixedDeltaTime;
    }
    private void CalculatePaths()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            SpawnPosition spawnPosition = spawnPositions[i];
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(spawnPosition.position, shelter.transform.position, NavMesh.AllAreas, path);
            spawnPosition.path = path;
        }
    }
    int CalcEnemyCount()
    {
        return enemyCount + waveNumber;

    }
    IEnumerator SpawnEnemies()
    {
        float interval = waveDuration / enemyCount;

        waveOngoing = true;

        for (int i = 0; i < enemyCount; ++i)
        {
            DecideNextEnemyType();
            SpawnEnemy();
            yield return new WaitForSeconds(interval);
        }
        waveIntervalTimer = waveInterval;
        waveOngoing = false;
    }

    private void SpawnEnemy()
    {
        SpawnPosition spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity, transform);
        NavMeshAgent enemyMeshAgent =  newEnemy.AddComponent<NavMeshAgent>();
        if (enemyMeshAgent.isOnNavMesh)
        {
            enemyMeshAgent.path = spawnPos.path;
            enemyMeshAgent.autoRepath = false;
        }
    }

    private void DecideNextEnemyType()
    {
        enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            Gizmos.color = Color.darkRed;
            Gizmos.DrawSphere(spawnPosition.position, 0.5f);
        }
    }
}