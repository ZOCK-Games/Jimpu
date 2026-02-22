using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class NooteBookManager : MonoBehaviour
{
    public GameObject linePrefab;
    public List<RectTransform> DrawingSites;
    public GameObject LineParent;
    public Camera PixelCamera;
    public ColorPicker colorPicker;
    private UILineRenderer currentLine;
    private List<Vector2> points = new List<Vector2>();
    private InputSystem_Actions inputActions;
    private bool isDrawing;
    [Space(1)]
    public Slider FontSizeSlider;
    private Vector2 mouseScreenPos;
    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        inputActions.UI.RightClick.performed += ctx =>
        {
            isDrawing = !isDrawing;
            if (!isDrawing)
            {
                CreateLine();
            }
        };
    }
    void CreateLine()
    {
        points.Clear();

        Transform parent = LineParent.transform;

        GameObject line = Instantiate(linePrefab, parent, false);

        RectTransform rt = line.GetComponent<RectTransform>();

        rt.SetParent(parent, false);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchoredPosition = Vector2.zero;
        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.sizeDelta = Vector2.zero;

        currentLine = line.GetComponent<UILineRenderer>();


        if (currentLine != null)
        {
            currentLine.LineThickness = FontSizeSlider.value;
            currentLine.color = colorPicker.PickedColor;
        }
    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.UI.Disable();
            inputActions.Dispose();
            inputActions = null;
        }
    }

    void Update()
    {
        if (isDrawing && currentLine != null)
        {
            mouseScreenPos = inputActions.UI.Point.ReadValue<Vector2>();

            if (IsOnPage(mouseScreenPos))
            {
                UpdateLine();
            }

        }
    }
    void UpdateLine()
    {
        RectTransform targetRect = currentLine.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, mouseScreenPos, Camera.main, out Vector2 localPoint))
        {
            if (IsOnPage(mouseScreenPos))
            {
                points.Add(localPoint);
                currentLine.Points = points.ToArray();
                currentLine.SetAllDirty();
            }
            else
            {
                Debug.Log("Not on screen: " + mouseScreenPos);
            }
        }
    }

    bool IsOnPage(Vector2 Position)
    {
        for (int i = 0; i < DrawingSites.Count; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(DrawingSites[i], Position))
            {
                return true;
            }
        }
        return false;
    }
}
