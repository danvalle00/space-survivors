using System;
using System.Collections.Generic;
public class StatModifierCollection<TStatEnum> where TStatEnum : Enum
{
    private Dictionary<TStatEnum, float> statMultipliers = new();
    private Dictionary<TStatEnum, float> statAdditions = new();
    public StatModifierCollection()
    {
        InitializateModifiers();
    }

    public void AddMultiplier(TStatEnum statType, float multiplier)
    {
        statMultipliers[statType] += multiplier;
    }
    public void AddAddition(TStatEnum statType, float addition)
    {
        statAdditions[statType] += addition;
    }

    public void InitializateModifiers()
    {
        foreach (TStatEnum stat in Enum.GetValues(typeof(TStatEnum)))
        {
            statMultipliers[stat] = 1f;
            statAdditions[stat] = 0f;
        }
    }

    public float ApplyModifiers(TStatEnum statType, float baseValue)
    {
        float multiplier = statMultipliers[statType];
        float addition = statAdditions[statType];

        return (baseValue * multiplier) + addition;
    }
}

