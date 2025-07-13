using UnityEngine;
using UnityEngine.UI;

public class Simple_Opener : MonoBehaviour
{
    public Button OpenButton;
    public GameObject ToAktivate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpenButton.onClick.AddListener(ButtonClicked);


    }

    // Update is called once per frame
    void ButtonClicked()
    {
        ToAktivate.SetActive(true);

    }
    
}
