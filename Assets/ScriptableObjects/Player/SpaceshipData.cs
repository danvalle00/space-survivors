using UnityEngine;

[CreateAssetMenu(fileName = "New Spaceship", menuName = "Spaceships/SpaceshipData")]
public class SpaceshipData : ScriptableObject
{
    [Header("Spaceship Info")]
    [Tooltip("Basic information about the spaceship")]
    public string shipName;
    public string shipDescription;
    public Sprite shipSprite;

    [Header("Spaceship Basic Stats")]
    [Tooltip("Stats that define the spaceship's capabilities")]
    public float shipMaxHp;
    public float shipHpRegen;
    public float shipArmor;
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
    public float shipLifesteal;

    [Header("Spaceship Point Stats")]
    [Tooltip("Currency, Xp or Gold stats for the spaceship")]
    public float shipXpMultiplier;
    public float shipGoldMultiplier;
    public float shipPointMultiplier; // out of game currency (buy persistent upgrades)


}







