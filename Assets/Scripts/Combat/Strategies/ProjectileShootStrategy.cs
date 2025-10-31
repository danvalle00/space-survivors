using UnityEngine;

public class ProjectileShootStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        if (context.weaponData.projectilePrefab == null)
        {
            Debug.LogWarning("ProjectileShootStrategy: No projectile prefab assigned in WeaponData.");
            return;
        }
        GameObject projectile = Object.Instantiate(context.weaponData.projectilePrefab, context.spawnPosition, Quaternion.identity);
        Projectile projComponent = projectile.GetComponent<Projectile>();
        if (projComponent == null)
        {
            Debug.LogWarning("ProjectileShootStrategy: No Projectile component found on projectile prefab.");
            return;
        }
        projComponent.Initialize(context.direction, context.weaponData.projectileSpeed, context.weaponData.baseDamage, context.targetLayer);
    }
}
