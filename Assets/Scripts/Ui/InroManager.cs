using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InroManager : MonoBehaviour
{
    public GameObject InfoButton;
    private InputSystem_Actions inputActions;
    public GameObject StartInfo;
    public GameObject Intro;
    public VibrateControllerManager vibrateController;
    public Button ContinueButton;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        Intro.SetActive(false);
        StartInfo.SetActive(true);
        StartCoroutine(StartWarning());
    }
    void OnEnable()
    {
        inputActions.UI.Enable();
    }
    void OnDisable()
    {
        inputActions.UI.Disable();
    }
    IEnumerator StartWarning()
    {
        vibrateController.VibrateController(0.2f, 0.2f, 10);
        StartInfo.SetActive(true);
        Intro.SetActive(false);
        yield return new WaitForSeconds(2);
        StartInfo.SetActive(false);
        Intro.SetActive(true);
    }
    void Start()
    {
        ContinueButton.onClick.AddListener(() => SceneManager.LoadScene("Start Screen"));
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.UI.RightClick.WasPerformedThisFrame())
        {
            SceneManager.LoadScene("Start Screen");
        }
        if (inputActions.UI.Submit.WasPerformedThisFrame())
        {
            SceneManager.LoadScene("Start Screen");
        }
    }
}
