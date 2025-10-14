using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersitenceManger : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string FileName = "Jimpu.json";

    [Header("Encryption")]
    [SerializeField] private bool useEncryption = true;

    private List<IDataPersitence> dataPersistenceObjects = new List<IDataPersitence>();
    private FileDataHandler dataHandler;
    public static DataPersitenceManger Instance { get; private set; }
    private GameData gameData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Mehr als ein DataPersistenceManager in der Szene!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        // Liste schon hier initialisieren, um NullReference zu vermeiden
        dataPersistenceObjects = new List<IDataPersitence>();
    }

    private void Start()
    {
        // FileDataHandler erzeugen
        dataHandler = new FileDataHandler(Application.persistentDataPath, FileName, useEncryption);

        // Alle Objekte finden, die IDataPersitence implementieren
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Spielstand laden
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        if (dataHandler == null)
        {
            Debug.LogError("DataHandler ist null! Kann nicht laden.");
            return;
        }

        // Daten aus Datei laden
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("Keine Spieldaten gefunden, neues Spiel wird erstellt.");
            NewGame();
        }

        foreach (IDataPersitence dataPersistenceObject in dataPersistenceObjects)
        {
            if (dataPersistenceObject != null)
                dataPersistenceObject.LoadGame(gameData);
        }

        Debug.Log("Game data geladen. Curent Skin: " + gameData.SkinIndex);
    }

    public void SaveGame()
    {
        if (dataPersistenceObjects == null)
        {
            Debug.LogWarning("Keine IDataPersitence-Objekte gefunden. Speichern übersprungen.");
            return;
        }

        foreach (IDataPersitence dataPersistenceObject in dataPersistenceObjects)
        {
            if (dataPersistenceObject != null)
                dataPersistenceObject.SaveGame(ref gameData);
        }

        if (dataHandler != null)
            dataHandler.Save(gameData);
    }

    private List<IDataPersitence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersitence> foundObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                                    .OfType<IDataPersitence>();

        return new List<IDataPersitence>(foundObjects);
    }
}
