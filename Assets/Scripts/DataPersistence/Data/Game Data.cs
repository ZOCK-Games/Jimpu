using System;
using System.Collections.Generic;

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
    //public List<EnemySaveData> enemies = new List<EnemySaveData>();
    public string CurentItem;
    public string CurentScene;
    //All the device information 
    public string Device;
    public string SimpleDevice;
    // FPS stuff
    public bool isFPSVisible;
    // Tutorial Settings
    public bool TutorialHasPlayed;



    public GameData()
    {
        isFPSVisible = false;
    }

}
