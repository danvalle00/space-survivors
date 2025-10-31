using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed");
        }
    }

}
