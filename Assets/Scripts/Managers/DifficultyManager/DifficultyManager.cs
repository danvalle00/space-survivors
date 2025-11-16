using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Difficulty Settings")]
    [SerializeField] private float playerDifficultyMultiplier = 1f; // REVIEW - qnd o criar os upgrades persistentes do player, alterar este valor
    [SerializeField] private float spaceShipDifficultyMultiplier = 1f;
    [Header("Time-Based Difficulty Increase")]
    [SerializeField] private float difficultyIncreaseRate = 1.12f; // rate at which difficulty increases per minute

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public float GetCurrentDifficultyMultiplier()
    {
        float timeInMinutes = Time.time / 60f;
        float timeMod = Mathf.Pow(difficultyIncreaseRate, timeInMinutes);
        float currentDifficulty = playerDifficultyMultiplier * spaceShipDifficultyMultiplier * timeMod; // super aggressive
        return currentDifficulty;
    }
}
