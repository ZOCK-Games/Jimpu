using System.Collections;
using TMPro;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HousDoor : MonoBehaviour
{
    [Header("Animator Trigger Names")]
    public DialogManager dialogManager;
    public PlayerControll playerControll;
    public float InfoTextRange;
    public string GoToScene;
    public GameObject UILoading;
    public float LoadingTime;
    private bool CanOpenDoor;
    private InputSystem_Actions inputActions;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        CanOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        float Distance = Vector3.Distance(transform.position, playerControll.Player.transform.position);
        if (Distance < InfoTextRange && dialogManager.currentDialogData.dialog_id != "tutorial_dialog_house_001")
        {
            dialogManager.LoadDialog("Dialogs/The_Voice/Tutorial_Dialog/House_Dialog/House_Dialog_1");
        }
        if (playerControll.rb.IsTouching(this.gameObject.GetComponent<BoxCollider2D>()))
        {
            Debug.Log("Can Open Door");
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                StartCoroutine(LoadSceneAsync());
            }
        }

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
            LoadingText.text = $"{Mathf.RoundToInt(displayProgress *100)}%";
            LoadingSlider.value = displayProgress;
        }
        operation.allowSceneActivation = true;
    }

}
