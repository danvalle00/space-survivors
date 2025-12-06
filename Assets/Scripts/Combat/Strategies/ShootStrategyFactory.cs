using UnityEngine;

public static class ShootStrategyFactory
{
    public static IShootStrategy GetStrategy(ShootStrategyType shootType)
    {
        switch (shootType)
        {
            case ShootStrategyType.Projectile:
                return new ProjectileShootStrategy();
            case ShootStrategyType.TargetedAoe:
                return new TargetedAOEStrategy();
            case ShootStrategyType.RandomAoe:
                return new RandomAoeStrategy();
            case ShootStrategyType.FrontalCone:
                return new FrontalConeStrategy();
            case ShootStrategyType.MeleeCircle:
                return new MeleeCircleShootStrategy();
            default:
                Debug.LogError("Unsupported Shoot Strategy Type: " + shootType);
                return null;
        }
    }
}
