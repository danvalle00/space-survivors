using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;
    public string enemyDescription;
    public GameObject enemyPrefab; // maybe change for sprite later
    public float enemyBaseHealth = 100f;
    public float enemyBaseSpeed = 2f;
    public float enemyBaseArmor = 0f;
    public float enemyBaseContactDamage = 10f;
    [Header("Ranged Enemy Stats")]
    public float enemyProjectileSpeed = 5f;
    public float enemyProjectileDamage = 10f;
    public float enemyProjectileFireRate = 1f;
    public float enemyProjectileRange = 10f;
}
