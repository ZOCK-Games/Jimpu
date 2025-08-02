using System;

[Serializable]
public class GameData
{
    public string colorhex; //Gaderobe stuff
    public int SkinIndex; 

    public int Health; //player stuff
    public float PlayerPositionX;
    public float PlayerPositionY;
    public int CoinValue;

    public int EnemyHealth; //enemy stuff
    public float EnemyPositionX;
    public float EnemyPositionY;



    public GameData()
    {
        this.Health = 3;
        this.SkinIndex = 0;
    }
}
