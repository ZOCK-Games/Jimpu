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

    public string CurentItem;
    public string CurentScene;



    public GameData()
    {
    }
}
