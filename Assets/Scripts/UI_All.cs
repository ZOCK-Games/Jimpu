using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class UI_All : MonoBehaviour
{
    public List<GameObject> to_open;
    public List<Button> Opener_Button;
    [Space(10)]
    public List<GameObject> to_close_when_open;
    public List<GameObject> to_close_when_close;
    [Space(10)]
    public List<Button> Close_button;
    [Space(30)]
    [Header("At Start Settings")]
    [Space(10)]
    public bool DeaktivateAtStart = false;
    public bool DeaktivateCloseAtStart = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Opener_Button.Count; i++)
            Opener_Button[i].onClick.AddListener(Open);
        for (int i = 0; i < Close_button.Count; i++)
            Close_button[i].onClick.AddListener(close);
        if (DeaktivateAtStart == true)
        {
            for (int i = 0; i < to_open.Count; i++)
                to_open[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            for (int i = 0; i < to_close_when_close.Count; i++)
            to_close_when_close[i].SetActive(false);
            Debug.Log("Escape Has Closen" + to_close_when_close);  
        }

    }
    public void Open()
    {
        for (int i = 0; i < to_open.Count; i++)  
        to_open[i].SetActive(true);
        for (int i = 0; i < to_close_when_open.Count; i++)
        to_close_when_open[i].SetActive(false);
        Debug.Log("Open wen Closet has closen" + to_close_when_close);  
    }
    public void close()
    {
        for (int i = 0; i < to_close_when_close.Count; i++)
        to_close_when_close[i].SetActive(false);
                Debug.Log("Close wen Close closet" + to_close_when_close);  
    }
    
    
}
