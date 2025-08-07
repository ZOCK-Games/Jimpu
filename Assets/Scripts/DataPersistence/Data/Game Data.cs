using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameData
{
    public string colorhex; //Garderobe stuff
    public int SkinIndex; 

    public int Health; //player stuff
    public float PlayerPositionX;
    public float PlayerPositionY;
    public int CoinValue;

    public int EnemyHealth; //enemy stuff
    public float EnemyPositionX;
    public float EnemyPositionY;

    public string CurentItem;
    public string CurentScene;
                                //All the device information 
    public string Device;
    public string SimpleDevice;



    public GameData()
    {
    }
}
