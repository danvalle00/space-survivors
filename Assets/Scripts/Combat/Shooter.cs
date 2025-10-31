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
    private void OnDrawGizmos()
    {
        if (weaponData == null) return;

        // Draw shooter position marker
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        // Draw strategy-specific visualizations
        switch (weaponData.shootStrategyType)
        {
            case ShootStrategyType.TargetedAOE:
                DrawTargetedAOEGizmos();
                break;
            case ShootStrategyType.RandomAOE:
                DrawRandomAOEGizmos();
                break;
            default:
                // For other strategies, just show detection range
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, weaponData.baseRange);
                break;
        }

        // In Play mode, draw line to current target
        if (Application.isPlaying)
        {
            Transform target = FindClosestEnemy();
            if (target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.position);
                Gizmos.DrawWireSphere(target.position, 0.3f);
            }
        }
    }

    private void DrawTargetedAOEGizmos()
    {
        // Throw range (detection range for finding target)
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, weaponData.baseRange);

        Vector2 playerPos = (Vector2)transform.position;
        Vector2 aoeCenter;

        // In Play mode, show where the AOE lands on actual target
        if (Application.isPlaying)
        {
            Transform target = FindClosestEnemy();
            if (target != null)
            {
                aoeCenter = (Vector2)target.position;

                // Draw line to target
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, aoeCenter);
            }
            else
            {
                // No target, show preview at max range
                aoeCenter = playerPos + (Vector2)transform.up * weaponData.baseRange;
            }
        }
        else
        {
            // Edit mode: show preview at baseRange in forward direction
            aoeCenter = playerPos + (Vector2)transform.up * weaponData.baseRange;
        }

        // Draw AOE blast radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(aoeCenter, weaponData.aoeRadius);
    }

    private void DrawRandomAOEGizmos()
    {
        // Throw range (where random AOEs can spawn)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, weaponData.baseRange);

        // Show sample AOE positions (4 examples around the player)
        Gizmos.color = new Color(1f, 0f, 1f, 0.6f); // Magenta with transparency
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90f;
            float radians = angle * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * (weaponData.baseRange * 0.7f);
            Vector2 samplePos = (Vector2)transform.position + offset;

            // Draw sample AOE blast radius
            Gizmos.DrawWireSphere(samplePos, weaponData.aoeRadius);
        }
    }
}