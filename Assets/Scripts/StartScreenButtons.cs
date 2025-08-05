using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenButtons : MonoBehaviour //this script manges all buttons in Start 
{
    public SceneManager SceneManager;
    public string StartSceneName;
    [Header("Buttons")]
    public Button StartButton;
    public Button ShopButton;
    public Button SkinButton;
    public Button QuitButton;
    [Header("UI Elments to Open")]
    public GameObject StartObjekt;
    public String ShopCanvas;
    public GameObject Gaderobe;
    void Start()
    {
        StartButton.onClick.AddListener(ExecutingStart);
        ShopButton.onClick.AddListener(OpeningShop);
        SkinButton.onClick.AddListener(OpeningSkin);
        QuitButton.onClick.AddListener(ExecutingQuit);

    }
    public void ExecutingStart() //is executed when StartButton is being clicked
    {
        Debug.Log("Start Button Clicked Load Scene: " + StartSceneName);
        SceneManager.LoadScene(StartSceneName);
    }
    public void OpeningShop() //is executed when ShopButton is being clicked
    {
        DataPersitenceManger.Instance.SaveGame();
        Debug.Log("Automatic Save Aktivated!");
        StartObjekt.SetActive(false);
        SceneManager.LoadScene(ShopCanvas);
        Debug.Log(ShopCanvas + " Has been aktivated becaus of a button");
    }
    public void OpeningSkin() //is executed when SkinButton is being clicked
    {
        Gaderobe.SetActive(true);
        Debug.Log(Gaderobe.name + " Has been aktivated becaus of a button");
    }
    public void ExecutingQuit() //is executed when QuitButton is being clicked
    {
        Debug.LogWarning("Quit in Start has been presed Quiting game");
        Application.targetFrameRate = 1;
        Application.Quit();
    }


}
