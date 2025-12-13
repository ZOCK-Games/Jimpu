using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public Button QuitButtonClick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuitButtonClick.onClick.AddListener(ButtonClick);

    }
    public void ButtonClick()
    {
        Debug.Log("Ending Game ...");
        Application.targetFrameRate = 1;
        Screen.brightness = 0f;
        Application.Quit();
    }
}
