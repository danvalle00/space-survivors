using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;
    private IShootStrategy currentStrategy;
    private float nextFireTime;
    void Start()
    {
        if (weaponData == null)
        {
            Debug.LogError("Shooter: No WeaponData assigned.");
            return;
        }
        EquipWeapon(weaponData);
    }
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Transform targetEnemy = FindClosestEnemy();
            if (targetEnemy != null)
            {
                Shoot(targetEnemy.position);
            }
        }
    }
    public void Shoot(Vector3 targetPosition)
    {
        if (currentStrategy == null || weaponData == null)
        {
            Debug.LogWarning("Shooter: No shooting strategy or weapon data assigned.");
            return;
        }
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
        Vector3 direction = (targetPosition - spawnPos).normalized;
        ShootContext context = new()
        {
            spawnPosition = (Vector2)spawnPos,
            direction = (Vector2)direction,
            targetLayer = targetLayer,
            weaponData = weaponData,
            shooterTransform = transform
        };
        currentStrategy.Execute(context);
        nextFireTime = Time.time + 1f / weaponData.baseFireRate;
    }
    public void EquipWeapon(WeaponData weapon)
    {
        currentStrategy = ShootStrategyFactory.GetStrategy(weapon.shootStrategyType);
        weaponData = weapon;
    }
    private Transform FindClosestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, weaponData.baseRange, targetLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }
        return closestEnemy;
    }
}
