using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventManager : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject gameObject = new GameObject("SceneEventManager");
        gameObject.AddComponent<SceneEventManager>();
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.sceneUnloaded += OnUnLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneUnloaded -= OnUnLoaded;
    }
    void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene geladen: " + scene.name);
        foreach (var obj in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (obj is ISceneEvents e)
                e.OnSceneStart();
        }
    }

    void OnUnLoaded(Scene scene)
    {
        Debug.Log("Scene entladen: " + scene.name);
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is ISceneEvents e)
                e.OnSceneUnload();
        }
    }
}

public interface ISceneEvents
{
    void OnSceneStart();
    void OnSceneUnload();
}