using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour, IDataPersitence
{
    public Button showFPSButton;
    public TextMeshProUGUI fpsText;
    public bool isFPSVisible;

    public void LoadGame(GameData data)
    {
        this.isFPSVisible = data.isFPSVisible;
    }

    public void SaveGame(ref GameData data)
    {
        data.isFPSVisible = this.isFPSVisible;
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
        DataPersitenceManger.Instance.SaveGame();
        DataPersitenceManger.Instance.LoadGame();
        fpsText.text = isFPSVisible ? "true" : "false";
    }
}
