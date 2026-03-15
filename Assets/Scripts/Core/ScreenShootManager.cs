using System;
using UnityEngine;
using System.IO;
using System.Linq;

public class ScreenShootManager : MonoBehaviour
{
    public static ScreenShootManager instance {get; private set;}
    private InputSystem_Actions inputActions;
    public event Action TakeScreenShotEvent;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject SSM = new GameObject("ScreenShootManager");
        SSM.AddComponent<ScreenShootManager>();
        DontDestroyOnLoad(SSM);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
    public void TakeScreenShot()
    {
        TakeScreenShotEvent?.Invoke();
        string FullPath = Path.Combine(Application.persistentDataPath,"ScreenShots", $"{DateTime.Now:yy-MM-dd_HH-mm-ss}.png");
        ScreenCapture.CaptureScreenshot(FullPath);
        Debug.Log($"Saved Screen Shoot at: {FullPath}");
    }
}
