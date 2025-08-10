using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenLinks : MonoBehaviour
{
    public string Link;
    public Button button;
    public AudioSource audioSource;
    void Start()
    {
        button.onClick.AddListener(OpenLink);
        
    }
    void OpenLink() //Open link
    {
        Debug.Log("Open Link: " + Link);
        audioSource.Play();
        Application.OpenURL(Link); 
        
    }
}
