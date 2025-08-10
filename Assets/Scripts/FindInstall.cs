using UnityEngine;
using UnityEngine.UI;

public class FindInstall : MonoBehaviour
{
    public Button button;
    public FileDataHandler fileDataHandler;
    void Start()
    {
        button.onClick.AddListener(GetInstall);
        
    }
    void GetInstall()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
        Debug.Log("Jimpu Config" + Application.persistentDataPath);        
    }
}
