using UnityEngine;

public class UiInfoManager : MonoBehaviour
{
    public Animator ConnectedControllerAnimator;
    public Animator DisconnectedControllerAnimator;
    public Animator ScreenShootAnimator;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ControllerConnected()
    {
        ConnectedControllerAnimator.SetTrigger("Play");
    }
    public void ControllerDisconnected()
    {
        DisconnectedControllerAnimator.SetTrigger("Play");
    }
    public void ScreenShootCaptured()
    {
        ScreenShootAnimator.SetTrigger("Play");
    }
}
