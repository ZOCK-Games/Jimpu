using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private Button HeartsButton;
    [SerializeField] private Button EnemyButton;
    [SerializeField] private Button LuckyBlockButton;
    [SerializeField] private Button BombButton;
    [SerializeField] private GameObject CheatCodeUi;
 
    private int CurentInt;
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private LuckyBlock luckyBlock;
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private BombScribt bombScribt;
    void Start()
    {
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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
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
                playerControll.PlayerHealth += 1;
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
