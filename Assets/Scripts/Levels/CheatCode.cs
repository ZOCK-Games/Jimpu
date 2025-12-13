using UnityEngine;
using UnityEngine.UI;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private Button HeartsButton;
    [SerializeField] private Button EnemyButton;
    [SerializeField] private Button LuckyBlockButton;
    [SerializeField] private Button BombButton;
    [SerializeField] private Button ShotVoiceButton;
    [SerializeField] private GameObject CheatCodeUi;
    [SerializeField] private GameObject CheatShootVoiceUI;
    [SerializeField] private Button EnergyButton;

    private int CurentInt;
    [SerializeField] private HealthManagerPlayer healthManagerPlayer;
    [SerializeField] private LuckyBlock luckyBlock;
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private BombScribt bombScribt;
    [SerializeField] private EnergyManager energyManager;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        EnergyButton.onClick.AddListener(() =>
        {
            StartCoroutine(energyManager.AddEnergy(25));
            
        });
        HeartsButton.onClick.AddListener(() =>
        {
            CurentInt = 0;
            CheckCheat();
        });
        LuckyBlockButton.onClick.AddListener(() =>
        {
            CurentInt = 1;
            CheckCheat();
        });
        EnemyButton.onClick.AddListener(() =>
        {
            CurentInt = 2;
            CheckCheat();
        });
        BombButton.onClick.AddListener(() =>
        {
            CurentInt = 3;
            CheckCheat();
        });
        ShotVoiceButton.onClick.AddListener(() => CheatShootVoiceUI.SetActive(true));
    }
    void Update()
    {
        if (inputActions.Player.CheatMenue.WasPerformedThisFrame())
        {
            CheatCodeUi.SetActive(!CheatCodeUi.activeSelf);
        }
    }

    // Update is called once per frame
    void CheckCheat()
    {
        switch (CurentInt)
        {
            case 0:
                healthManagerPlayer.PlayerHealth += 1;
                Debug.Log("added 1 Health for player");
                break;
            case 1:
                luckyBlock.SpawnChest();
                Debug.Log("Spawned a Chest for player");
                break;
            case 2:
                enemyScript.MaxEnemys += 1;
                enemyScript.SpawnEnemy();
                Debug.Log("Spawned Enemy for player");
                break;
            case 3:
                bombScribt.SetBombPosition();
                Debug.Log("SetBomb");
                break;



        }

    }
}
