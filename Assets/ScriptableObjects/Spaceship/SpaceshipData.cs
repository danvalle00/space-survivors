using UnityEngine;

[CreateAssetMenu(fileName = "New Spaceship", menuName = "Spaceships/SpaceshipData")]
public class SpaceshipData : ScriptableObject
{
    [Header("Spaceship Template Data")]
    [Header("Spaceship Description")] // REVIEW - DEFINIR SE TEM MAIS, SE É MULTIPLICADORES OU FLAT / SOBRE PREFAB OU SPRITES, SOUNDS VFX ETC
    // esses dados sao a base do ship, o player persistent data e os upgrades vao modificar esses valores
    // entao o crit change tem que ser por exemplo 5% base no ship e o player persistent data pode aumentar isso em 1.2x por exemplo
    [Tooltip("Basic information about the spaceship")]
    public string shipName;
    public string shipDescription;
    public Sprite shipSprite;

    [Header("Spaceship Basic Stats")]
    [Tooltip("Stats that define the spaceship's capabilities")]
    public float shipMaxHp;
    public float shipHpRegen;
    public int shipArmor;
    public float shipSpeed;
    [Header("Spaceship Combat Stats")]
    [Tooltip("Combat-related stats for the spaceship")]
    public float shipCritChance;
    public float shipCritDamage;
    public float shipIncreasedArea; // NOTE - affects AOE radius e cone angle e o baseRange do cone tbm
    public float shipIncreasedFireRate;
    public float shipIncreasedProjectileSpeed;
    public int shipIncreasedQuantity;
    // public float shipIncreasedDuration; // REVIEW - se vai ter arma com duration
    public float shipIncreasedDamage;
    public float shipLifeSteal;

    [Header("Spaceship Point Stats")]
    [Tooltip("Currency, Xp or Difficulty stats for the spaceship")]
    public float shipXpMultiplier;
    public float shipDifficultyMultiplier;
    public float shipCurrencyMultiplier;


}







