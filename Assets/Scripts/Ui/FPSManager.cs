using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour, IDataPersitence
{
    public GameObject FPSDisplayText;
    private bool ShowFPS = false;

    public void LoadGame(GameData data)
    {
        this.ShowFPS = data.isFPSVisible;
    }

    public void SaveGame(ref GameData data)
    {
    }

    void Start()
    {
        FPSWayting();
        FPSDisplayText.SetActive(false);
        StartCoroutine(FPSUpdate());
    }

    // Update is called once per frame
    public void FPSWayting()
    {
        switch (ShowFPS)
        {
            case true:
                FPSDisplayText.SetActive(true);
                FPSDisplayText.GetComponent<TextMeshProUGUI>().text = "FPS: " + math.round(1.0f / Time.deltaTime).ToString() ;
                break;
            case false:
                FPSDisplayText.SetActive(false);
                break;

        }

    }
    IEnumerator FPSUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        FPSWayting();
        StartCoroutine(FPSUpdate());
    }
}
