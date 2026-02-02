using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour, IDataPersitence
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
    public List<EnemyInfo> JimpuInfos = new List<EnemyInfo>(JimpusInfos);
    public static List<EnemyInfo> JimpusInfos = new List<EnemyInfo>();

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (JimpusInfos != null && JimpusInfos.Count < MaxEnemys)
        {
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
                info.SetTarget(Player);
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

    public void SaveData(SaveManager manager)
    {
        if (JimpusInfos != null)
        {
            List<EnemyInfo> JimpuInfos = new List<EnemyInfo>(JimpusInfos);
            for (int i = 0; i < JimpuInfos.Count; i++)
            {
                JimpuDataSO jimpuData = manager.JimpuListData.jimpuDatas.Find(x => x.JimpuID == JimpuInfos[i].JimpuID);
                if (jimpuData == null)
                {
                    jimpuData = new JimpuDataSO { JimpuID = JimpusInfos[i].JimpuID };
                    manager.JimpuListData.jimpuDatas.Add(jimpuData);
                }
                jimpuData.Health = JimpusInfos[i].EnemyHealt;
                jimpuData.Position = JimpusInfos[i].JimpuObj.transform.position;
            }
        }
    }
    public void LoadData(SaveManager manager)
    {
        List<JimpuDataSO> jimpuList = manager.JimpuListData.jimpuDatas;
        for (int i = 0; i < jimpuList.Count; i++)
        {
            if (jimpuList[i] != null)
            {
                int TypeEnemy = Random.Range(0, EnemyPrefab.Count);
                GameObject CE = Instantiate(EnemyPrefab[TypeEnemy]);
                CE.transform.position = jimpuList[i].Position;
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
                info.EnemyHealt = (int)jimpuList[i].Health;
                info.JimpuID = jimpuList[i].JimpuID;
                info.playerControll = this.playerControll;
                JimpusInfos.Add(info);
                Debug.Log("Spawned jimpu");
            }
        }
    }
}