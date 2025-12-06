using UnityEngine;

public class FrontalConeStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        float halfConeAngleDegrees = context.weaponInstance.GetStat(WeaponStatType.ConeAngle) / 2f;
        float dotThreshold = Mathf.Cos(halfConeAngleDegrees * Mathf.Deg2Rad);
        float damage = context.weaponInstance.RollDamage();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(context.shooterTransform.position, context.weaponInstance.GetStat(WeaponStatType.Range), context.targetLayer);

        DrawConeDebug(context, halfConeAngleDegrees);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            Vector2 dirToEnemy = (enemyCollider.transform.position - context.shooterTransform.position).normalized;
            float dotProduct = Vector2.Dot(context.direction, dirToEnemy);
            if (!(dotProduct >= dotThreshold))
            {
                continue;
            }

            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(damage);
        }

    }
    private void DrawConeDebug(ShootContext context, float halfConeAngleDegrees)
    {
        Vector3 shooterPos = context.shooterTransform.position;
        float range = context.weaponInstance.GetStat(WeaponStatType.Range);

        // Calculate and draw cone edges
        Vector2 leftEdge = RotateVector(context.direction, halfConeAngleDegrees);
        Vector2 rightEdge = RotateVector(context.direction, -halfConeAngleDegrees);

        Debug.DrawRay(shooterPos, leftEdge * range, Color.yellow, 5f);
        Debug.DrawRay(shooterPos, rightEdge * range, Color.yellow, 5f);
        Debug.DrawRay(shooterPos, context.direction * range, Color.cyan, 5f);

        // Draw range circle
        DrawCircle(shooterPos, range, Color.blue, 5f);
    }
    private void DrawCircle(Vector3 center, float radius, Color color, float duration)
    {
        int segments = 32;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius, 0f);
            Vector3 point2 = center + new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius, 0f);

            Debug.DrawLine(point1, point2, color, duration);
        }
    }
    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

}

