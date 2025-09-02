using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearData : MonoBehaviour, IDataPersitence
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(DeleteData);        
    }
    void DeleteData()
    {
        DataPersitenceManger.Instance.SaveGame();
        Debug.Log("Data cleared and new game started.");
    }


    public void SaveGame(ref GameData data)
    {
        data.CoinValue = 0;
        data.colorhex = Color.white.ToString();
        data.CurentItem = null;
        data.Health = 3;
        data.SkinIndex = 0;
        Debug.Log("Reset data");
    }

    public void LoadGame(GameData data)
    {

    }
}
