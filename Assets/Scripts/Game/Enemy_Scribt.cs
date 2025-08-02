using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IDataPersitence
{
    public GameObject Player;
    public GameObject enemy;
    public int Attack_range;
    public int EnemyHealt;
    public PolygonCollider2D Hit_box_player;
    public PolygonCollider2D Hit_box_enemy_body;
    public PolygonCollider2D Hit_box_enemy_head;
    public List<GameObject> Heart;
    public GameObject Player_death_screen;
    public GameData gameData;
    public TilemapCollider2D GroundTilemapCollider2d;
    public BoxCollider2D PlayerBoxTouchBlock;
    public bool EnemyCanMove = true;
    public bool canTakeDamage = true;
    public float EnemyY;
    public NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_death_screen.SetActive(false);
        EnemyHealt = 1;
        EnemyY = enemy.transform.position.y;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (EnemyCanMove == true)
            MoveEnemy();

        if (Hit_box_player.IsTouching(Hit_box_enemy_body) && canTakeDamage) //Checkt ob der spiler Chaden bekom
        {
            StartCoroutine(DamagePlayer());
            Debug.Log("-1 leben");
        }
        //Removed Die Herzen / fügt sie hinzu
        if (gameData.Health <= 1)
            Heart[0].SetActive(false);
        if (gameData.Health <= 2)
            Heart[1].SetActive(false);
        if (gameData.Health <= 3)
            Heart[2].SetActive(false);

        if (gameData.Health >= 1)
            Heart[0].SetActive(true);
        if (gameData.Health >= 2)
            Heart[1].SetActive(true);
        if (gameData.Health >= 3)
            Heart[2].SetActive(true);
        if (Hit_box_enemy_head.IsTouching(Hit_box_player) && Input.GetKey(KeyCode.LeftShift))
        {
            //PLayer attacke
        }
        if (gameData.Health == 0)              // Aktiviert Den Dead Screen
            Player_death_screen.SetActive(true);
        if (EnemyHealt <= 0)
        {
            enemy.SetActive(false);
        }

        void MoveEnemy()  //Soll den spiler bewegen wen Keine kolision bei objekt
        {
            if (agent.enabled == true)
            {
                agent.SetDestination(Player.transform.position);

                NavMeshPath path = agent.path;
                if (agent.path == null || agent.path.corners.Length < 2) return;

                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
                }



                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }


    }
    public IEnumerator DamagePlayer()   //Damage the Player 
    {
        gameData.Health -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;


    }
    public void SaveGame(ref GameData data) // Save the current Data to GameData
    {
        data.EnemyHealth = this.EnemyHealt;
        data.EnemyPositionX = this.enemy.transform.localPosition.x;
        data.EnemyPositionY = this.enemy.transform.localPosition.y;
    }
    public void LoadGame(GameData data) // Load the GameData to the current Data
    {
        this.EnemyHealt = data.EnemyHealth;
        this.enemy.transform.localPosition = new Vector3(data.EnemyPositionX, data.EnemyPositionY, 0);
    }
}
