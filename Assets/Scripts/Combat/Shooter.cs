using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;

    private readonly SpaceshipData spaceship;
    [SerializeField] private readonly List<WeaponSlot> weaponsSlots = new(6);
    [SerializeField] private List<WeaponData> weaponsData = new();

    void Start()
    {
        if (weaponsData == null || weaponsData.Count == 0)
        {
            Debug.LogError("Shooter: No WeaponData assigned.");
            return;
        }
        EquipWeapons(weaponsData);
    }
    void Update() // manter shoot como atirar 1 arma e aqui eu faço o loop para todas as armas
    {

        for (int i = 0; i < weaponsSlots.Count; i++)
        {
            WeaponSlot index = weaponsSlots[i];
            if (Time.time >= index.nextFireTime)
            {
                Transform targetEnemy = FindClosestEnemy(index.data.baseRange);
                if (targetEnemy != null)
                {
                    Shoot(targetEnemy.position, index);
                    index.nextFireTime = Time.time + 1f / index.data.baseFireRate;
                }
            }

        }

    }
    private void Shoot(Vector3 targetPosition, WeaponSlot weaponIndex)
    {
        if (weaponIndex.strategy == null || weaponIndex.data == null)
        {
            Debug.LogWarning("Shooter: No shooting strategy or weapon data assigned.");
            return;
        }
        if (weaponIndex.instance.baseQuantity >= 1 && weaponIndex.instance.delayBetweenShots > 0f)
        {
            StartCoroutine(ShootSequence(targetPosition, weaponIndex));
        }
        else // se for quantity > 1 mas delayBetweenShots == 0
        {
            ExecuteShoot(targetPosition, weaponIndex);
        }

    }

    private IEnumerator ShootSequence(Vector3 targetPosition, WeaponSlot weaponIndex)
    {
        for (int i = 0; i < weaponIndex.instance.baseQuantity; i++)
        {
            ExecuteShoot(targetPosition, weaponIndex);
            if (i < weaponIndex.instance.baseQuantity - 1) // evita esperar apos o ultimo tiro
            {
                yield return new WaitForSeconds(weaponIndex.instance.delayBetweenShots);
            }
        }
    }

    private void ExecuteShoot(Vector3 targetPosition, WeaponSlot weaponIndex)
    {
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position; // esse shoot point pode ser a arma que tiver no ring magnetico ao redor da nave
        Vector3 direction = (targetPosition - spawnPos).normalized;
        ShootContext context = new()
        {
            targetPosition = (Vector2)targetPosition,
            spawnPosition = (Vector2)spawnPos,
            direction = (Vector2)direction,
            targetLayer = targetLayer,
            weaponInstance = weaponIndex.instance,
            shooterTransform = transform
        };

        weaponIndex.strategy.Execute(context);
    }
    private void EquipWeapons(List<WeaponData> weaponsTemplates)
    {
        foreach (WeaponData weapon in weaponsTemplates)
        {
            WeaponSlot slot = new()
            {
                data = weapon,
                instance = spaceship ? new WeaponInstance(weapon, spaceship) : new WeaponInstance(weapon),
                strategy = ShootStrategyFactory.GetStrategy(weapon.shootStrategyType),
                nextFireTime = 0f
            };
            weaponsSlots.Add(slot);
        }


    }
    private Transform FindClosestEnemy(float weaponRange)
    {
        // REVIEW - trocar OverlapCircleAll por OverlapCircle para performance 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, weaponRange, targetLayer);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in hitColliders)
        {
            if (!collider.gameObject.activeInHierarchy) // ← Add this check
            {
                Debug.Log($"WARNING: Found inactive enemy {collider.gameObject.name} in targeting!");
                continue; // Skip inactive enemies
            }
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }
        return closestEnemy;
    }
}