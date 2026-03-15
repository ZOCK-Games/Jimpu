using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour
{
    public GameObject FPSDisplayText;
    private bool ShowFPS = false;
    private InputSystem_Actions inputActions;
    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Global.ShowFps.performed += ctx => ToggleFps();
    }
    void ToggleFps()
    {
        ShowFPS = !ShowFPS;
        StartCoroutine(CheckFPS());

    }
    void OnDisable()
    {
        inputActions.Disable();
    }

    public void LoadGame(SaveManager manager)
    {
        this.ShowFPS = manager.dataSOs.userSettingsSO.ShowFPS;
    }


    void Start()
    {
        FPSDisplayText.SetActive(false);
    }

    public IEnumerator CheckFPS()
    {
        switch (ShowFPS)
        {
            case true:
                FPSDisplayText.SetActive(true);
                while (ShowFPS)
                {
                    FPSDisplayText.GetComponent<TextMeshProUGUI>().text = "FPS: " + math.round(1.0f / Time.deltaTime).ToString();
                    yield return new WaitForSeconds(0.2f);
                }
                FPSDisplayText.SetActive(false);
                break;
            case false:
                FPSDisplayText.SetActive(false);
                break;
        }

    }
}
