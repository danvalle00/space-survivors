using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 10000f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 playerDir;
    [SerializeField] private float speed = 2f;
    private Rigidbody2D enemyRb;
    void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRb.gravityScale = 0f;
    }
    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = FindFirstObjectByType<Player>().transform;
        }
        playerDir = (playerTransform.position - transform.position).normalized;

    }
    void FixedUpdate()
    {
        enemyRb.linearVelocity = playerDir * speed;
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
