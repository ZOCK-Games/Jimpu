using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    public string SceneToGoTo;
    public Button Button;
    public bool BlockInput;
    private InputSystem_Actions inputActions;
    private bool SceneLoading;
    void OnEnable()
    {
        if (Button == null)
        {
            return;
        }
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        if (!BlockInput)
        {
            inputActions.UI.Close.performed += ctx => ButtonPreset(SceneToGoTo);
        }
    }
    void OnDisable()
    {
        if (Button == null)
        {
            return;
        }
        inputActions.UI.Disable();

    }
    void Start()
    {
        if (Button == null)
        {
            return;
        }
        SceneLoading = false;
        Button.onClick.AddListener(() => ButtonPreset(SceneToGoTo));
    }
    public void ButtonPreset(string Scene)
    {
        if (!SceneLoading)
        {
            SceneLoading = true;
            Debug.Log("Loaded scene: " + Scene);
            SceneManager.LoadScene(Scene);
        }
    }
}
