using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int money;
    public int Skin;
    public float[] position;

    public PlayerData(PlayerStats playerStats)
    {
        health = playerStats.PlayerHealt;
        money = playerStats.PlayerMoney;

        position = new float[2];
        position[0] = playerStats.PlayerPosition.x;
        position[1] = playerStats.PlayerPosition.y;
    }
}
