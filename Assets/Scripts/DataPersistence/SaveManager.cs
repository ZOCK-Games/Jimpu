using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    public PlayerDataSO playerDataSO;
    public JimpuListData JimpuListData;
    public UserSettingsSO userSettingsSO;
    public InventorDataSO inventorDataSO;
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
        string JsonFileInventorySettings = JsonUtility.ToJson(inventorDataSO);
        if (encrypt)
        {
            JsonFileInventorySettings = Encrypt(JsonFileInventorySettings);
        }
        File.WriteAllText(Application.persistentDataPath + "/InventoryData.dat", JsonFileInventorySettings);
        Debug.Log("Saved Inventory Data");
    }
    /// <summary>
    /// Loads All Currently added SOs
    /// </summary>
    public void Load()
    {
        LoadFile("/PlayerData.dat", playerDataSO);
        LoadFile("/JimpuData.dat", JimpuListData);
        LoadFile("/SettingsData.dat", userSettingsSO);
        LoadFile("/InventoryData.dat", inventorDataSO);

        IEnumerable<IDataPersitence> dataPersistenceObjects =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersitence>();

        foreach (IDataPersitence dataObject in dataPersistenceObjects)
        {
            dataObject.LoadData(this);
        }

        StartCoroutine(AfterLoad());
    }

    private void LoadFile(string fileName, ScriptableObject targetSO)
    {
        string fullPath = Application.persistentDataPath + fileName;

        if (File.Exists(fullPath))
        {
            string jsonContent = File.ReadAllText(fullPath);

            if (encrypt)
            {
                jsonContent = Encrypt(jsonContent);
            }

            JsonUtility.FromJsonOverwrite(jsonContent, targetSO);
            Debug.Log($"Loaded and Decrypted: {fileName}");
        }
        else
        {
            Debug.LogWarning($"Savefile {fileName} not found");
        }
    }



    IEnumerator AfterLoad()
    {
        if (instance.userSettingsSO.Language != null)
        {
            yield return LocalizationSettings.InitializationOperation;
            Locale desiredLocale = LocalizationSettings.AvailableLocales.GetLocale(instance.userSettingsSO.Language);
            if (desiredLocale != null)
            {
                LocalizationSettings.SelectedLocale = desiredLocale;
            }
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
