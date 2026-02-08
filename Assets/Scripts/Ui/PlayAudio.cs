using UnityEngine;
using UnityEngine.UI;

public class PlayAudio : MonoBehaviour
{
    public void PlaySound(string AudioName)
    {
        AudioManager.instance.PlayAudio(AudioName, transform, true, 2);
    }
}
