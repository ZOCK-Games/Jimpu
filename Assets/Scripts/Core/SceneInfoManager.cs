using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this script is used for 
/// Getting the type of the current scene and
/// to check if the scene changed 
/// </summary>

public class SceneInfoManager : MonoBehaviour
{
    public static SceneInfoManager instance { get; set; }
    public List<SceneSettings> sceneSettings = new List<SceneSettings>();
    private SceneSettings CurrentScene;
    public static event Action<SceneSettings> OnSceneChanged;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject gameObject = new GameObject("ScenManager");
        instance = gameObject.AddComponent<SceneInfoManager>();
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentScene = sceneSettings.Find(x => x.scene.name == scene.name);
        OnSceneChanged?.Invoke(CurrentScene);
    }
}
[System.Serializable]
public class SceneSettings
{
    public SceneAsset scene;
    public SceneTags tag;
}

public enum SceneTags
{
    none,
    Settings,
    Game,
    CutScene,
    Menu
}