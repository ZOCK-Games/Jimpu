using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_screen : MonoBehaviour
{
    public Button StartButton;
    public string StartSceneName;
    [SerializeField] private GameObject TransitionUi;

    private void Awake()
    {
        TransitionUi.SetActive(false);
        StartButton.onClick.AddListener(() =>
            {
                StartCoroutine(StartButtonClicked());
            }
            );
    }

    public IEnumerator StartButtonClicked()
    {
        Debug.Log("Start Button Clicked Load Scene: " + StartSceneName);

        TransitionUi.SetActive(true);
        yield return new WaitForSecondsRealtime(30f);
        TransitionUi.SetActive(false);

    }
}
