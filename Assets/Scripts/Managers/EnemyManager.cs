using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private GameObject shelter;
    public List<SpawnPosition> spawnPositions;
    public List<Wave> waves;

    private int waveNumber;
    private float spawnTimer;
    private float waveTimer;
    private Wave currentWave;

    [HideInInspector]public int enemiesAlive;

    [System.Serializable]
    public class SpawnPosition
    {
        public Vector3 position;
        public NavMeshPath path;
        [HideInInspector] public List<EnemyGroup> groups;
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyGroup> enemyGroups;
        public float spawnInterval;
        public float waitForNextWave;
        [HideInInspector]public int spawnCount;
        [HideInInspector]public int spawnQuota;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public int minEnemyCount;
        public int maxEnemyCount;
        [HideInInspector] public int enemyCount;
        public GameObject enemyPrefab;
        public int spawnPosition;
        [HideInInspector]public int spawnCount;
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
        StartNextWave();
    }

    private void Update()
    {
        
        if (currentWave.spawnCount < currentWave.spawnQuota)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnTimer = currentWave.spawnInterval;
                SpawnEnemy();
            }
        }
        else if (waveNumber < waves.Count)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0)
            {
                StartNextWave();
            }
        }
        else if (enemiesAlive == 0)
        {
            GameManager.Instance.saveData.levelsCompleted.Add(SceneManager.GetActiveScene().buildIndex);
            JsonSave.Save(GameManager.Instance.saveData);
            GameManager.Instance.SwitchState(GameManager.GameState.Win);
            
            gameObject.SetActive(false);
        }
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

    private void SpawnEnemy()
    {
        foreach (SpawnPosition spawn in spawnPositions)
        {
            foreach(EnemyGroup enemyGroup in spawn.groups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    GameObject newEnemy = Instantiate(enemyGroup.enemyPrefab, spawn.position, Quaternion.identity, transform);
                    enemiesAlive++;
                    currentWave.spawnCount++;
                    enemyGroup.spawnCount++;

                    NavMeshAgent enemyMeshAgent = newEnemy.AddComponent<NavMeshAgent>();
                    if (enemyMeshAgent.isOnNavMesh)
                    {
                        enemyMeshAgent.path = spawn.path;
                        enemyMeshAgent.autoRepath = false;
                    }
                }
            }
        }
    }
    private void CalculateSpawnQuota()
    {
        int spawnQuota = 0;
        foreach (EnemyGroup enemyGroup in currentWave.enemyGroups)
        {
            spawnQuota += enemyGroup.enemyCount;
        }
        currentWave.spawnQuota = spawnQuota;
    }

    private void AssignEnemyGroups()
    {
        foreach(SpawnPosition spawn in spawnPositions)
        {
            spawn.groups.Clear();
        }
        foreach(EnemyGroup enemyGroup in currentWave.enemyGroups)
        {
            int spawnPoint = Mathf.Min(enemyGroup.spawnPosition, spawnPositions.Count-1);
            if (spawnPoint < 0)
            {
                spawnPoint = Random.Range(0, spawnPositions.Count);
            }
            spawnPositions[spawnPoint].groups.Add(enemyGroup);
        }
    }

    private void DecideEnemyCounts()
    {
        foreach (EnemyGroup enemyGroup in currentWave.enemyGroups)
        {
            enemyGroup.enemyCount = Random.Range(enemyGroup.minEnemyCount, enemyGroup.maxEnemyCount + 1);
        }
    }


    private void StartNextWave()
    {
        currentWave = waves[waveNumber];
        DecideEnemyCounts();
        CalculateSpawnQuota();
        AssignEnemyGroups();
        waveTimer = currentWave.waitForNextWave;
        waveNumber++;
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