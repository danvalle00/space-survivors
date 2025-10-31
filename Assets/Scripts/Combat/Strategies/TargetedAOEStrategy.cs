using UnityEngine;

public class TargetedAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        // targeted aoe at a enemy position if he is in range
        Vector2 enemyPos = context.direction;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyPos, context.weaponData.baseRange, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);
        }

    }
    
}
