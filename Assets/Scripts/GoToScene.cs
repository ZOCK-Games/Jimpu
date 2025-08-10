using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    public string SceneToGoTo;
    public Button Button;
    void Start()
    {
        Button.onClick.AddListener(ButtonPresed);

    }
    void ButtonPresed()
    {
        Debug.Log("Loaded scene: " + SceneToGoTo);
        SceneManager.LoadScene(SceneToGoTo);
    }
}
