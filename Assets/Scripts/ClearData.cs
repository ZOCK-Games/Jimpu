using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClearData : MonoBehaviour, IDataPersitence
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(DeleteData);
        
    }

    // Update is called once per frame
    void DeleteData()
    {

        
    }

    public void LoadGame(GameData data)
    {
    }

    public void SaveGame(ref GameData data)
    {
        data.CoinValue = 0;
        data.colorhex = Color.white.ToHexString();
        data.CurentItem = null;
        data.EnemyHealth = 5;
        data.EnemyPositionX = 0;
        data.EnemyPositionY = 0;
        data.Health = 3;
        data.SkinIndex = 0;
        Debug.Log("Reset data");
    }
}
