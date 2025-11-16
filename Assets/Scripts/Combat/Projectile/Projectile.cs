using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{

    private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask targetLayer;
    private bool isReturningToPool = false;
    private readonly WaitForSeconds returnDelay = new(10f); // Need find a number that the projectile will surely be out of screen


    public void Initialize(Vector2 direction, float speed, float damage, LayerMask targetLayer)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.targetLayer = targetLayer;
    }
    void OnEnable()
    {
        isReturningToPool = false;
        StartCoroutine(ReturnToPoolAfterTime());
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        yield return returnDelay;
        ReturnToPool();
    }
    void Update()
    {
        transform.position += (Vector3)(speed * Time.deltaTime * direction);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (!other.TryGetComponent<IDamageable>(out var damageable))
            {
                return;
            }
            damageable.TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isReturningToPool) return;
        isReturningToPool = true;
        StopAllCoroutines();
        ObjectPoolManager.ReturnToPool(this.gameObject, ObjectPoolManager.PoolType.Projectiles);
    }
}
