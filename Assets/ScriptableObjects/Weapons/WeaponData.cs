using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Base Stats")]
    public string weaponName;
    public float baseDamage;
    public float baseFireRate;
    public float baseRange;

    [Header("Projectile Settings")]
    public float projectileSpeed;
    public GameObject projectilePrefab;

    [Header("Aoe Settings")]
    public float aoeRadius;
    public GameObject aoePrefab;

    [Header("Cone Settings")]
    public float coneAngle;

    [Header("Strategy")]
    public ShootStrategyType shootStrategyType;


}

public enum ShootStrategyType
{
    Projectile,
    MeleeCircle,
    AOE,
    FrontalCone,
}