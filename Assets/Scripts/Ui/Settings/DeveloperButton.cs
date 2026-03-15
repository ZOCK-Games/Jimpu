using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DeveloperButton : MonoBehaviour
{
        public Button developerButton;
    void Start()
    {
        developerButton.onClick.AddListener(OnDeveloperButtonClick);
        
    }

    void OnDeveloperButtonClick()
    {
        Debug.developerConsoleVisible = true;
        Debug.Log("Developer mode activated!");
        
    }
}
