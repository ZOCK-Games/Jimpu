using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InputManagerController : MonoBehaviour
{
    public bool UIisVisible;
    private InputSystem_Actions inputActions;
    public List<GameObject> ButtonParent;
    private List<Button> Buttons;
    private int CurrentButton;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }
    void Start()
    {
        for (int i = 0; i < ButtonParent.Count; i++)
        {
            for (int x = 0; x < ButtonParent[i].transform.childCount; i++)
            {
                Button UIButton = ButtonParent[i].transform.GetChild(x).gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
                Buttons.Add(UIButton);
            }
        }

        CurrentButton = 0;
        Buttons[CurrentButton].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (UIisVisible)
        {
            inputActions.UI.Enable();
            inputActions.Player.Disable();
        }
        else
        {
            inputActions.Player.Enable();
            inputActions.UI.Disable();  
        }
    }
}
