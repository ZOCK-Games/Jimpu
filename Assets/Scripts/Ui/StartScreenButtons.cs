using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenButtons : MonoBehaviour //this script manges all buttons in Start 
{
    public SceneManager SceneManager;
    public string StartSceneName;
    [Header("Buttons")]
    public Button ShopButton;
    public Button SkinButton;
    public Button MultiplayerButton;
    public Button QuitButton;
    public Button SettingsButton;
    [Header("UI Elments to Open")]
    public GameObject StartObjekt;
    public String ShopCanvas;
    public string MultiplayerScene;
    public GameObject Gaderobe;
    public GameObject Settings;
    [SerializeField] private GameObject TransitionUi;
    void Start()
    {
        TransitionUi.SetActive(false);
        ShopButton.onClick.AddListener(OpeningShop);
        SkinButton.onClick.AddListener(OpeningSkin);
        QuitButton.onClick.AddListener(ExecutingQuit);
        SettingsButton.onClick.AddListener(OpeningSettings);
        MultiplayerButton.onClick.AddListener(OpeningSceneMuliplayer);
    }

    public void OpeningShop() //is executed when ShopButton is being clicked
    {
        Debug.Log("Automatic Save Aktivated!");
        SceneManager.LoadScene(ShopCanvas);
        Debug.Log(ShopCanvas + " Has been aktivated becaus of a button");
    }
    public void OpeningSkin() //is executed when SkinButton is being clicked
    {
        SceneManager.LoadScene("SkinChanger");

        Debug.Log(Gaderobe.name + " Has been aktivated becaus of a button");
    }
    public void ExecutingQuit() //is executed when QuitButton is being clicked
    {
        Debug.LogWarning("Quit in Start has been presed Quiting game");
        Application.targetFrameRate = 1;
        Application.Quit();
    }
    public void OpeningSceneMuliplayer()
    {
        Debug.Log("Loaded scene: " + MultiplayerScene);
        SceneManager.LoadScene(MultiplayerScene);

    }
    public void OpeningSettings()
    {
        Debug.Log("Opened: " + Settings.name);
        Settings.SetActive(true);

    }


}
