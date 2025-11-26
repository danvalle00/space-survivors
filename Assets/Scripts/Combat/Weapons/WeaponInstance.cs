using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance
{
    public WeaponData weaponData;
    private readonly Dictionary<WeaponStatType, float> templateStats = new();
    private readonly PlayerStatsInstance playerStats = GameManager.Instance != null ? GameManager.Instance.PlayerStatsInstance : null;
    private StatModifierCollection<WeaponStatType> modifiers = new();
    private bool isPlayer;

    private static readonly Dictionary<WeaponStatType, WeaponStatConfig> statConfigs = new()
    {
        { WeaponStatType.Damage, new WeaponStatConfig(StatType.Damage, ScalingType.Multiplicative) },
        { WeaponStatType.FireRate, new WeaponStatConfig(StatType.FireRate, ScalingType.Multiplicative) },
        { WeaponStatType.Quantity, new WeaponStatConfig(StatType.Quantity, ScalingType.Additive) },
        { WeaponStatType.ProjectileSpeed, new WeaponStatConfig(StatType.ProjectileSpeed, ScalingType.Multiplicative) },
        { WeaponStatType.AoeRadius, new WeaponStatConfig(StatType.Area, ScalingType.Multiplicative) },
        { WeaponStatType.ConeAngle, new WeaponStatConfig(StatType.Area, ScalingType.Multiplicative) },
        { WeaponStatType.Range, new(null, ScalingType.None) },
        { WeaponStatType.DelayBetweenShots, new(null, ScalingType.None) },
        { WeaponStatType.SpreadAngle, new(null, ScalingType.None) }
    };

    public WeaponInstance(WeaponData weaponData, bool isPlayer)
    {

        this.weaponData = weaponData;
        templateStats.Add(WeaponStatType.Damage, weaponData.baseDamage);
        templateStats.Add(WeaponStatType.FireRate, weaponData.baseFireRate);
        templateStats.Add(WeaponStatType.Quantity, weaponData.baseQuantity);
        templateStats.Add(WeaponStatType.ProjectileSpeed, weaponData.projectileSpeed);
        templateStats.Add(WeaponStatType.AoeRadius, weaponData.aoeRadius);
        templateStats.Add(WeaponStatType.ConeAngle, weaponData.coneAngle);
        templateStats.Add(WeaponStatType.Range, weaponData.baseRange);
        templateStats.Add(WeaponStatType.DelayBetweenShots, weaponData.delayBetweenShots);
        templateStats.Add(WeaponStatType.SpreadAngle, weaponData.spreadAngle);

        this.isPlayer = isPlayer;
    }
    public float GetStat(WeaponStatType weaponStat)
    {
        float templateValue = templateStats[weaponStat]; // valor a ser modificado
        WeaponStatConfig config = statConfigs[weaponStat];
        if (!isPlayer || !statConfigs.ContainsKey(weaponStat))
        {
            return templateValue;
        }

        if (config.playerStat == null || config.scalingType == ScalingType.None)
        {
            return templateValue;
        }
        float playerStatValue = playerStats.GetFinalStat(config.playerStat.Value); // modificador do jogador (meta prog + spaceship)
        float scaledStatValue;
        switch (config.scalingType)
        {
            case ScalingType.Additive:
                scaledStatValue = templateValue + playerStatValue;
                break;
            case ScalingType.Multiplicative:
                scaledStatValue = templateValue * playerStatValue;
                break;
            default:
                scaledStatValue = 0f;
                break;
        }
        float finalValue = modifiers.ApplyModifiers(weaponStat, scaledStatValue);
        return finalValue;
    }

    public float RollDamage()
    {
        float damage = GetStat(WeaponStatType.Damage);
        if (playerStats == null)
        {
            return damage;
        }
        bool isCritical = Random.value <= playerStats.GetFinalStat(StatType.CritChance);
        if (isCritical)
        {
            damage *= playerStats.GetFinalStat(StatType.CritDamage);
        }
        return damage;
    }
    public void AddMultiplier(WeaponStatType statType, float multiplier)
    {
        modifiers.AddMultiplier(statType, multiplier);
    }
    public void AddAddition(WeaponStatType statType, float addition)
    {
        modifiers.AddAddition(statType, addition);
    }
}
public class WeaponStatConfig
{
    public StatType? playerStat;
    public ScalingType scalingType;

    public WeaponStatConfig(StatType? playerStat, ScalingType scalingType)
    {
        this.playerStat = playerStat;
        this.scalingType = scalingType;
    }
}

public enum WeaponStatType
{
    Damage,
    FireRate,
    Quantity,
    ProjectileSpeed,
    AoeRadius,
    ConeAngle,
    Range,
    DelayBetweenShots,
    SpreadAngle,
}

public enum ScalingType
{
    None,
    Additive,
    Multiplicative
}
