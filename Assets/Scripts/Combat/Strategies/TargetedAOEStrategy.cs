using UnityEngine;

public class TargetedAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        Vector2 enemyPos = context.targetPosition;
        GameObject aoePrefab = context.weaponInstance.weaponData.aoePrefab;
        if (aoePrefab == null)
        {
            Debug.LogWarning("TargetedAOEStrategy: No AOE prefab assigned in WeaponData.");
            return;
        }
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyPos, context.weaponInstance.baseRange, context.targetLayer);


        GameObject aoeInstance = Object.Instantiate(aoePrefab, enemyPos, Quaternion.identity);
        aoeInstance.transform.localScale = new Vector3(context.weaponInstance.aoeRadius * 2, context.weaponInstance.aoeRadius * 2);

        Object.Destroy(aoeInstance, 0.5f); // REVIEW - hardcoded destroy time ser baseado no tempo de animacao do AOE

        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponInstance.baseDamage);
        }
    }
}




