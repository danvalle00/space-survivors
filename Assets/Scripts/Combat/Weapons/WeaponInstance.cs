public class WeaponInstance
{
    public WeaponData weaponData; // template
    public float delayBetweenShots;
    public float baseDamage;
    public float baseFireRate;
    public float baseRange;
    public int baseQuantity;
    public float projectileSpeed;
    public float aoeRadius;
    public float coneAngle;
    public float spreadAngle;

    public WeaponInstance(WeaponData weaponData, SpaceshipData spaceship)
    {
        this.weaponData = weaponData;
        delayBetweenShots = weaponData.delayBetweenShots;
        baseDamage = weaponData.baseDamage * (1 + spaceship.shipIncreasedDamage / 100f);
        baseFireRate = weaponData.baseFireRate * (1 + spaceship.shipIncreasedFireRate / 100f);
        baseRange = weaponData.baseRange;
        baseQuantity = weaponData.baseQuantity + spaceship.shipIncreasedQuantity;
        projectileSpeed = weaponData.projectileSpeed * (1 + spaceship.shipIncreasedProjectileSpeed / 100f);
        aoeRadius = weaponData.aoeRadius * (1 + spaceship.shipIncreasedArea / 100f);
        coneAngle = weaponData.coneAngle * (1 + spaceship.shipIncreasedArea / 100f);
        spreadAngle = weaponData.spreadAngle;
    } 
    public WeaponInstance(WeaponData weaponData)
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
