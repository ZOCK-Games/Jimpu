using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    public PlayerDataSO playerDataSO;
    public JimpuListData JimpuListData;
    public UserSettingsSO userSettingsSO;
    public string secretKey = "ChangeOnPublish";
    public bool encrypt;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void OnApplicationQuit()
    {
        Save();
    }

    void Start()
    {
        Load();
    }
    /// <summary>
    /// Saves All Currently Added SOs 
    /// </summary>
    public void Save()
    {
        var dataPersistenceObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                         .OfType<IDataPersitence>().ToList();
        if (dataPersistenceObjects.Count == 0)
        {
            Debug.LogError("FEHLER: Keine Objekte mit IDataPersistence in der Szene gefunden!");
            return;
        }
        foreach (IDataPersitence dataObject in dataPersistenceObjects)
        {
            dataObject.SaveData(this);
            Debug.Log($"ERFOLG: {dataObject.GetType().Name} hat Daten ins SO geschrieben.");
        }

        string JsonFilePlayer = JsonUtility.ToJson(playerDataSO);
        if (encrypt)
        {
            JsonFilePlayer = Encrypt(JsonFilePlayer);
        }

        File.WriteAllText(Application.persistentDataPath + "/PlayerData.dat", JsonFilePlayer);
        Debug.Log("Saved Player Data");

        string JsonFileJimpu = JsonUtility.ToJson(JimpuListData);
        if (encrypt)
        {
            JsonFileJimpu = Encrypt(JsonFileJimpu);
        }
        File.WriteAllText(Application.persistentDataPath + "/JimpuData.dat", JsonFileJimpu);
        Debug.Log("Saved Jimpu Data");

        string JsonFileJUserSettings = JsonUtility.ToJson(userSettingsSO);
        if (encrypt)
        {
            JsonFileJUserSettings = Encrypt(JsonFileJUserSettings);
        }
        File.WriteAllText(Application.persistentDataPath + "/SettingsData.dat", JsonFileJUserSettings);
        Debug.Log("Saved User Settings Data");
    }
    /// <summary>
    /// Loads All Currently added SOs
    /// </summary>
    public void Load()
    {
        IEnumerable<IDataPersitence> dataPersistenceObjects =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersitence>();

        foreach (IDataPersitence dataObject in dataPersistenceObjects)
        {
            dataObject.LoadData(this);
        }

        string PlayerPath = Application.persistentDataPath + "PlayerData.json";
        if (File.Exists(PlayerPath))
        {
            JsonUtility.FromJsonOverwrite(PlayerPath, playerDataSO);
        }

        string JimpuData = Application.persistentDataPath + "JimpuData.json";
        if (File.Exists(JimpuData))
        {
            JsonUtility.FromJsonOverwrite(JimpuData, JimpuListData);
        }

        string SettingsPath = Application.persistentDataPath + "SettingsData.json";
        if (File.Exists(SettingsPath))
        {
            JsonUtility.FromJsonOverwrite(SettingsPath, userSettingsSO);
        }
    }

    public string Encrypt(string text)
    {
        string output = "";
        for (int i = 0; i < text.Length; i++)
        {
            output += (char)(text[i] ^ secretKey[i % secretKey.Length]);
        }
        return output;
    }



}
