using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HousDoor : MonoBehaviour
{
    [Header("Animator Trigger Names")]
    public PlayerControll playerControll;
    public float InfoTextRange;
    public string GoToScene;
    public GameObject UILoading;
    public float LoadingTime;
    public DeviceManger deviceManger;
    private bool CanOpenDoor;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();

    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += ctx =>
        {
            if (CanOpenDoor)
            {
                OpenApplication();
            }
        };
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        CanOpenDoor = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanOpenDoor = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanOpenDoor = false;
        }
    }
    void OpenApplication()
    {
        string path = Path.Combine(Application.streamingAssetsPath, $"Crash/{deviceManger.DesktopOS}/CrashWindow");
        Process.Start(path);
        Application.Quit();
    }

    IEnumerator LoadSceneAsync()
    {
        GameObject LoadingCanvas = Instantiate(UILoading);
        TextMeshProUGUI LoadingText = LoadingCanvas.transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>();
        Slider LoadingSlider = LoadingCanvas.transform.GetChild(2).transform.GetComponent<Slider>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(GoToScene);
        operation.allowSceneActivation = false;

        float StartTime = Time.time;
        while (operation.progress < 0.9f || Time.time < StartTime + LoadingTime)
        {
            float currentProgress = operation.progress;
            if (operation.progress < 0.9f)
            {
                currentProgress = 1;
            }
            yield return null;
            float timeElapsed = Time.time - StartTime;
            float timeProgress = timeElapsed / LoadingTime;
            float displayProgress = Mathf.Min(currentProgress, timeProgress);
            LoadingText.text = $"{Mathf.RoundToInt(displayProgress * 100)}%";
            LoadingSlider.value = displayProgress;
        }
        operation.allowSceneActivation = true;
    }

}
