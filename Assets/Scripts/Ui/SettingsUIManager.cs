using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public Button saveButton;
    public Button ToStartButton;
    public InputSystem_Actions inputActions;
    public GameObject SettingsOverlay;
    private bool UiActive;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
        inputActions.Player.Disable();
    }
    void Start()
    {
        UiActive = false;
        DisableUI();
        ToStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start Screen");
        });

        inputActions.Player.Settings.performed += ctx => ToggleUI();

    }
    public void ToggleUI()
    {
        UiActive = !UiActive;
        if (UiActive)
        {
            SetUIActive();
        }
        else
        {
            DisableUI();
        }
    }

    void SetUIActive()
    {
        SettingsOverlay.SetActive(true);
        PauseManager.instance.PauseGame();
    }
    void DisableUI()
    {
        SettingsOverlay.SetActive(false);
        PauseManager.instance.ResumeGame();
    }
    /*void SaveGame()
    {
        dataPersistenceManager.SaveGame();
        Debug.Log("Game saved successfully!");
        if (dataPersistenceManager == null)
        {
            dataPersistenceManager = DataPersitenceManger.Instance;
            if (dataPersistenceManager == null)
            {
                Debug.LogError("Data Persistence Manager is not initialized.");
                return;
            }
        }
    }/*/
}
