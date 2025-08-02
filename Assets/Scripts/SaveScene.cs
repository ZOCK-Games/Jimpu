using UnityEngine;

public class SaveSceneOnStart : MonoBehaviour
{
    public string sceneName;
    public bool LoadScene;
    public bool SaveSceneKey;
    void Start()
    {

        if (LoadScene == true)
        {
            DataPersitenceManger.Instance.LoadGame();
            Debug.Log("Autamtic Start Laden ausgelöst! Data");
        }
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) && SaveSceneKey == true) // Beispiel: F5 zum Speichern
        {
            if (DataPersitenceManger.Instance != null)
            {
                DataPersitenceManger.Instance.SaveGame();
                Debug.Log("Manuelles Speichern ausgelöst!");
            }
        }
    }
}
