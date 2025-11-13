using UnityEngine;

public class RandomAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        GameObject aoePrefab = context.weaponInstance.weaponData.aoePrefab;
        if (aoePrefab == null)
        {
            Debug.LogWarning("RandomAOEStrategy: No AOE prefab assigned in WeaponData.");
            return;
        }
        Vector2 playerPos = new(context.shooterTransform.position.x, context.shooterTransform.position.y);
        Vector2 randomOffset = Random.insideUnitCircle * context.weaponInstance.baseRange;
        Vector2 AOECenter = playerPos + randomOffset;

        GameObject aoeInstance = Object.Instantiate(aoePrefab, AOECenter, Quaternion.identity);
        aoeInstance.transform.localScale = new Vector3(context.weaponInstance.aoeRadius * 2, context.weaponInstance.aoeRadius * 2);
        Object.Destroy(aoeInstance, 2f); // REVIEW - hardcoded destroy time ser baseado no tempo de animacao do AOE

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(AOECenter, context.weaponInstance.aoeRadius, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponInstance.baseDamage);
        }
    }
}

