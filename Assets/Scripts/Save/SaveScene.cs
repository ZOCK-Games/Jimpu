using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSceneOnStart : MonoBehaviour, IDataPersitence
{

    public bool LoadScene;
    public bool SaveSceneKey;

    public void LoadGame(GameData data)
    {
    }

    public void SaveGame(ref GameData data)
    {
        data.CurentScene = SceneManager.GetActiveScene().name;
    }
    void OnSceneUnloaded(Scene scene)
    {
            if (DataPersitenceManger.Instance != null)
            {
                DataPersitenceManger.Instance.SaveGame();
                Debug.Log("Manuelles Speichern ausgelöst!");
            }
    }
    public void Save()
    {
        DataPersitenceManger.Instance.SaveGame();
        Debug.Log("Saved Game");

    }
    

    void Awake()
    {
        if (LoadScene == true)
        {
            DataPersitenceManger.Instance.LoadGame();
            Debug.Log("Autamtic Start Laden ausgelöst! Data");
        }

    }    
}
