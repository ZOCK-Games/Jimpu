using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public GameObject PickerCursor;
    public Color PickedColor;
    public RawImage ColorTexture;
    private Button ColorUIButton;
    public GameObject ColorUI;
    private bool IsPointerOnColorPicker;
    private InputSystem_Actions inputActions;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        ColorUIButton = ColorUI.GetComponent<Button>();
        inputActions.UI.Enable();
        ColorUIButton.onClick.AddListener(() =>
        {
            IsPointerOnColorPicker = !IsPointerOnColorPicker;
            if (IsPointerOnColorPicker)
            {
                StartCoroutine(PickColor());
            }
        });
    }
    private IEnumerator PickColor()
    {
        RectTransform rect = ColorUI.GetComponent<RectTransform>();
        while (IsPointerOnColorPicker)
        {
            Vector2 CursorPosition = inputActions.UI.Point.ReadValue<Vector2>();
            PickerCursor.transform.position = CursorPosition;
            yield return null;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, CursorPosition, null, out Vector2 localPoint))
            {
                float x = (localPoint.x - rect.rect.x) / rect.rect.width;
                float y = (localPoint.y - rect.rect.y) / rect.rect.height;
                Texture2D texture = (Texture2D)ColorTexture.texture;
                PickedColor = texture.GetPixelBilinear(x, y);
            }
        }
    }
}
