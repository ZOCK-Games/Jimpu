using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkinChangerColor : MonoBehaviour
{
    public GameObject Brush;
    public List<ColorFields> colorFields = new List<ColorFields>();
    public TMP_InputField ColorTextField;
    public SkinChangerManager skinChangerManager;
    private InputSystem_Actions inputActions;
    public Color CurrentColor;
    public SkinElementDisplayMain lastMainDisplay;
    private Color lastMainDisplayColor;
    public bool Coloring;
    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += ctx => CheckForColorField();
        Coloring = false;
        ColorTextField.text = CurrentColor.ToString();
        colorFields[0].color = CurrentColor;
        Brush.SetActive(false);

        for (int i = 1; i < colorFields.Count; i++)
        {
            colorFields[i].ColorCollider.gameObject.GetComponent<SpriteRenderer>().color = colorFields[i].color;
        }
        ColorTextField.onSubmit.AddListener((string value) =>


{
    var hex = value.TrimStart('#');

    if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
    {
        CurrentColor = color;
        colorFields[0].color = color;
        colorFields[0].ColorCollider.gameObject.GetComponent<SpriteRenderer>().color = color;
        ColorTextField.text = hex;
    }
    else
    {
        ColorTextField.text = "";
    }
    ;
}
        );
    }
    void OnDisable()
    {
        inputActions.Disable();
    }

    public void CheckForColorField()
    {
        Vector2 ClickPos = inputActions.UI.Point.ReadValue<Vector2>();
        Vector2 WorldClickPos = Camera.main.ScreenToWorldPoint(ClickPos);
        var hit = Physics2D.Raycast(WorldClickPos, Vector2.zero);
        var field = colorFields.Find(x => x.ColorCollider == hit.collider);
        if (field != null)
        {
            Coloring = true;
            CurrentColor = field.color;
            ColorTextField.text = ColorUtility.ToHtmlStringRGB(CurrentColor);
        }
    }

    public void ColorizePlayerElements()
    {
    }

    void Update()
    {
        if (inputActions.UI.Click.IsPressed() && Coloring)
        {
            Vector2 ClickPos = inputActions.UI.Point.ReadValue<Vector2>();
            Vector2 WorldClickPos = Camera.main.ScreenToWorldPoint(ClickPos);
            var hit = Physics2D.Raycast(WorldClickPos, Vector2.zero);
            SkinElementDisplayMain mainDisplay = skinChangerManager.skinDisplayMains.Find(x => x.Object.GetComponent<Collider2D>() == hit.collider);
            if (mainDisplay != null && mainDisplay != lastMainDisplay)
            {
                if (lastMainDisplay != null && lastMainDisplay.sprite != null && lastMainDisplayColor != null)
                {
                    lastMainDisplay.sprite.color = lastMainDisplayColor;
                }
                mainDisplay.sprite.color = CurrentColor;

                lastMainDisplay = mainDisplay;
                lastMainDisplayColor = mainDisplay.sprite.color;
            }
        }
        if (inputActions.UI.Click.WasReleasedThisFrame() && Coloring)
        {
            Coloring = false;
            if (lastMainDisplay != null && lastMainDisplay.sprite != null)
            {
                lastMainDisplay.sprite.color = CurrentColor;
                lastMainDisplay = null;
            }
        }


    }
    void LateUpdate()
    {
        if (Coloring)
        {
            Brush.SetActive(true);

            Brush.GetComponentInChildren<SpriteRenderer>().color = CurrentColor;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(inputActions.UI.Point.ReadValue<Vector2>());
            Brush.transform.position = worldPos;
        }
        else if (Brush.activeSelf)
        {
            Brush.SetActive(false);
        }
    }

}

[System.Serializable]
public class ColorFields
{
    public Color color;
    public BoxCollider2D ColorCollider;
}