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
        PlayerPosition = Player.transform.position;
        playerData.Skin = SkinNumber;
    }
}
