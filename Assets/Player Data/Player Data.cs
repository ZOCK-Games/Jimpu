using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int money;
    public int Skin = 0;
    public float[] position;
    public PlayerStats playerStats;
    public PlayerData(PlayerStats playerStats)
    {
        health = playerStats.PlayerHealt;
        money = playerStats.PlayerMoney;

        position = new float[2];
        position[0] = playerStats.PlayerPosition.x;
        position[1] = playerStats.PlayerPosition.y;
    }
}
