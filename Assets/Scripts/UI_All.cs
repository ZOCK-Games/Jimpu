using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UI_All : MonoBehaviour
{
    public GameObject to_open;
    public Canvas to_open_canvas;
    public Button Opener_Button;
    public List<GameObject> to_close_when_open;
    public List<GameObject> to_close_when_close;
    public Button Close_button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        to_open.SetActive(false);
        Opener_Button.onClick.AddListener(Open);
        Close_button.onClick.AddListener(close);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            to_close_when_close[0].SetActive(false);
            to_close_when_close[1].SetActive(false);
            to_close_when_close[2].SetActive(false);
            to_close_when_close[3].SetActive(false);
            to_close_when_close[4].SetActive(false);


        }

    }
    public void Open()
    {
        to_open.SetActive(true);
        to_open_canvas.enabled = true;
        to_close_when_open[0].SetActive(false);
        to_close_when_open[1].SetActive(false);
        to_close_when_open[2].SetActive(false);
        to_close_when_open[3].SetActive(false);
        to_close_when_open[4].SetActive(false);
    }
    public void close()
    {
        to_close_when_close[0].SetActive(false);
        to_close_when_close[1].SetActive(false);
        to_close_when_close[2].SetActive(false);
        to_close_when_close[3].SetActive(false);
        to_close_when_close[4].SetActive(false);
    }
    
    
}
