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
    public BoxCollider2D Hit_box_player;
    public BoxCollider2D Hit_box_enemy_body;
    public BoxCollider2D Hit_box_enemy_head;
    public List<Image> Heart;
    public float Heart_count;
    private bool canTakeDamage = true;
    public GameObject Player_death_screen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Heart_count = 0;
        Player_death_screen.SetActive(false);

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
        if (Heart_count == 1)
            Heart[0].enabled = false;
        if (Heart_count == 2)
            Heart[1].enabled = false;
        if (Heart_count == 3)
            Heart[2].enabled = false;

        if (Heart_count < 1)
            Heart[0].enabled = true;
        if (Heart_count < 2)
            Heart[1].enabled = true;
        if (Heart_count < 3)
            Heart[2].enabled = true;


        if (Hit_box_enemy_head.IsTouching(Hit_box_player) && Input.GetKey(KeyCode.LeftShift))
        {
            //PLayer attacke
        }
        if (Heart_count == 3)
            Player_death_screen.SetActive(true);
        }
    public IEnumerator DamagePlayer()
    {
        Heart_count += 1;
        canTakeDamage = false;
        yield return new WaitForSeconds(3);
        canTakeDamage = true;
        

           }
}
