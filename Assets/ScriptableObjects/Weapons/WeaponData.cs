using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Base Stats")]
    [Tooltip("All Strategies use these base stats")]
    public string weaponName;
    public float baseDamage;
    public float baseFireRate;
    public float baseRange;

    [Header("Projectile Settings")]
    [Tooltip("Settings for projectile-based weapons")]
    public float projectileSpeed;
    public GameObject projectilePrefab;

    [Header("Aoe Settings")]
    [Tooltip("Settings for area of effect weapons (Random and Targeted)")]
    public float aoeRadius;
    public GameObject aoePrefab;

    [Header("Cone Settings")]
    [Tooltip("Settings for frontal cone weapons")]
    public float coneAngle;

    [Header("Strategy")]
    public ShootStrategyType shootStrategyType;


}

public enum ShootStrategyType
{
    Projectile,
    MeleeCircle,
    TargetedAOE,
    RandomAOE,
    FrontalCone,
}