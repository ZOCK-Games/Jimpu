using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_screen : MonoBehaviour
{
    public Button StartButton;
    public string StartSceneName;

    private void Start()
    {
        StartButton.onClick.AddListener(Start_Button_Clicked);
    }

    public void Start_Button_Clicked()
    {
        Debug.Log("Start Button Clicked Load Scene: " + StartSceneName);
        SceneManager.LoadScene(StartSceneName);
    }
}
