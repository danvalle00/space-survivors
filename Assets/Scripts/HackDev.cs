using UnityEngine;
public class HackDev : MonoBehaviour
{
    [Header("Live Stats Display")]
    [SerializeField] private float currentDamage;
    [SerializeField] private float currentCritChance;
    [SerializeField] private float currentCritDamage;
    [SerializeField] private float currentFireRate;
    [SerializeField] private float currentProjectileSpeed;
    [SerializeField] private int currentQuantity;
    [SerializeField] private float currentAoeRadius;


    [Header("Stats Modifications")]
    [SerializeField] private float critChanceToAdd = 20f;
    [SerializeField] private float critDamageToAdd = 50f;
    [SerializeField] private float damageMultiplierToAdd = 1f;
    [SerializeField] private float fireRateMultiplierToAdd = 0.5f;
    [SerializeField] private float projectileSpeedMultiplierToAdd = 0.5f;
    [SerializeField] private int quantityToAdd = 1;
    [SerializeField] private float aoeRadiusToAdd = 0.5f;


    [Header("Overrides")] [SerializeField] private bool overrideStats;
    [SerializeField] private float overrideDamage = 10f;
    [SerializeField] private float overrideCritChance = 100f;
    [SerializeField] private float overrideCritDamage = 9999f;

}