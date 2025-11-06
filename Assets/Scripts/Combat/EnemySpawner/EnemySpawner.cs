using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_05 = new(0.05f);
    private static readonly WaitForSeconds _waitForSeconds30 = new(30f);
    private static readonly WaitForSeconds _waitForSeconds300 = new(300f);

    // periodic waves of enemies (every 30 seconds?) big wave of normal enemies
    // boss spawns at intervals (5min 10min 15min 20min last boss)
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
    [SerializeField] private float bossSpawnInterval = 300f; // 5 minutes
    [SerializeField] private float waveSpawnInterval = 30f; // 30 seconds   
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Zone Settings")]
    [SerializeField] private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    private float minSpawnDistance;


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
        StartCoroutine(WaveSpawnLoop());
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
        Vector2 spawnPosition = GetOffscreenSpawnPosition();
        int enemyIndex = Random.Range(0, enemyVariants.Length);
        for (int i = 0; i < enemyPerSpawn; i++)
        {
            yield return _waitForSeconds0_05; // small delay between spawns (tried to prevent lag spike)
            Instantiate(enemyVariants[enemyIndex], spawnPosition, Quaternion.identity);
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
            yield return _waitForSeconds300; // spawn a boss every 5 minutes
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
            yield return _waitForSeconds30; // spawn a wave every 30 seconds
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


}


