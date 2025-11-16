using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static readonly WaitForSeconds _enemySpawnStagger = new(0.05f); //NOTE - these intervals can be adjusted as needed
    private static readonly WaitForSeconds _bossSpawnInterval = new(300f); // 5 minutes
    private static readonly WaitForSeconds _waveSpawnInterval = new(30f); // 30 seconds


    [Header("Enemy Spawning Settings")]
    [Tooltip("Array of enemy prefabs to spawn from this specific level")]
    [SerializeField] private GameObject[] enemyVariants;

    [Tooltip("Array of bosses prefabs to spawn from this specific level")]
    [SerializeField] private GameObject[] bossesVariants;

    [SerializeField] private float spawnRateIncrease = 0.05f;
    [SerializeField] private float maxSpawnRate = 0.25f;

    private float enemyPerSpawn;
    private int enemyInWave = 20;

    [Header("Spawn Timings")]
    [SerializeField] private float gameTime;
    [SerializeField] private float spawnInterval = 2f;


    [Header("Spawn Zone Settings")]
    [SerializeField] private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    private float minSpawnDistance;
    private int currentEnemyCount;
    [SerializeField, Range(1, 5000)] private int maxEnemyCount = 50;

    void OnEnable()
    {
        Enemy.OnEnemyDied += DecrementCount;
    }
    void OnDisable()
    {
        Enemy.OnEnemyDied -= DecrementCount;
    }
    void Awake()
    {
        mainCamera = Camera.main;
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        minSpawnDistance = Mathf.Sqrt(Mathf.Pow(cameraWidth / 2, 2) + Mathf.Pow(cameraHeight / 2, 2));

    }
    void Start()
    {
        StartCoroutine(EnemySpawnLoop());
        // StartCoroutine(WaveSpawnLoop());
        // StartCoroutine(BossSpawnLoop());
    }

    void Update()
    {
        gameTime = Time.time;
        if (spawnInterval > maxSpawnRate)
        {
            spawnInterval = Mathf.Max(maxSpawnRate, spawnInterval - Time.deltaTime * spawnRateIncrease); // diminui o intervalo até 0.25s no minimo
        }
        enemyPerSpawn = Mathf.Min(5, 1 + Mathf.Floor(gameTime / 30)); // aumenta o numero de inimigos por spawn a cada 30 segundos max de 5
    }

    private IEnumerator SpawnEnemy() // REVIEW - qnd nasce mais de 1 inimigo, eles nascem em filinha um atras do outro
    {
        if (currentEnemyCount >= maxEnemyCount)
        {
            yield break;
        }
        Vector2 spawnPosition = GetOffscreenSpawnPosition();
        int enemyIndex = Random.Range(0, enemyVariants.Length);
        float difficultyMod = DifficultyManager.Instance.GetCurrentDifficultyMultiplier();
        for (int i = 0; i < enemyPerSpawn; i++)
        {
            yield return _enemySpawnStagger; // small delay between spawns (tried to prevent lag spike)
            GameObject enemyObj = ObjectPoolManager.SpawnObject(enemyVariants[enemyIndex], spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
            
            if (enemyObj.TryGetComponent(out Enemy enemy))
            {
                enemy.Initialize(difficultyMod);
                currentEnemyCount++;
            }
        }
    }


    private IEnumerator EnemySpawnLoop()
    {
        while (true)
        {
            StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnBoss() // REVIEW - currently not used
    {
        Vector2 spawnPosition = GetOffscreenSpawnPosition();
        int bossIndex = Random.Range(0, bossesVariants.Length);
        Instantiate(bossesVariants[bossIndex], spawnPosition, Quaternion.identity);
    }

    private IEnumerator BossSpawnLoop()
    {
        while (true)
        {
            yield return _bossSpawnInterval; // spawn a boss every 5 minutes
            SpawnBoss();
        }
    }
    private void SpawnWave()
    {
        Debug.Log("Spawning Wave of " + enemyInWave + " enemies.");
        for (int i = 0; i < enemyInWave; i++) // spawn a wave of enemies: enemyInWave * enemyPerSpawn enemies?
        {
            StartCoroutine(SpawnEnemy());
        }
        enemyInWave += 5;
    }

    private IEnumerator WaveSpawnLoop()
    {
        while (true)
        {
            yield return _waveSpawnInterval; // spawn a wave every 30 seconds
            SpawnWave();
        }
    }

    private Vector2 GetOffscreenSpawnPosition()
    {
        Vector2 centerPos = mainCamera.transform.position;

        float spawnAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float spawnDistance = Random.Range(minSpawnDistance, cameraWidth);

        float cosSpawnAngle = Mathf.Cos(spawnAngle);
        float sinSpawnAngle = Mathf.Sin(spawnAngle);
        Vector2 spawnOffset = new Vector2(cosSpawnAngle, sinSpawnAngle) * spawnDistance;
        return centerPos + spawnOffset;
    }

    private void DecrementCount()
    {
        currentEnemyCount--;
    }
}


