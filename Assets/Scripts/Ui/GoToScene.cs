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
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        if (!BlockInput)
        {
            inputActions.UI.Close.performed += ctx => ButtonPreset();
        }
    }
    void OnDisable()
    {

        inputActions.UI.Disable();

    }
    void Start()
    {
        SceneLoading = false;
        Button.onClick.AddListener(ButtonPreset);
    }
    void ButtonPreset()
    {
        if (!SceneLoading)
        {
            SceneLoading = true;
            Debug.Log("Loaded scene: " + SceneToGoTo);
            SceneManager.LoadScene(SceneToGoTo);
        }
    }
}
