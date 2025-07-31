using UnityEngine;

public interface IDataPersitence
{
    
    void LoadGame(GameData data);
    void SaveGame(ref GameData data);

}
