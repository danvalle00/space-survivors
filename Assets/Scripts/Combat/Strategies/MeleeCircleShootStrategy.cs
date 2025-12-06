using UnityEngine;

public class MeleeCircleShootStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(context.shooterTransform.position, context.weaponInstance.GetStat(WeaponStatType.Range), context.targetLayer); // um circulo ao redor to player
        float damage = context.weaponInstance.RollDamage();
        DrawDebug(context); // Desenha o circulo de alcance para debug
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(damage);
        }
    }

    private static void DrawCircle(Vector3 center, float radius, Color color, float duration)
    {
        const int segments = 32;
        const float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius, 0f);
            Vector3 point2 = center + new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius, 0f);

            Debug.DrawLine(point1, point2, color, duration);
        }
    }

    private static void DrawDebug(ShootContext context)
    {
        DrawCircle(context.shooterTransform.position, context.weaponInstance.GetStat(WeaponStatType.Range), Color.red, 5f);
    }
}