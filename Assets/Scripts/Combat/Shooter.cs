using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;

    private readonly List<IShootStrategy> strategies = new();
    [SerializeField] private List<WeaponData> weaponsData = new();
    private readonly List<float> nextFireTimes = new();

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

        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (Time.time >= nextFireTimes[i])
            {
                Transform targetEnemy = FindClosestEnemy(weaponsData[i].baseRange);
                if (targetEnemy != null)
                {

                    Shoot(targetEnemy.position, i);
                    Debug.Log("Shooter: Shooting with weapon " + weaponsData[i].weaponName);
                    nextFireTimes[i] = Time.time + 1f / weaponsData[i].baseFireRate; //REVIEW - se vai estar atirando no tempo corrento
                }
            }

        }

    }
    private void Shoot(Vector3 targetPosition, int weaponIndex)
    {
        if (strategies[weaponIndex] == null || weaponsData[weaponIndex] == null)
        {
            Debug.LogWarning("Shooter: No shooting strategy or weapon data assigned.");
            return;
        }
        int quantity = weaponsData[weaponIndex].baseQuantity;
        float delayBetweenShots = weaponsData[weaponIndex].delayBetweenShots;
        if (quantity >= 1 && delayBetweenShots > 0f)
        {
            StartCoroutine(ShootSequence(targetPosition, quantity, delayBetweenShots, weaponIndex));
        }
        else // se for quantity > 1 mas delayBetweenShots == 0
        {
            ExecuteShoot(targetPosition, weaponIndex);
        }

    }

    private IEnumerator ShootSequence(Vector3 targetPosition, int quantity, float delayBetweenShots, int weaponIndex)
    {
        for (int i = 0; i < quantity; i++)
        {
            ExecuteShoot(targetPosition, weaponIndex);
            if (i < quantity - 1) // evita esperar apos o ultimo tiro
            {
                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
    }

    private void ExecuteShoot(Vector3 targetPosition, int weaponIndex)
    {
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position; // esse shoot point pode ser a arma que tiver no ring magnetico ao redor da nave
        Vector3 direction = (targetPosition - spawnPos).normalized;
        ShootContext context = new()
        {
            targetPosition = (Vector2)targetPosition,
            spawnPosition = (Vector2)spawnPos,
            direction = (Vector2)direction,
            targetLayer = targetLayer,
            weaponData = weaponsData[weaponIndex],
            shooterTransform = transform
        };
        strategies[weaponIndex].Execute(context);
    }
    private void EquipWeapons(List<WeaponData> weapons)
    {
        foreach (WeaponData weapon in weapons)
        {
            IShootStrategy strategy = ShootStrategyFactory.GetStrategy(weapon.shootStrategyType);
            strategies.Add(strategy);
            nextFireTimes.Add(0f); //NOTE -  just for initialization?

        }

    }
    private Transform FindClosestEnemy(float weaponRange)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, weaponRange, targetLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in hitColliders)
        {
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