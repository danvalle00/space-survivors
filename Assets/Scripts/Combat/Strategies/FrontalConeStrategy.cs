using UnityEngine;

public class FrontalConeStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        float halfConeAngleDegrees = context.weaponData.coneAngle / 2f;
        float dotThreshold = Mathf.Cos(halfConeAngleDegrees * Mathf.Deg2Rad);
        Vector2 playerPos = context.shooterTransform.position;

        
        Vector2 leftEdge = RotateVector(context.direction, halfConeAngleDegrees);
        Vector2 rightEdge = RotateVector(context.direction, -halfConeAngleDegrees);

        Debug.DrawLine(playerPos, playerPos + leftEdge * context.weaponData.baseRange, Color.yellow, 1f);
        Debug.DrawLine(playerPos, playerPos + rightEdge * context.weaponData.baseRange, Color.yellow, 1f);
        Debug.DrawLine(playerPos, playerPos + context.direction * context.weaponData.baseRange, Color.red, 1f);



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
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}

