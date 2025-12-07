using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindsSettings : MonoBehaviour
{
    public Animator KeyBindsInOutAnimator;
    public Button KeyBindsOpen;
    public Button KeyBindsClose;
    public GameObject KeyBindsUi;
    public GameObject ChangeKeyBindUI;
    private InputSystem_Actions inputActions;
    [Header("Key Binds")]
    public Button WalkLeft;
    public Button WalkRight;
    public Button Jump;
    public Button Interact;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();

    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        KeyBindsUi.SetActive(false);
        KeyBindsOpen.onClick.AddListener(() => { KeyBindsUi.SetActive(true); KeyBindsInOutAnimator.SetTrigger("In"); });
        KeyBindsClose.onClick.AddListener(() => StartCoroutine(Out()));

        WalkLeft.onClick.AddListener(() => StartCoroutine(ChangeKeyBind()));
    }
    IEnumerator ChangeKeyBind()
    {
        Debug.Log("Not Done Yet");
        yield return null;
    }

    // Update is called once per frame
    IEnumerator Out()
    {
        KeyBindsInOutAnimator.SetTrigger("Out");
        yield return new WaitForSeconds(3.85f);
        KeyBindsUi.SetActive(false);
    }
}
