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

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject enemy;
    public int Attack_range;
    public int smoothTime;
    public int maxSpee;
    public int EnemyHealt;
    private Vector2 EnemyGoTo;
    private Vector2 EnemyPosition;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_death_screen.SetActive(false);
        EnemyHealt = 1;

        EnemyY = enemy.transform.position.y;


    }

    // Update is called once per frame
    void Update()
    {
        if (GroundTilemapCollider2d.IsTouching(PlayerBoxTouchBlock))
        {
            EnemyGoTo = Player.transform.position;
            Debug.Log("New goal Position for enemy" + EnemyGoTo);
        }


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
        if (EnemyPosition == EnemyGoTo)
        {
            MoveEnemy();
        }

        void MoveEnemy()  //Soll den spiler bewegen wen Keine kolision bei objekt
        {
            float newX = Mathf.SmoothDamp(transform.position.x, EnemyGoTo.x, ref EnemyPosition.x, smoothTime, maxSpee);
            transform.position = new Vector2(newX, EnemyY);
            enemy.transform.rotation = quaternion.Euler(0, 0, 0);
            EnemyPosition.y = EnemyY;  // Setzt die höhe vom Enemy
        }


    }
    public IEnumerator DamagePlayer()   //Damage the Player 
    {
        gameData.Health -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;


    }
}
