using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// this script is used for 
/// Getting the type of the current scene and
/// to check if the scene changed 
/// </summary>

public class SceneInfoManager : MonoBehaviour
{
    public static SceneInfoManager instance { get; set; }
    public sceneSetting sceneSettings;
    public string LastGameScene;
    public SceneSettings CurrentScene;
    public static event Action<SceneSettings> OnSceneChanged;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject gameObject = new GameObject("SceneInfoManager");
        instance = gameObject.AddComponent<SceneInfoManager>();
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        sceneSettings = Resources.Load<sceneSetting>("SceneSettings/SceneSetting");
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Resources.UnloadUnusedAssets(); // fixes memory 
        
        var sceneSetting = sceneSettings.sceneSettings.Find(x => x.sceneName == scene.name);
        if (sceneSetting != null && sceneSetting.tag == SceneTags.Game)
        {
            LastGameScene = scene.name;
            Debug.Log("New Last Game Scene");
        }
        else
        {
            LastGameScene = "GameTutorial";
        }
        CurrentScene = sceneSettings.sceneSettings.Find(x => x.sceneName == scene.name);
        OnSceneChanged?.Invoke(CurrentScene);
    }
}
[System.Serializable]
public class SceneSettings
{
    public string sceneName;
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
