using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Opener : MonoBehaviour
{
    public Canvas Canvas_settings;
    public Canvas Canvas_garderobe;
    public Button Button_open;
    public Button Button_closer;
    public Button Garderobe_Button;

    void Start()
    {
        Canvas_settings.enabled = false;
        Button_open.onClick.AddListener(openButtonClick);
        Button_closer.onClick.AddListener(closeButtonClick);
        Garderobe_Button.onClick.AddListener(Garderobe_Button_clicked);
    }

    // Update is called once per frame
    void Update()
    {



    }
    private void openButtonClick()
    {
        Canvas_settings.enabled = true;
    }
    private void closeButtonClick()
    {
        Canvas_settings.enabled = false;
    }
    private void Garderobe_Button_clicked()
    {
        Canvas_garderobe.enabled = true;
         Canvas_settings.enabled = false;
    }
        


}
