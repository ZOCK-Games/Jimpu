using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InroManager : MonoBehaviour
{
    public GameObject InfoButton;
    private InputSystem_Actions inputActions;
    public GameObject StartInfo;
    public GameObject Intro;
    public bool CanGoToStart;
    public Button ContinueButton;
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
    void Start()
    {
        Intro.SetActive(false);
        StartInfo.SetActive(true);
        StartCoroutine(StartWarning());

        ContinueButton.onClick.AddListener(() =>
        {
            if (CanGoToStart)
            {
                VibrateControllerManager.instance.VibrateController(0.4f, 0.4f, 1);
                SceneManager.LoadScene("Start Screen");
            }
        });
        inputActions.UI.AnyKey.performed += ctx =>
        {
            if (CanGoToStart)
            {
                VibrateControllerManager.instance.VibrateController(0.4f, 0.4f, 1);
                SceneManager.LoadScene("Start Screen");
            }
        };
    }
    IEnumerator StartWarning()
    {
        CanGoToStart = false;
        ContinueButton.enabled = false;
        StartInfo.SetActive(true);
        Intro.SetActive(false);
        yield return new WaitForSeconds(2);
        VibrateControllerManager.instance.VibrateController(065f, 0.65f, 1);
        StartInfo.SetActive(false);
        Intro.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        ContinueButton.enabled = true;
        CanGoToStart = true;
    }
}
