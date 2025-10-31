using UnityEngine;

public class MeleeCircleShootStrategy : IShootStrategy
{
    public void Execute(ShootContext context)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(context.shooterTransform.position, context.weaponData.baseRange, context.targetLayer); // um circulo ao redor to player
        foreach (Collider2D enemyCollider in hitColliders)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();
            damageable.TakeDamage(context.weaponData.baseDamage);
            // se for null vai dar problema mas  
            // n eh pra ter nulls aqui
        }
    }
}