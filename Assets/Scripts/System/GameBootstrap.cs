using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private static GameObject player;
    private static GameObject cam;
    private static GameObject hud;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject go = new GameObject("GameBootstrap");
        DontDestroyOnLoad(go);
        go.AddComponent<GameBootstrap>();
    }

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        if (FindAnyObjectByType<playerControl>() == null)
        {
            player = Instantiate(Resources.Load<GameObject>("Player/Player"));
            DontDestroyOnLoad(player);
        }

        if (FindAnyObjectByType<PlayerHudUIManager>() == null)
        {
            hud = Instantiate(Resources.Load<GameObject>("UI/Player Hud Canvas"));
            DontDestroyOnLoad(hud);
        }

        if (FindAnyObjectByType<CameraManager>() == null)
        {
            cam = Instantiate(Resources.Load<GameObject>("Camera/MainCameraManager"));
            DontDestroyOnLoad(cam);
        }
        player.GetComponent<playerControl>().Init();
    }
}