using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.U2D.Aseprite;

public class EnemyScript : MonoBehaviour, IDataPersitence
{
    public Transform GoTo;
    public GameObject Player;
    public GameObject EnemyContainer;
    public int AttackRange;
    public int EnemyHealt;
    public List<GameObject> Heart;
    public string DeathScene;
    public TilemapCollider2D GroundTilemapCollider2d;
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
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
            if (EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt <= 0)
            {
                Destroy(EnemyContainer.transform.GetChild(i).gameObject);
                Debug.Log("Removed Enemy: " + EnemyContainer.transform.GetChild(i).gameObject.name);
        }

        if (EnemyContainer.transform.childCount >= MaxEnemys + 1)
            {
                for (int i = 0; i < EnemyContainer.transform.childCount; i++)
                {
                    Destroy(EnemyContainer.transform.GetChild(i).gameObject);
                    break;
                }
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
        if (Player.GetComponent<PolygonCollider2D>().IsTouching(EnemyContainer.transform.GetChild(i).gameObject.GetComponent<PolygonCollider2D>()) && canTakeDamage) //Checkt ob der spiler Chaden bekom
            {
                StartCoroutine(DamagePlayer());
                Debug.Log("-1 leben");
            }
        }  
        //Removed Die Herzen / fügt sie hinzu
        if (playerControll.PlayerHealth <= 1)
            Heart[0].SetActive(false);
        if (playerControll.PlayerHealth <= 2)
            Heart[1].SetActive(false);
        if (playerControll.PlayerHealth <= 3)
            Heart[2].SetActive(false);

        if (playerControll.PlayerHealth >= 1)
            Heart[0].SetActive(true);
        if (playerControll.PlayerHealth >= 2)
            Heart[1].SetActive(true);
        if (playerControll.PlayerHealth >= 3)
            Heart[2].SetActive(true);
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
                agent.SetDestination(Player.transform.position);
        }
            else
            {
                Debug.LogWarning("Enemy nicht auf NavMesh platziert!");
            }

        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

    public void SpawnEnemy()
    {
        for (int i = 0; i < Grid.transform.childCount; i++)

        {
            if (Grid.transform.GetChild(1).gameObject.tag == "Ground")
            {
                Tilemap CurentT = Grid.transform.GetChild(i).gameObject.GetComponent<Tilemap>();

                int Posy = Random.Range(0, 50);
                int Posx = Random.Range(0, 50);

                Vector3 PosTile = new Vector3(Posx, Posy, 0);
                Vector3Int cellPos = CurentT.WorldToCell(PosTile);

                if (!CurentT.HasTile(cellPos))
                {
                    int TypeEnemy = Random.Range(0, EnemyPrefab.Count);
                    GameObject CE = Instantiate(EnemyPrefab[TypeEnemy]);
                    CE.transform.position = cellPos;
                    CE.name = "Enemy_" + TypeEnemy;
                    CE.transform.SetParent(EnemyContainer.transform);

                    Debug.Log("Keien Tile: " + cellPos);
                    Debug.DrawRay(cellPos, Vector3.up * 0.2f, Color.red);
                    EnemyContainer.transform.GetChild(i).gameObject.GetComponent<EnemyInfo>().EnemyHealt = 1;
                }
                else
                {
                    Debug.Log("Cant Place there is a tile");
                    SpawnEnemy();
                    break;
                }

            }
            else
            {
                Debug.LogError("No Tilemap With Layer was found");
                break;
            }
        }
    }
    public IEnumerator DamagePlayer()   //Damage the Player 
    {
        playerControll.PlayerHealth -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    public void SaveGame(ref GameData data) // Save the current Data to GameData
    {
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            data.EnemyHealth = this.EnemyHealt;
        data.EnemyPositionX = this.EnemyContainer.transform.GetChild(i).transform.localPosition.x;
        data.EnemyPositionY = this.EnemyContainer.transform.GetChild(i).transform.localPosition.y;
        }
    }
    public void LoadGame(GameData data) // Load the GameData to the current Data
    {
        this.EnemyHealt = data.EnemyHealth;
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        this.EnemyContainer.transform.GetChild(i).transform.localPosition = new Vector3(data.EnemyPositionX, data.EnemyPositionY, 0);
    }
}
