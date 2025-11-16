using UnityEngine;

public class ProjectileShootStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        int quantity = context.weaponInstance.baseQuantity;
        float spreadAngle = context.weaponInstance.spreadAngle;
        GameObject projectilePrefab = context.weaponInstance.weaponData.projectilePrefab;
        if (projectilePrefab == null)
        {
            Debug.LogWarning("ProjectileShootStrategy: No projectile prefab assigned in WeaponData.");
            return;
        }
        for (int i = 0; i < quantity; i++)
        {
            float angleOffset = (i - (quantity - 1) / 2f) * spreadAngle;
            Vector2 modifiedDirection = RotateVector2D(angleOffset, context.direction);
            GameObject projectile = ObjectPoolManager.SpawnObject(projectilePrefab, context.spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
            if (!projectile.TryGetComponent<Projectile>(out var projComponent))
            {
                Debug.LogWarning("ProjectileShootStrategy: No Projectile component found on projectile prefab.");
                return;
            }
            projComponent.Initialize(modifiedDirection, context.weaponInstance.projectileSpeed, context.weaponInstance.baseDamage, context.targetLayer);

        }
    }
    private Vector2 RotateVector2D(float spreadAngle, Vector2 originalVector)
    {
        // formula da matrix de rotacao: Xr = Xcosθ - Ysinθ; Yr = Xsinθ + Ycosθ
        // vetor rotacionado = (Xr, Yr)
        float radAngle = spreadAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(radAngle);
        float sinAngle = Mathf.Sin(radAngle);
        float rotatedX = originalVector.x * cosAngle - originalVector.y * sinAngle;
        float rotatedY = originalVector.x * sinAngle + originalVector.y * cosAngle;
        Vector2 rotatedVector = new(rotatedX, rotatedY);
        return rotatedVector;
    }
}
