using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponIcon;
    [Header("Base Stats")] // REVIEW - DEFINIR SE TEM MAIS, SOBRE PREFAB OU SPRITES, SOUNDS VFX ETC
    [Tooltip("All Strategies use these base stats")]
    public float baseDamage;
    public float baseFireRate;
    public float baseRange;
    public int baseQuantity;
    [Tooltip("Delay between projectiles when quantity > 1 in seconds")]
    public float delayBetweenShots;

    [Header("Projectile Settings")]
    [Tooltip("Settings for projectile-based weapons")]
    public float projectileSpeed;
    public float spreadAngle;
    public GameObject projectilePrefab;

    [Header("Aoe Settings")]
    [Tooltip("Settings for area of effect weapons (Random and Targeted)")]
    public float aoeRadius;
    public GameObject aoePrefab;

    [Header("Cone Settings")]
    [Tooltip("Settings for frontal cone weapons")]
    public float coneAngle;
    public GameObject conePrefab;

    [Header("Strategy")]
    public ShootStrategyType shootStrategyType;


}

public enum ShootStrategyType
{
    Projectile,
    MeleeCircle,
    TargetedAoe,
    RandomAoe,
    FrontalCone,
}