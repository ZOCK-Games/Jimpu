using UnityEngine;

public class PlayerHudUI : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        gameObject.SetActive(false);
        SceneInfoManager.OnSceneChanged += x => CheckScene(x);
    }
    void CheckScene(SceneSettings scene)
    {
        if (scene.tag == SceneTags.Game)
        {
            toggle(true);
        }
        else
        {
            toggle(false);
        }
    }

    void toggle(bool status)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(status);
        }
    }
}
