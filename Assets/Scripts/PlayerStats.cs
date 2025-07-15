using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int PlayerMoney;
    public int PlayerHealt;
    public Vector2 PlayerPosition;
    public GameObject Player;
    public PlayerData playerData;
    public int SkinNumber = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerData.Skin = SkinNumber;
        PlayerPosition = Player.transform.position;
    }

}
