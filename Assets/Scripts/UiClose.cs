using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiClose : MonoBehaviour
{
    public List<Button> CloseButton;
    public List<GameObject> ToClose;
    public GameData GameData;
    public string sceneName = "StartScene"; // Default scene name
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < CloseButton.Count; i++)
            CloseButton[i].onClick.AddListener(Close);
    }

    // Update is called once per frame
    void Close()
    {
        for (int i = 0; i < ToClose.Count; i++)
        {
            if (ToClose[i] != null)
            {
                ToClose[i].SetActive(false);
                GameData.Health = 3;
                Debug.Log("Closed: " + ToClose[i].name);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("ToClose list contains a null reference at index: " + i);
            }
        }
    }
}
