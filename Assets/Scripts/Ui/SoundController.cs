using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Slider VolumeSlider;
    public AudioMixer AudioMusic;
    public TextMeshProUGUI VolumeText;
    public string volumeParameter = "Master"; // Name im AudioMixer

    void Update()
    {
        float dB = Mathf.Lerp(0f, 1f, VolumeSlider.value);
        AudioMusic.SetFloat(volumeParameter, dB);
        VolumeText.text = $"{ (VolumeSlider.value * 100f):F0}%";
    }
}
