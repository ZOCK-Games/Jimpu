using System;
using System.Numerics;

[Serializable]
public class GameData
{
    public int SkinIndex;     //everything that is saved in the game should be here
    public int CoinValue;
    public int Health;
    public float PlayerPositionX;
    public float PlayerPositionY;


    public GameData()
    {
        this.Health = 3;
        this.SkinIndex = 0;
        this.CoinValue = 0;
    }
}
