using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_screen : MonoBehaviour
{
    public Button StartButton;
    public GameObject GameObjektToAktivate;
    public GameObject StartCanvas;
    public List<GameObject> GameObjektToDeaktivate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartButton.onClick.AddListener(Start_Button_Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartCanvas.activeSelf)
        {
            GameObjektToDeaktivate[0].SetActive(false);
            GameObjektToDeaktivate[1].SetActive(false);
            GameObjektToDeaktivate[2].SetActive(false);
        }
    }
    public void Start_Button_Clicked()
    {
        StartCanvas.SetActive(false);
        GameObjektToAktivate.SetActive(true);
        GameObjektToDeaktivate[0].SetActive(true);
        GameObjektToDeaktivate[1].SetActive(true);
        GameObjektToDeaktivate[2].SetActive(true);
    }
}
