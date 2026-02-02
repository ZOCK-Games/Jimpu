using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITopButtons : MonoBehaviour
{
    public Button saveButton;
    public Button ToStartButton;
    public InputSystem_Actions inputActions;
    public GameObject SettingsOverlay;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }
    void Start()
    {
        SettingsOverlay.SetActive(false);
        ToStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start Screen");
        });
    }
    void Update()
    {
        if (inputActions.Player.Settings.WasPerformedThisFrame())
        {
            Debug.Log("DDD");
            SettingsOverlay.SetActive(true);
        }
        if (SettingsOverlay.activeSelf && !inputActions.UI.enabled)
        {
            inputActions.UI.Enable();
            inputActions.Player.Disable();
        }
        else if (!inputActions.Player.enabled)
        {
            inputActions.UI.Disable();
            inputActions.Player.Enable();
        }
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
