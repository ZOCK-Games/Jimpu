using UnityEngine;
using UnityEngine.UI;

public class UI_Opener : MonoBehaviour
{
    public GameObject Canvas_settings;
    public GameObject Canvas_garderobe;
    public Button Button_open;
    public Button Button_closer;
    public Button Garderobe_Button;

    void Start()
    {
        Canvas_settings.SetActive(false);
        Canvas_garderobe.SetActive(false);
        Button_open.onClick.AddListener(openButtonClick);
        Button_closer.onClick.AddListener(closeButtonClick);
        Garderobe_Button.onClick.AddListener(Garderobe_Button_clicked);
    }
    private void openButtonClick()
    {
        Canvas_settings.SetActive(true); 
    }
    private void closeButtonClick()
    {
        Canvas_settings.SetActive(false);
    }
    private void Garderobe_Button_clicked()
    {
        Canvas_garderobe.SetActive(true);
         Canvas_settings.SetActive(false);
    }
        


}
