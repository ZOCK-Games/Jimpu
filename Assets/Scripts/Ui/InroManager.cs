using UnityEngine;
using UnityEngine.SceneManagement;

public class InroManager : MonoBehaviour
{
    public GameObject InfoButton;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.UI.Enable();
    }
    void OnDisable()
    {
        inputActions.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (InfoButton.activeSelf && inputActions.UI.Submit.WasPerformedThisFrame())
        {
            SceneManager.LoadScene("Start Screen");
        }
    }
}
