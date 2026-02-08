using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
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
        ChangeKeyBindUI.SetActive(false);
        KeyBindsOpen.onClick.AddListener(() => { KeyBindsUi.SetActive(true); KeyBindsInOutAnimator.SetTrigger("In"); });
        KeyBindsClose.onClick.AddListener(() => StartCoroutine(Out()));

        WalkLeft.onClick.AddListener(() => StartCoroutine(ChangeKeyBind()));
    }
    IEnumerator ChangeKeyBind()
    {
        ChangeKeyBindUI.SetActive(true);
        TextMeshProUGUI keyText = ChangeKeyBindUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        string Key = null;
        bool listeningInput = true;
        while (listeningInput)
        {
            InputSystem.onAnyButtonPress.Call((control) =>
            {
                Key = control.path;
                keyText.text = control.name;
            });

            RemapAction(inputActions.Player.Attack, "Keyboard", "<keyboard>/a");
            yield return null;
        }
    }

    public void RemapAction(InputAction action, string group, string newPath)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].groups.Contains(group))
            {
                action.Disable();
                action.ApplyBindingOverride(i, newPath);
                action.Enable();
                Debug.Log($"{action.name} ({group}) geändert auf: {newPath}");
                return;
            }
        }
    }

    // Update is called once per frame
    IEnumerator Out()
    {
        KeyBindsInOutAnimator.SetTrigger("Out");
        yield return new WaitForSeconds(3.85f);
        KeyBindsUi.SetActive(false);
    }
}
