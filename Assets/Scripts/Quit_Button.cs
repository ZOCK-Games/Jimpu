using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public Button QuitButtonClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuitButtonClick.onClick.AddListener(ButtonClick);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonClick()
    {
        Debug.Log("Ending Game ...");
        Application.targetFrameRate = 1;
        Screen.brightness = 0f;
        UnloadSceneOptions unloadOptions = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects;
        Application.Quit();
    }
}
