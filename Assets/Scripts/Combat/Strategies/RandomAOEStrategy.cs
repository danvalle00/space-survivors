using UnityEngine;

public class RandomAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        GameObject aoePrefab = context.weaponData.aoePrefab;
        Vector2 playerPos = new(context.shooterTransform.position.x, context.shooterTransform.position.y);
        Vector2 randomOffset = Random.insideUnitCircle * context.weaponData.baseRange;
        Vector2 AOECenter = playerPos + randomOffset;

        if (context.weaponData.aoePrefab != null)
        {
            GameObject aoeInstance = Object.Instantiate(aoePrefab, AOECenter, Quaternion.identity);
            aoeInstance.transform.localScale = new Vector3(context.weaponData.aoeRadius * 2, context.weaponData.aoeRadius * 2);
            Object.Destroy(aoeInstance, 3f);
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(AOECenter, context.weaponData.aoeRadius, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);
        }
    }
}

