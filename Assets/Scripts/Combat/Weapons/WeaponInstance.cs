using UnityEngine;

public class WeaponInstance
{
    public WeaponData weaponData;
    public PlayerStatsInstance PlayerStats => GameManager.Instance != null ? GameManager.Instance.PlayerStatsInstance : null;
    public float delayBetweenShots;
    public float baseDamage;
    public float critChance;
    public float critDamage;
    public float baseFireRate;
    public float baseRange;
    public int baseQuantity;
    public float projectileSpeed;
    public float aoeRadius;
    public float coneAngle;
    public float spreadAngle;

    public WeaponInstance(WeaponData weaponData, bool isPlayer)
    {
        if (isPlayer)
        {
            InitializeAsPlayerWeapon(weaponData);
            return;
        }
        InitializeWeaponInstance(weaponData);

    }

    public float RollDamage()
    {
        bool isCrit = Random.value <= critChance;
        if (isCrit)
        {
            Debug.Log("Critical Hit! " + critDamage + "x damage.");
            return baseDamage * critDamage;
        }
        return baseDamage;
    }
    private void InitializeAsPlayerWeapon(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        delayBetweenShots = weaponData.delayBetweenShots;
        baseDamage = weaponData.baseDamage * PlayerStats.damage;
        critChance = PlayerStats.critChance;
        critDamage = PlayerStats.critDamage;
        baseFireRate = weaponData.baseFireRate * PlayerStats.fireRate;
        baseRange = weaponData.baseRange;
        baseQuantity = weaponData.baseQuantity + PlayerStats.quantity;
        projectileSpeed = weaponData.projectileSpeed * PlayerStats.projectileSpeed;
        aoeRadius = weaponData.aoeRadius * PlayerStats.area;
        coneAngle = weaponData.coneAngle * PlayerStats.area;
        spreadAngle = weaponData.spreadAngle;
    }
    private void InitializeWeaponInstance(WeaponData weaponData) // for enemies
    {
        this.weaponData = weaponData;
        delayBetweenShots = weaponData.delayBetweenShots;
        baseDamage = weaponData.baseDamage;
        baseFireRate = weaponData.baseFireRate;
        baseRange = weaponData.baseRange;
        baseQuantity = weaponData.baseQuantity;
        projectileSpeed = weaponData.projectileSpeed;
        aoeRadius = weaponData.aoeRadius;
        coneAngle = weaponData.coneAngle;
        spreadAngle = weaponData.spreadAngle;
    }
}
