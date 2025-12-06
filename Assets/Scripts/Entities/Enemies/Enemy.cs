using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float contactDamage;
    [SerializeField] private float currentHealth;

    private Transform playerTransform;
    private Vector2 playerDir;
    private Rigidbody2D enemyRb;
    public static event System.Action OnEnemyDied;


    public void Initialize(float difficultyMultiplier)
    {
        maxHealth = enemyData.enemyBaseHealth * difficultyMultiplier;
        speed = enemyData.enemyBaseSpeed * difficultyMultiplier;
        contactDamage = enemyData.enemyBaseContactDamage * difficultyMultiplier;
        currentHealth = maxHealth;
    }


    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRb.gravityScale = 0f;
        Initialize(1f);

    }
    private void Start()
    {
        ObjectPoolManager.RegisterPreLoadedObject(this.gameObject, enemyData.enemyPrefab, ObjectPoolManager.PoolType.Enemies);
    }


    private void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = FindFirstObjectByType<Player>().transform;
        }
        playerDir = (playerTransform.position - transform.position).normalized;

    }
    private void FixedUpdate()
    {
        enemyRb.linearVelocity = playerDir * speed;
    }


    public void TakeDamage(float damageAmount)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        currentHealth -= damageAmount;
        if (!(currentHealth <= 0))
        {
            return;
        }

        ObjectPoolManager.ReturnToPool(this.gameObject, ObjectPoolManager.PoolType.Enemies);
        OnEnemyDied?.Invoke();
    }

}
