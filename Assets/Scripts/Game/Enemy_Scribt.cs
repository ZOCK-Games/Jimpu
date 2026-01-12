using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour//, IDataPersitence
{
    public Transform GoTo;
    public GameObject Player;
    public GameObject HeartContainer;
    public GameObject HeartPrefab;
    public GameObject JimbuBulletPrefab;
    public GameObject BulletContainer;
    public string DeathScene;
    private bool JimpusCanMove = true;
    private bool canTakeDamage = true;
    public HealthManagerPlayer healthManagerPlayer;
    public PlayerControll playerControll;
    public GameObject Grid;
    [Header("Enemy Settings")]
    public List<GameObject> EnemyPrefab;
    public int MaxEnemys;
    public int EnemyHealt;
    public GameObject EnemyContainer;
    public int AttackRange;

    public static List<EnemyInfo> JimpusInfos;


    void Start()
    {
        JimpusInfos = new List<EnemyInfo>();
        for (int i = 0; i < 4; i++)
        {
            SpawnEnemy();
        }
    }

    void Update()
    {
        if (JimpusInfos != null && JimpusInfos.Count < MaxEnemys)
        {
            SpawnEnemy();
        }

        if (JimpusCanMove == true)
        {
            if (JimpusInfos == null) return;

            for (int i = 0; i < JimpusInfos.Count; i++)
            {
                if (JimpusInfos[i].Target == null)
                {
                    NavMeshAgent agent = JimpusInfos[i].meshAgent;

                    if (agent == null)
                    {
                        Debug.LogError($"No Agent on {agent.gameObject.name}");
                    }
                    else if (agent != null)
                    {
                        JimpusInfos[i].SetTarget(Player);
                    }
                }
            }
        }
    }


    public void SpawnEnemy()
    {
        bool FoundTile = false;
        int maxAttempts = 0;
        List<Tilemap> Grounds = new List<Tilemap>();
        for (int i = 0; i < Grid.transform.childCount; i++)
        {
            if (Grid.transform.GetChild(i).gameObject.tag == "Ground" && Grid.transform.GetChild(i).gameObject.GetComponent<Tilemap>() != null)
            {
                Grounds.Add(Grid.transform.GetChild(i).gameObject.GetComponent<Tilemap>());
            }
        }
        while (!FoundTile && maxAttempts <= 15)
        {
            maxAttempts += 1;
            Tilemap CurentT = Grid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
            Vector3Int PlayerPos = new Vector3Int((int)playerControll.transform.position.x, (int)playerControll.transform.position.y);
            int Posy = Random.Range(PlayerPos.y - 10, PlayerPos.y + 10);
            int Posx = Random.Range(PlayerPos.x - 25, PlayerPos.x + 25);

            Vector3 PosTile = new Vector3(Posx, Posy, 0);
            Vector3Int cellPos = CurentT.WorldToCell(PosTile);

            if (!CurentT.HasTile(cellPos))
            {
                FoundTile = true;
                int TypeEnemy = Random.Range(0, EnemyPrefab.Count);
                GameObject CE = Instantiate(EnemyPrefab[TypeEnemy]);
                CE.transform.position = cellPos;
                CE.name = "Enemy_" + TypeEnemy + "_" + EnemyContainer.transform.childCount;
                CE.transform.SetParent(EnemyContainer.transform);
                if (CE.GetComponent<NavMeshAgent>() == null)
                {
                    CE.AddComponent<NavMeshAgent>();
                }
                NavMeshAgent agent = CE.GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                EnemyInfo info = CE.GetComponent<EnemyInfo>();
                if (info == null)
                {
                    CE.AddComponent<EnemyInfo>();
                }
                info.EnemyHealt = 5;
                info.playerControll = this.playerControll;
                JimpusInfos.Add(info);
                Debug.Log("Spawned jimpu");
            }
        }
    }


    public IEnumerator DamagePlayer()   //Damage the Player 
    {
        healthManagerPlayer.PlayerHealth -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    /*public void SaveGame(ref GameData data) // Save the current Data to GameData
    {
        // Alle Gegner durchlaufen
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {

            GameObject enemy = EnemyContainer.transform.GetChild(i).gameObject;
            EnemyInfo info = enemy.GetComponent<EnemyInfo>();




            EnemySaveData eData = new EnemySaveData
            {
                EnemyHealth = enemy.GetComponent<EnemyInfo>().EnemyHealt,
                EnemyPositionX = enemy.transform.position.x,
                EnemyPositionY = enemy.transform.position.y
            };

            data.enemies.Add(eData);

        }

    }

public void LoadGame(GameData data)
{
    if (EnemyPrefab == null)
    {
        Debug.LogError("Enemy Prefab nicht gesetzt!");
        return;
    }

    // Alte Gegner löschen
    for (int i = EnemyContainer.transform.childCount - 1; i >= 0; i--)
    {
        Destroy(EnemyContainer.transform.GetChild(i).gameObject);
    }

    // Gegner aus GameData wiederherstellen
    int index = 0;
    foreach (var eData in data.enemies)
    {
        Vector3 pos = new Vector3(eData.EnemyPositionX, eData.EnemyPositionY, 0);
        GameObject enemy = Instantiate(EnemyPrefab[0], pos, Quaternion.identity, EnemyContainer.transform);
        enemy.name = EnemyPrefab[0].name + "_" + index;
        index++;

        EnemyInfo info = enemy.GetComponent<EnemyInfo>();
        if (info != null)
            info.EnemyHealt = eData.EnemyHealth;
        else
            Debug.LogWarning("EnemyInfo fehlt auf Prefab!");
    } */

}