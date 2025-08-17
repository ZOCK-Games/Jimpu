using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour, IDataPersitence
{
    public Button showFPSButton;
    public TextMeshProUGUI fpsText;
    public bool isFPSVisible = false;

    public void LoadGame(GameData data)
    {
        data.isFPSVisible = this.isFPSVisible;
    }

    public void SaveGame(ref GameData data)
    {
        data.isFPSVisible = this.isFPSVisible;
    }

    void Start()
    {
        showFPSButton.onClick.AddListener(() =>
        {
        DataPersitenceManger.Instance.LoadGame();
            ToggleFPSVisibility();
        });
        fpsText.text = isFPSVisible ? "false" : "true";
        
    }

    // Update is called once per frame
    void ToggleFPSVisibility()
    {
        isFPSVisible = !isFPSVisible;
        //kukt den status der FPS-Anzeige

        switch (isFPSVisible)
        {
            case true:
                fpsText.text = "true";
                DataPersitenceManger.Instance.SaveGame();
                break;
            case false:
                fpsText.text = "false";
                DataPersitenceManger.Instance.SaveGame();
                break;
            default:
                fpsText.text = "false";
                break;
        }
        
    }
}
