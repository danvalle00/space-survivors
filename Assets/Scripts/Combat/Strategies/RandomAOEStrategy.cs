using UnityEngine;

public class RandomAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        // random point in the game world within a certain range from the player
        Vector2 playerPos = new(context.shooterTransform.position.x, context.shooterTransform.position.y);
        Vector2 randomOffset = Random.insideUnitCircle * context.weaponData.baseRange;
        Vector2 AOECenter = playerPos + randomOffset;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(AOECenter, context.weaponData.baseRange, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);
        }
    }
}