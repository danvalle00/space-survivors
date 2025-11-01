using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask targetLayer;
    

    public void Initialize(Vector2 direction, float speed, float damage, LayerMask targetLayer)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.targetLayer = targetLayer;
        Destroy(gameObject, 5f);

    }

    void Update()
    {
        transform.position += (Vector3)(speed * Time.deltaTime * direction);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null)
            {
                return;
            }
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
