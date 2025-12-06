using System.Collections.Generic;
public class PlayerStatsInstance
{
    // Base Stats Instance combining PlayerData and SpaceshipData
    private readonly Dictionary<StatType, float> baseStats = new();

    // Upgrade Modifiers
    private readonly StatModifierCollection<StatType> modifiers = new();
    // Utility stats
    public int numberOfLives;
    public int skipUpgradeChances;
    public int banishUpgrades;
    public int sealUpgrades;


    public PlayerStatsInstance(PlayerData playerData, SpaceshipData spaceshipData)
    {

        // defensive
        baseStats.Add(StatType.MaxHp, spaceshipData.shipMaxHp * playerData.hpMultiplier);
        baseStats.Add(StatType.HpRegen, spaceshipData.shipHpRegen * playerData.hpRegenMultiplier);
        baseStats.Add(StatType.Armor, spaceshipData.shipArmor + playerData.armorMultiplier);
        baseStats.Add(StatType.Speed, spaceshipData.shipSpeed * playerData.speedMultiplier);

        // offensive
        baseStats.Add(StatType.Damage, spaceshipData.shipIncreasedDamage * playerData.damageMultiplier);
        baseStats.Add(StatType.CritChance, spaceshipData.shipCritChance * playerData.critChanceMultiplier);
        baseStats.Add(StatType.CritDamage, spaceshipData.shipCritDamage * playerData.critDamageMultiplier);
        baseStats.Add(StatType.FireRate, spaceshipData.shipIncreasedFireRate * playerData.fireRateMultiplier);
        baseStats.Add(StatType.ProjectileSpeed, spaceshipData.shipIncreasedProjectileSpeed * playerData.projectileSpeedMultiplier);
        baseStats.Add(StatType.Area, spaceshipData.shipIncreasedArea * playerData.aoeRadiusMultiplier);
        baseStats.Add(StatType.Quantity, spaceshipData.shipIncreasedQuantity + playerData.quantityMultiplier);
        baseStats.Add(StatType.LifeSteal, spaceshipData.shipLifeSteal * playerData.lifeStealMultiplier);

        // currency, xp, difficulty
        baseStats.Add(StatType.XpMultiplier, spaceshipData.shipXpMultiplier * playerData.xpMultiplier);
        baseStats.Add(StatType.DifficultyMultiplier, spaceshipData.shipDifficultyMultiplier * playerData.difficultyMultiplier);
        baseStats.Add(StatType.CurrencyMultiplier, spaceshipData.shipCurrencyMultiplier * playerData.currencyMultiplier);

        // utility
        numberOfLives = playerData.numberOfLives;
        skipUpgradeChances = playerData.skipUpgradeChances;
        banishUpgrades = playerData.banishUpgrades;
        sealUpgrades = playerData.sealUpgrades;
    }

    public void AddMultiplier(StatType statType, float multiplier)
    {
        modifiers.AddMultiplier(statType, multiplier);
    }

    public void AddAddition(StatType statType, float addition)
    {
        modifiers.AddAddition(statType, addition);
    }

    public float GetFinalStat(StatType statType)
    {
        float baseValue = baseStats[statType];
        return modifiers.ApplyModifiers(statType, baseValue);
    }

}



public enum StatType
{
    // Defensive
    MaxHp,
    HpRegen,
    Armor,
    Speed,
    // Offensive
    Damage,
    CritChance,
    CritDamage,
    FireRate,
    ProjectileSpeed,
    Area,
    Quantity,
    LifeSteal,
    // Currency, xP, Difficulty
    XpMultiplier,
    DifficultyMultiplier,
    CurrencyMultiplier
}