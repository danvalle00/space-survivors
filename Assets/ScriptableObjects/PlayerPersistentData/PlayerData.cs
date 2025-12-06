using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    // esses multiplicadores tem que ser >= 1 pra n dar problema 
    // e n ser negativo tbm 1.2 = 20% increase, 0.8 = 20% decrease
    [Header("Player Persistent Data Info")]
    public string playerName;
    [Header("Player Persistent Data Multipliers")] // REVIEW - DEFINIR SE TEM MAIS, SE É MULTIPLICADORES OU FLAT
    [Header("Defensive Stats")]
    public float hpMultiplier;
    public float hpRegenMultiplier;
    public int numberOfLives;
    public int armorMultiplier;

    [Header("Offensive Stats")]
    public float speedMultiplier;
    public float damageMultiplier;
    public float fireRateMultiplier;
    public float critChanceMultiplier;
    public float critDamageMultiplier;
    public float projectileSpeedMultiplier;
    public float aoeRadiusMultiplier;
    public int quantityMultiplier;
    public float lifeStealMultiplier;

    [Header("Utility Stats")]
    public int skipUpgradeChances;
    public int banishUpgrades;
    public int sealUpgrades;

    [Header("Score Stats")]
    public float xpMultiplier;
    public float difficultyMultiplier;
    public float currencyMultiplier;


}
