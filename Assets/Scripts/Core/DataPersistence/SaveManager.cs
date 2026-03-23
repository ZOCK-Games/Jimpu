using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[System.Serializable]
public class DataSOs
{
    public PlayerDataSO playerDataSO;
    public JimpuListData JimpuListData;
    public UserSettingsSO userSettingsSO;
    public InventorDataSO inventorDataSO;
    public NoteBookListSO noteBookDataSO;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    public string secretKey = "ChangeOnPublish";
    public DataSOs dataSOs;
    public bool encrypt;
    private string DataSOsResourcePath = "SaveManager";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject SM = new GameObject("SaveManager");
        SM.AddComponent<SaveManager>();

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        if (DataSOsResourcePath != null)
        {
            List<ScriptableObject> scriptableObjects = Resources.LoadAll<ScriptableObject>(DataSOsResourcePath).ToList();
            DataSOs data = new DataSOs()
            {
                playerDataSO = scriptableObjects.OfType<PlayerDataSO>().FirstOrDefault(),
                JimpuListData =  scriptableObjects.OfType<JimpuListData>().FirstOrDefault(),
                userSettingsSO =  scriptableObjects.OfType<UserSettingsSO>().FirstOrDefault(),
                inventorDataSO =  scriptableObjects.OfType<InventorDataSO>().FirstOrDefault(),
                noteBookDataSO =  scriptableObjects.OfType<NoteBookListSO>().FirstOrDefault()
            };
            dataSOs = data;

        }
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


        WriteData(dataSOs.playerDataSO, "PlayerData.dat");
        WriteData(dataSOs.JimpuListData, "JimpuData.dat");
        WriteData(dataSOs.userSettingsSO, "SettingsData.dat");
        WriteData(dataSOs.inventorDataSO, "InventoryData.dat");
        WriteData(dataSOs.noteBookDataSO, "NoteBookData.dat");

        /// <summary>
        /// Writes the data from the Scriptable Objects
        /// to the .dat file with the FileName
        /// </summary>
        void WriteData(ScriptableObject scriptableObject, string FileName)
        {

            string JsonFile = JsonUtility.ToJson(scriptableObject, true);
            if (encrypt)
            {
                JsonFile = Encrypt(JsonFile);
            }

            /// <summary>
            /// Combines the PersistentDataPath with the file name
            /// to ensure cross-platform compatibility
            /// </summary>
            string FullPath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);

            File.WriteAllText(FullPath, JsonFile);
            Debug.Log($"Saved {FileName}");
        }

    }
    /// <summary>
    /// Loads All Currently added SOs
    /// </summary>
    public void Load()
    {
        LoadFile("/PlayerData.dat", dataSOs.playerDataSO);
        LoadFile("/JimpuData.dat", dataSOs.JimpuListData);
        LoadFile("/SettingsData.dat", dataSOs.userSettingsSO);
        LoadFile("/InventoryData.dat", dataSOs.inventorDataSO);

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
        string fullPath = UnityEngine.Application.persistentDataPath + fileName;

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
        if (instance.dataSOs.userSettingsSO.Language != null)
        {
            yield return LocalizationSettings.InitializationOperation;
            Locale desiredLocale = LocalizationSettings.AvailableLocales.GetLocale(instance.dataSOs.userSettingsSO.Language);
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
