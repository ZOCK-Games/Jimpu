using System;
using UnityEngine;
using System.IO;
using System.Linq;

public class ScreenShootManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        inputActions = new InputSystem_Actions();
        inputActions.Global.Enable();
        string ScreenShootPath = Path.Combine(Application.persistentDataPath,"ScreenShots");
        if (!Directory.Exists(ScreenShootPath))
        {
            Directory.CreateDirectory(ScreenShootPath);
        }
        inputActions.Global.ScreenShoot.performed += ctx => TakeScreenShot();
    }
    void OnDisable()
    {
        inputActions.Global.Disable();
    }
    void TakeScreenShot()
    {
        string FullPath = Path.Combine(Application.persistentDataPath,"ScreenShots", $"{DateTime.Now:yy-MM-dd_HH-mm-ss}.png");
        ScreenCapture.CaptureScreenshot(FullPath);
        Debug.Log($"Saved Screen Shoot at: {FullPath}");
    }
}
