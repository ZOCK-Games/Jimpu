using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    public Button showFPSButton;
    public TextMeshProUGUI fpsText;
    public bool isFPSVisible;

    public void LoadGame(SaveManager manager)
    {
        this.isFPSVisible = manager.userSettingsSO.ShowFPS;
    }

    public void SaveGame(SaveManager manager)
    {
        manager.userSettingsSO.ShowFPS = this.isFPSVisible;
    }

    void Start()
    {
        fpsText.text = isFPSVisible ? "true" : "false";
        showFPSButton.onClick.AddListener(() =>
        {
            ToggleFPSVisibility();
        });
        
    }

    // Update is called once per frame
    void ToggleFPSVisibility()
    {
        isFPSVisible = !isFPSVisible;
        fpsText.text = isFPSVisible ? "true" : "false";
    }
}
