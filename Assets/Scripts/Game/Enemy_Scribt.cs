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
    public Vector2 EnemyPosition;
    public PolygonCollider2D Hit_box_player;
    public PolygonCollider2D Hit_box_enemy_body;
    public PolygonCollider2D Hit_box_enemy_head;
    public List<GameObject> Heart;
    private bool canTakeDamage = true;
    public GameObject Player_death_screen;
    public PlayerStats PlayerStats;
    public int EnemyHealt;
    public string EnemyPositionGoal;
    public TilemapCollider2D GroundTilemapCollider2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_death_screen.SetActive(false);
        EnemyHealt = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Hit_box_enemy_body.IsTouching(GroundTilemapCollider2d))
        {
            EnemyPositionGoal = "-";
        }



        if (Hit_box_player.IsTouching(Hit_box_enemy_body) && canTakeDamage)
            {
                StartCoroutine(DamagePlayer());
                Debug.Log("-1 leben");
            }
        if (PlayerStats.PlayerHealt <= 1)
            Heart[0].SetActive(false);
        if (PlayerStats.PlayerHealt <= 2)
            Heart[1].SetActive(false);
        if (PlayerStats.PlayerHealt <= 3)
            Heart[2].SetActive(false);

        if (PlayerStats.PlayerHealt >= 1)
            Heart[0].SetActive(true);
        if (PlayerStats.PlayerHealt >= 2)
            Heart[1].SetActive(true);
        if (PlayerStats.PlayerHealt >= 3)
            Heart[2].SetActive(true);
        if (Hit_box_enemy_head.IsTouching(Hit_box_player) && Input.GetKey(KeyCode.LeftShift))
        {
            //PLayer attacke
        }
        if (PlayerStats.PlayerHealt == 0)
            Player_death_screen.SetActive(true);
        if (EnemyHealt <= 0)
        {
            enemy.SetActive(false);
        }

        void MoveEnemy()  //Soll den spiler bewegen wen Keine kolision bei objekt
        {
            transform.position = Vector2.SmoothDamp(EnemyPosition, EnemyPositionGoal + , ref EnemyPosition, smoothTime, maxSpee); //not finished 
        }
         
    }
    public IEnumerator DamagePlayer()
    {
        PlayerStats.PlayerHealt -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
        

           }
}
