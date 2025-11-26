using UnityEngine;

public class TargetedAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        float aoeRadius = context.weaponInstance.GetStat(WeaponStatType.AoeRadius);
        Vector2 enemyPos = context.targetPosition;
        GameObject aoePrefab = context.weaponInstance.weaponData.aoePrefab;
        if (aoePrefab == null)
        {
            Debug.LogWarning("TargetedAOEStrategy: No AOE prefab assigned in WeaponData.");
            return;
        }
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyPos, context.weaponInstance.GetStat(WeaponStatType.Range), context.targetLayer);
        float damage = context.weaponInstance.RollDamage();

        GameObject aoeInstance = Object.Instantiate(aoePrefab, enemyPos, Quaternion.identity);
        aoeInstance.transform.localScale = new Vector3(aoeRadius * 2, aoeRadius * 2);

        Object.Destroy(aoeInstance, 0.5f); // REVIEW - hardcoded destroy time ser baseado no tempo de animacao do AOE

        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(damage);
        }
    }
}




