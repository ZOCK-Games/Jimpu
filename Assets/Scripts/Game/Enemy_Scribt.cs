using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class enemy_scribt : MonoBehaviour
{
    public GameObject Player;
    public GameObject enemy;
    public int Attack_range;
    public int smoothTime;
    public int maxSpee;
    public Vector2 Vector2_sel;
    public float Enemy_pos_y;
    public PolygonCollider2D Hit_box_player;
    public PolygonCollider2D Hit_box_enemy_body;
    public PolygonCollider2D Hit_box_enemy_head;
    public List<GameObject> Heart;
    private bool canTakeDamage = true;
    public GameObject Player_death_screen;
    public PlayerStats PlayerStats;
    public int EnemyHealt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_death_screen.SetActive(false);
        EnemyHealt = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position_P = Player.transform.position;
        Vector2 position_E = enemy.transform.position;
        Vector2 Distance = position_P - position_E;
        if (Distance.x <= Attack_range)
        {
            enemy.transform.Translate(Distance);
            transform.position = Vector2.SmoothDamp(position_E, position_P, ref Vector2_sel, smoothTime, maxSpee);
        }
        Vector2 pos = enemy.transform.position;
        pos.y = Enemy_pos_y;
        enemy.transform.position = pos;

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
    }
    public IEnumerator DamagePlayer()
    {
        PlayerStats.PlayerHealt -= 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
        

           }
}
