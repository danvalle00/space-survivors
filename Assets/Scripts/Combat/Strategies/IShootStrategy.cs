using UnityEngine;

public interface IShootStrategy
{
    void Execute(ShootContext context);
}

public struct ShootContext // i can add more context if a need arises
{
    public Vector2 spawnPosition;
    public Vector2 direction;
    public LayerMask targetLayer;
    public WeaponData weaponData;
    public Transform shooterTransform;
}