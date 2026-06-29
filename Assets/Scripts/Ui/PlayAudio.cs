using UnityEngine;
using UnityEngine.UI;

public class PlayAudio : MonoBehaviour
{
    public void PlaySound(string AudioName)
    {
        AudioManager.instance.PlayAudio(AudioName, transform, Vector2.zero, 0, true, 1.5f);
    }
}
