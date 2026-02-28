using UnityEngine;

public interface IDataPersitence
{
    void LoadData(SaveManager manager);
    void SaveData(SaveManager manager);

}
