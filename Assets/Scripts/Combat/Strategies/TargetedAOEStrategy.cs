using UnityEngine;

public class TargetedAOEStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        
        Vector2 enemyPos = context.targetPosition;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyPos, context.weaponData.baseRange, context.targetLayer);
        if (context.weaponData.aoePrefab != null)
        {
            GameObject aoeInstance = Object.Instantiate(context.weaponData.aoePrefab, enemyPos, Quaternion.identity);
            aoeInstance.transform.localScale = new Vector3(context.weaponData.aoeRadius * 2, context.weaponData.aoeRadius * 2);
            Object.Destroy(aoeInstance, 0.5f); // REVIEW - hardcoded destroy time ser baseado no tempo de animacao do AOE
        }
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);
            
        }
    }

}
