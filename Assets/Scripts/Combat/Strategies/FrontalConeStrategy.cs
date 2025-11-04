using UnityEngine;

public class FrontalConeStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        float halfConeAngleDegrees = context.weaponData.coneAngle / 2f;
        float dotThreshold = Mathf.Cos(halfConeAngleDegrees * Mathf.Deg2Rad);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(context.shooterTransform.position, context.weaponData.baseRange, context.targetLayer);

        foreach (Collider2D enemyCollider in hitColliders)
        {
            Vector2 dirToEnemy = (enemyCollider.transform.position - context.shooterTransform.position).normalized;
            float dotProduct = Vector2.Dot(context.direction, dirToEnemy);
            if (dotProduct >= dotThreshold)
            {
                IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
                damageable.TakeDamage(context.weaponData.baseDamage);
            }
        }

    }
}

