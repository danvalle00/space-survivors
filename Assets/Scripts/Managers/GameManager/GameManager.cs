using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* NOTE: esse caba vai gerenciar o estado geral do jogo, tipo pausar, game over, win condition
     e outras paradas globais que n se encaixam em outros managers kkkkkk
     e tbm vai referenciar qual ship o player ta usando e o player persistent data
     */
    public static GameManager Instance { get; private set; }
    [SerializeField] private PlayerData playerPersistentData;
    [SerializeField] private SpaceshipData currentSpaceshipData; // ele q vai mandar pro difficulty manager os multiplicadores

    public PlayerData PlayerPersistentData => playerPersistentData;
    public SpaceshipData CurrentSpaceshipData => currentSpaceshipData;
    public PlayerStatsInstance PlayerStatsInstance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayerStatsInstance = new PlayerStatsInstance(playerPersistentData, currentSpaceshipData);
    }

}