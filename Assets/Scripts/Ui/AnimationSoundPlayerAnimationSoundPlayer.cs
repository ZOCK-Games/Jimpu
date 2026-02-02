using UnityEngine;
using UnityEngine.UI;

public class AnimationSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public Button AudioButton;
    public void Start()
    {
        AudioButton.onClick.AddListener(PlaySound);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
