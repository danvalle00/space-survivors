using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // increase spawn rate over time
    // only spawn offscreen enemies 
    // think of different enemy types to spawn
    // periodic waves of enemies (every 30 seconds?) big wave of normal enemies
    // boss spawns at intervals (5min 10min 15min 20min last boss)
    // a camera sempre segue o player entao spawnar inimigos fora da view da camera
    [Header("Enemy Spawning Settings")]
    [SerializeField] private GameObject[] enemyVariants;
    [SerializeField] private GameObject[] bossesVariants;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Zone Settings")]
    [SerializeField] private Camera mainCamera;
    private float cameraHeight;
    private float cameraWidth;
    void Awake()
    {
        mainCamera = Camera.main;
        cameraHeight = 2f * mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;

    }
    void Start()
    {
        StartCoroutine(EnemySpawnLoop());
    }

    void Update()
    {
        spawnInterval = Mathf.Max(0.25f, spawnInterval - Time.deltaTime * 0.05f); // diminui o intervalo até 0.25s no minimo
    }
    private void SpawnEnemy()
    {
        Vector2 spawnPosition = GetOffscreenSpawnPosition();
        int enemyIndex = Random.Range(0, enemyVariants.Length);
        Instantiate(enemyVariants[enemyIndex], spawnPosition, Quaternion.identity);
    }

    private IEnumerator EnemySpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private Vector2 GetOffscreenSpawnPosition()
    {
    Vector2 spawnPosition = mainCamera.transform.position;
        int spawnEdge = Random.Range(0, 4);
        float spawnMargin = Random.Range(1f, 20f); // distancia extra fora da tela

        switch (spawnEdge)
        {
            case 0: // top
                spawnPosition.x += Random.Range(-cameraWidth / 2f, cameraWidth / 2f); // 
                spawnPosition.y += (cameraHeight / 2f) + spawnMargin;
                break;
            case 1: // right
                spawnPosition.x += (cameraWidth / 2f) + spawnMargin;
                spawnPosition.y += Random.Range(-cameraHeight / 2f, cameraHeight / 2f);
                break;
            case 2: // bottom
                spawnPosition.x += Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
                spawnPosition.y -= (cameraHeight / 2f) + spawnMargin;
                break;
            case 3: // left
                spawnPosition.x -= (cameraWidth / 2f) + spawnMargin;
                spawnPosition.y += Random.Range(-cameraHeight / 2f, cameraHeight / 2f);
                break;
        }
        return spawnPosition;
    }
}

    
