using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersitenceManger : MonoBehaviour
{
    [Header("File Storage Config")]
    private string FileName = "Jimpu.json";
    [Header("Encryption")]
    [SerializeField] private bool useEncryption = true; // <-- Hier steuerst du es im Inspector!
    private List<IDataPersitence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersitenceManger Instance { get; private set; }
    private GameData gameData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager in the scene.");
        }
        Instance = this;
    }
    private void Start()
    {
        // Übergib useEncryption an den FileDataHandler!
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, FileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void OnApplicationQuit()
    {
        SaveGame();
    }
    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // Lade die Daten aus der Datei
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No game data found, creating new game data.");
            NewGame();
        }

        foreach (IDataPersitence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.LoadGame(this.gameData);
            Debug.Log("Game data loaded. Curent Skin: " + this.gameData.SkinIndex);
        }
    }
    public void SaveGame() //save game data
    {
        foreach (IDataPersitence dataPersistenceObject in this.dataPersistenceObjects)
        {
            dataPersistenceObject.SaveGame(ref gameData);
            Debug.Log("Game data saved.");
        }
        dataHandler.Save(gameData);
    }
    private List<IDataPersitence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersitence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersitence>();

        return new List<IDataPersitence>(dataPersistenceObjects);
    }

}
