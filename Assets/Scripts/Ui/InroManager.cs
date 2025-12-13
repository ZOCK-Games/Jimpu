using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InroManager : MonoBehaviour
{
    public GameObject InfoButton;
    private InputSystem_Actions inputActions;
    public GameObject StartInfo;
    public GameObject Intro;
    public VibrateControllerManager vibrateController;
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
        inputActions.Player.Enable();
    }
    void OnDisable()
    {
        inputActions.UI.Disable();
        inputActions.Player.Disable();

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

    // Update is called once per frame
    void Update()
    {
        if (InfoButton.activeSelf && inputActions.UI.Submit.WasPerformedThisFrame())
        {
            SceneManager.LoadScene("Start Screen");
        }
    }
}
