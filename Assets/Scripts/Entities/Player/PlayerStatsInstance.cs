public class PlayerStatsInstance
{
    // Defensive Stats
    public float maxHp;
    public float hpRegen;
    public int armor;
    public float speed;

    // Offensive Stats
    public float damage;
    public float critChance;
    public float critDamage;
    public float fireRate;
    public float projectileSpeed;
    public float area;
    public int quantity;
    public float lifesteal;

    // Utility Stats
    public int numberOfLives;
    public int skipUpgradeChances;
    public int banishUpgrades;
    public int sealUpgrades;
    
    // Score Stats
    public float xpMultiplier;
    public float difficultyMultiplier;
    public float currencyMultiplier;

    public PlayerStatsInstance(PlayerData playerData, SpaceshipData spaceshipData)
    {
        // Defensive Stats
        maxHp = spaceshipData.shipMaxHp * playerData.hpMultiplier;
        hpRegen = spaceshipData.shipHpRegen * playerData.hpRegenMultiplier;
        armor = spaceshipData.shipArmor + playerData.armorMultiplier;
        speed = spaceshipData.shipSpeed * playerData.speedMultiplier;

        // Offensive Stats
        damage = spaceshipData.shipIncreasedDamage * playerData.damageMultiplier;
        critChance = spaceshipData.shipCritChance * playerData.critChanceMultiplier;
        critDamage = spaceshipData.shipCritDamage * playerData.critDamageMultiplier;
        fireRate = spaceshipData.shipIncreasedFireRate * playerData.fireRateMultiplier;
        projectileSpeed = spaceshipData.shipIncreasedProjectileSpeed * playerData.projectileSpeedMultiplier;
        area = spaceshipData.shipIncreasedArea * playerData.aoeRadiusMultiplier;
        quantity = spaceshipData.shipIncreasedQuantity + playerData.quantityMultiplier;
        lifesteal = spaceshipData.shipLifesteal * playerData.lifestealMultiplier;

        // Utility Stats
        numberOfLives = playerData.numberOfLives;
        skipUpgradeChances = playerData.skipUpgradeChances;
        banishUpgrades = playerData.banishUpgrades;
        sealUpgrades = playerData.sealUpgrades;

        // Score Stats
        xpMultiplier = spaceshipData.shipXpMultiplier * playerData.xpMultiplier;
        difficultyMultiplier = spaceshipData.shipDifficultyMultiplier * playerData.difficultyMultiplier;
        currencyMultiplier = spaceshipData.shipCurrencyMultiplier * playerData.currencyMultiplier;    

    }
}