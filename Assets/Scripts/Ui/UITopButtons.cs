using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITopButtons : MonoBehaviour
{
    public Button saveButton;
    public Button SettingsButton;
    public Button ToStartButton;
    public GameObject settingsCanvas;
    public DataPersitenceManger dataPersistenceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveButton.onClick.AddListener(SaveGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        ToStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start Screen");
        });
    }
    void SaveGame()
    {
        dataPersistenceManager.SaveGame();
        Debug.Log("Game saved successfully!");
        if (dataPersistenceManager == null)
        {
            dataPersistenceManager = DataPersitenceManger.Instance;
            if (dataPersistenceManager == null)
            {
                Debug.LogError("Data Persistence Manager is not initialized.");
                return;
            }
        }
    }
    void OpenSettings()
    {
        Debug.Log("Settings opened");
        if (settingsCanvas != null && !settingsCanvas.activeSelf)
        {
            settingsCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Settings Canvas is not assigned in the inspector!");
        }
    }

}
