using UnityEngine;

public interface IShootStrategy
{
    void Execute(ShootContext context);
}

public struct ShootContext
{
    public Vector3 spawnPosition;
    public Vector3 direction;
    public LayerMask targetLayer;
    public WeaponData weaponData;
    public Transform shooterTransform;
}