using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearData : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(DeleteData);        
    }
    void DeleteData()
    {
        Debug.Log("Data cleared and new game started.");
    }


}
