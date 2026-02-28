using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.iOS;
using UnityEngine.UI;

public class KeyBindsSettings : MonoBehaviour
{
    public Animator KeyBindsInOutAnimator;
    public Button KeyBindsOpen;
    public Button KeyBindsClose;
    public Button KeyBindsSave;
    public Button KeyBindsListen;
    public GameObject KeyBindsUi;
    public GameObject ChangeKeyBindUI;
    private InputSystem_Actions inputActions;
    [Header("Key Binds")]
    public Button Jump;
    public Button Interact;
    private bool SaveInput;
    private bool listeningInput;
    enum Key
    {
        Attack,
        Jump,
        Interact
    }
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
        SaveInput = false;
        KeyBindsUi.SetActive(false);
        ChangeKeyBindUI.SetActive(false);
        KeyBindsOpen.onClick.AddListener(() => { KeyBindsUi.SetActive(true); KeyBindsInOutAnimator.SetTrigger("In"); });
        KeyBindsClose.onClick.AddListener(() => StartCoroutine(Out()));
        KeyBindsSave.onClick.AddListener(() => SaveInput = true);
        KeyBindsListen.onClick.AddListener(() => listeningInput = true);
        Jump.onClick.AddListener(() => ChangeKeyBind(Key.Jump));
        Interact.onClick.AddListener(() => ChangeKeyBind(Key.Interact));
    }
    void ChangeKeyBind(Key key)
    {
        ChangeKeyBindUI.SetActive(true);
        TextMeshProUGUI keyText = ChangeKeyBindUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        string Key = null;
        listeningInput = true;
        System.IDisposable handle = null;
        handle = InputSystem.onAnyButtonPress.Call((control) =>
        {
            if (control.name == "escape")
            {
                EndListening();
                return;
            }
            if (control.device is not Mouse && control is UnityEngine.InputSystem.Controls.ButtonControl)
            {
                InputAction action = inputActions.FindAction(key.ToString());

                if (action != null)
                {
                    foreach (var b in action.bindings) Debug.Log($"Binding group: {b.groups}");
                    RemapAction(action, control.device, control.path);
                    keyText.text = control.displayName;
                    EndListening();
                }
            }
        });

        void EndListening()
        {
            listeningInput = false;
            ChangeKeyBindUI.SetActive(false);
            handle?.Dispose();
        }
    }

    public void RemapAction(InputAction action, InputDevice device, string newPath)
    {
        if (newPath.StartsWith("/"))
        {
            string deviceName = device is Gamepad ? "Gamepad" : "Keyboard";
            string keyName = newPath.Substring(newPath.LastIndexOf('/') + 1);
            newPath = $"<{deviceName}>/{keyName}";
        }

        string targetGroup = (device is Gamepad) ? "Gamepad" : "Keyboard&Mouse";


        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].groups.Contains(targetGroup))
            {
                action.Disable();
                action.ApplyBindingOverride(i, newPath);
                action.Enable();

                string json = action.actionMap.asset.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("Rebinds", json);
                PlayerPrefs.Save();

                Debug.Log($"Successful: {action.name} changed to {newPath}");
                return;
            }
        }
        Debug.LogWarning("no binding found");
    }

    IEnumerator Out()
    {
        KeyBindsInOutAnimator.SetTrigger("Out");
        yield return new WaitForSeconds(3.85f);
        KeyBindsUi.SetActive(false);
    }
}
