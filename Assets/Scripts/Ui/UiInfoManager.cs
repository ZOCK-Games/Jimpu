using UnityEngine;

public class UiInfoManager : MonoBehaviour
{
    public static UiInfoManager instance { get; private set; }
    public Animator ConnectedControllerAnimator;
    public Animator DisconnectedControllerAnimator;
    public Animator ScreenShootAnimator;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        if (ScreenShootManager.instance == null)
        {
            Debug.LogError("ScreenShootManager instance is null!");
            return;
        }
        ScreenShootManager.instance.TakeScreenShotEvent += ScreenShootCaptured;

    }
    void OnDisable()
    {
        ScreenShootManager.instance.TakeScreenShotEvent -= ScreenShootCaptured;
    }
    public void ControllerConnected()
    {
        ConnectedControllerAnimator.SetTrigger("Play");
    }
    public void ControllerDisconnected()
    {
        DisconnectedControllerAnimator.SetTrigger("Play");
    }
    void ScreenShootCaptured()
    {
        ScreenShootAnimator.SetTrigger("Play");
    }
}
