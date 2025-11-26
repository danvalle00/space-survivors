using UnityEngine;

public class RandomAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        float aoeRadius = context.weaponInstance.GetStat(WeaponStatType.AoeRadius);
        float damage = context.weaponInstance.RollDamage();
        GameObject aoePrefab = context.weaponInstance.weaponData.aoePrefab;
        if (aoePrefab == null)
        {
            Debug.LogWarning("RandomAOEStrategy: No AOE prefab assigned in WeaponData.");
            return;
        }

        Vector2 playerPos = new(context.shooterTransform.position.x, context.shooterTransform.position.y);
        Vector2 randomOffset = Random.insideUnitCircle * aoeRadius;
        Vector2 AOECenter = playerPos + randomOffset;


        GameObject aoeInstance = Object.Instantiate(aoePrefab, AOECenter, Quaternion.identity);
        aoeInstance.transform.localScale = new Vector3(aoeRadius * 2, aoeRadius * 2);
        Object.Destroy(aoeInstance, 2f); // REVIEW - hardcoded destroy time ser baseado no tempo de animacao do AOE

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(AOECenter, aoeRadius, context.targetLayer);
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(damage);
        }
    }
}

