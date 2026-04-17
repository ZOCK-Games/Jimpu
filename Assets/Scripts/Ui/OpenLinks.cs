using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenLinks : MonoBehaviour
{
    public string Link;
    public Button button;
    void Start()
    {
        button.onClick.AddListener(OpenLink);
        
    }
    void OpenLink() //Open link
    {
        Debug.Log("Open Link: " + Link);
        Application.OpenURL(Link); 
        
    }
}
