using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour//, IDataPersitence
{
    public Transform GoTo;
    public GameObject Player;
    public GameObject EnemyContainer;
    public int AttackRange;
    public int EnemyHealt;
    public GameObject HeartContainer;
    public GameObject HeartPrefab;
    public string DeathScene;
    public bool EnemyCanMove = true;
    public bool canTakeDamage = true;
    public PlayerControll playerControll;
    public GameObject Grid;
    public List<GameObject> EnemyPrefab;
    public int MaxEnemys;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        SpawnEnemy();

        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            if (EnemyContainer.transform.GetChild(i).gameObject.GetComponent<NavMeshAgent>() == null)
            {
                EnemyContainer.transform.GetChild(i).gameObject.AddComponent<NavMeshAgent>();
            }
            if (EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>() == null)
            {
                EnemyContainer.transform.GetChild(i).gameObject.AddComponent<EnemyInfo>();
                EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt = 1;
            }
            EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt = 1;
            EnemyContainer.transform.GetChild(i).gameObject.name = "Enemy" + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = EnemyContainer.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = EnemyContainer.transform.GetChild(i);
            EnemyInfo enemy = child.GetComponent<EnemyInfo>();

            if (enemy != null && enemy.EnemyHealt <= 0)
            {
                Debug.Log($"Removed Enemy: {child.gameObject.name}");
                Destroy(child.gameObject);
            }

        }

        if (EnemyContainer.transform.childCount >= MaxEnemys)
        {
            Transform child = EnemyContainer.transform.GetChild(0);
            Debug.Log($"Removed Enemy due to MaxEnemys: {child.gameObject.name}");
            Destroy(child.gameObject);

        }


        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            if (EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt <= -11)
            {
                Destroy(EnemyContainer.transform.GetChild(i).gameObject);
            }
        }

        if (EnemyCanMove == true)
            MoveEnemy();

        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            if (Player.GetComponent<PolygonCollider2D>().IsTouching(EnemyContainer.transform.GetChild(i).gameObject.GetComponent<CapsuleCollider2D>()) && canTakeDamage) //Checkt ob der spiler Chaden bekom
            {
                StartCoroutine(DamagePlayer());
                Debug.Log("-1 leben");
            }
        }
        ////////////////////////////////////////////////////////
        ///             Heart System                         ///
        ////////////////////////////////////////////////////////

        if (HeartContainer.transform.childCount != playerControll.PlayerHealth)
        {
            foreach (Transform child in HeartContainer.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < playerControll.PlayerHealth; i++)
            {
                GameObject HeartObj = Instantiate(HeartPrefab, HeartContainer.transform);
            }
        }
        ////////////////////////////////////////////////////////
    }
    public void MoveEnemy()
    {
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            GameObject enemy = EnemyContainer.transform.GetChild(i).gameObject;
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                enemy.AddComponent<NavMeshAgent>();
            }
            else if (agent != null)
            {
                agent.updateRotation = false;
                enemy.transform.rotation =  Quaternion.Euler(0, 0, 0);
                agent.SetDestination(Player.transform.position);
            }
        }
    }

    public void SpawnEnemy()
    {
        if (Grid.transform.GetChild(1).gameObject.tag == "Ground")
        {
            Tilemap CurentT = Grid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();

            int Posy = Random.Range(0, 50);
            int Posx = Random.Range(0, 50);

            Vector3 PosTile = new Vector3(Posx, Posy, 0);
            Vector3Int cellPos = CurentT.WorldToCell(PosTile);

            if (!CurentT.HasTile(cellPos))
            {
                int TypeEnemy = Random.Range(0, EnemyPrefab.Count);
                GameObject CE = Instantiate(EnemyPrefab[TypeEnemy]);
                CE.transform.position = cellPos;
                CE.name = "Enemy_" + TypeEnemy + "_" + EnemyContainer.transform.childCount;
                CE.transform.SetParent(EnemyContainer.transform);
                Debug.Log("Keien Tile: " + cellPos);
                Debug.DrawRay(cellPos, Vector3.up * 0.2f, Color.red);
                for (int i = 0; i < EnemyContainer.transform.childCount; i++)
                    EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt = 1;
            }
            else
            {
                Debug.Log("Cant Place there is a tile");
                SpawnEnemy();
            }

        }
        else
        {
            Debug.LogError("No Tilemap With Layer was found");
        }
    }
    public IEnumerator DamagePlayer()   //Damage the Player 
    {
        playerControll.PlayerHealth -= 1;
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