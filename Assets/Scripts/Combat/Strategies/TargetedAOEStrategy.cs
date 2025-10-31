using UnityEngine;

public class TargetedAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        // targeted aoe at a enemy position within a certain range from the player
        Vector2 playerPos = new(context.shooterTransform.position.x, context.shooterTransform.position.y);
        Vector2 enemyPos = context.direction * context.weaponData.throwRange;
        Vector2 AOECenter = playerPos + enemyPos; 


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(AOECenter, context.weaponData.aoeRadius, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);

        }
    }
}
