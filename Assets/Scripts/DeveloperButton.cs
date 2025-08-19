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
        Debug.Log("Developer button clicked!");
        Debug.developerConsoleVisible = true;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Debug.Log("Developer mode activated!");
        
    }
}
