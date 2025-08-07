using UnityEngine;
using UnityEngine.UI;

public class SessionNavigation : MonoBehaviour
{
    public Button CreateSessionButton;
    public Button JoinSessionButton;
    public Button CloseButton;

    public GameObject MainUIWindow;
    public GameObject CreateSessionKomponents;
    public GameObject JoinSessionKomponents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateSessionButton.onClick.AddListener(CreateSession);
        JoinSessionButton.onClick.AddListener(JoinSesion);
        CloseButton.onClick.AddListener(Close);

    }

    // Update is called once per frame
    void CreateSession()
    {
        MainUIWindow.SetActive(true);
        CreateSessionKomponents.SetActive(true);
        JoinSessionKomponents.SetActive(false);
    }
    void JoinSesion()
    {
        MainUIWindow.SetActive(true);
        CreateSessionKomponents.SetActive(false);
        JoinSessionKomponents.SetActive(true);
    }
    void Close()
    {
        MainUIWindow.SetActive(false);
        CreateSessionKomponents.SetActive(false);
        JoinSessionKomponents.SetActive(false);
    }
}
