using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Schwebe_platform : MonoBehaviour
{
    public LayerMask Schwebe_platform1;
    public Collider2D Player_collider;
    public GameObject Schewbe_platform;
    public Vector2 Posiotion_aktuel;
    public Vector2 Position_1;
    public Vector2 Position_2;
    public int smoothTime;
    public int maxSpeed;
    public bool Collision_P;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Player_collider.IsTouchingLayers(Schwebe_platform1))
        {
            Collision_P = true;
        }
        else 
        {Collision_P = false;}
    }
    public IEnumerator Moving()
    {
        if (Collision_P == true)
        {
            transform.position = Vector2.SmoothDamp(Position_2, Position_1, ref Posiotion_aktuel, smoothTime, maxSpeed);
            yield return new WaitForSeconds(0.5f);
            Collision_P = false;

        }
    }
}
