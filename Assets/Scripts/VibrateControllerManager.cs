
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrateControllerManager : MonoBehaviour
{
    public void VibrateController(float lowFrequency, float highFrequency, float duration)
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);

            StartCoroutine(StopVibration(duration));
        }
        else
        {
            Debug.Log("There is no gamepad");
        }
    }

    public IEnumerator StopVibration(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f); 
        }
    }
}
