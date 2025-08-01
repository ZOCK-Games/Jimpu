using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourPickerControl : MonoBehaviour
{
    public Image colorDisplay;
    public TMP_InputField HexInputField;
    public Color colorHex;

    public Button PickRedButton;
    public Button PickBlueButton;
    public Button PickWhiteButton;
    public Button PickPinkButton;
    public GarderobenScribt garderobenScribt;
    public TextMeshProUGUI FeedbackText;

    private void Start()
    {
        HexInputField.onEndEdit.AddListener(OnHexInputFieldChanged);

        PickRedButton.onClick.AddListener(() => PickColor("#A52A2A"));
        PickBlueButton.onClick.AddListener(() => PickColor("#4169E1"));
        PickWhiteButton.onClick.AddListener(() => PickColor("#FFFFFF"));
        PickPinkButton.onClick.AddListener(() => PickColor("#DDA0DD"));
        FeedbackText.text = "";
    }

    private void OnHexInputFieldChanged(string hex)
    {
        hex = hex.TrimStart('#');

        // Versuche Farbe zu parsen
        if (ColorUtility.TryParseHtmlString("#" + hex, out colorHex))
        {
            garderobenScribt.SkinShowcase.color = colorHex;
            colorDisplay.color = colorHex;
            Debug.Log("Hex value is now: " + hex);
        }
        else
        {
            Debug.LogError("Hex value is not a valid color: " + hex);
            FeedbackText.text = "Invalid color code!";
            StartCoroutine(TextDeaktivate());
        }
    }


    private void PickColor(string hex)
    {
        HexInputField.text = hex.TrimStart('#');
        OnHexInputFieldChanged(hex);
    }
    IEnumerator TextDeaktivate()
    {
        Debug.Log("TextDeaktivate coroutine started");
        yield return new WaitForSeconds(4f);
        FeedbackText.text = "";
        Debug.Log("TextDeaktivate coroutine ended. Text:" + FeedbackText.text);
    }
}
