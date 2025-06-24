using UnityEngine;
using UnityEngine.UI;

public class Quit_Button : MonoBehaviour
{
    public Button Quit_Button_click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Quit_Button_click.onClick.AddListener(Button_Click);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Button_Click()
    {
        Application.Quit();
        
        Debug.Log("Ending Game ...");
    }
}
