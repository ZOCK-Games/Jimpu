using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class SkinChangerManager : MonoBehaviour
{
    public SkinChangerStrings skinChangerStrings;
    [Tooltip("Add all your Skin data elements")]
    public List<SkinElement> skinElements; // The ScriptableObject Data
    public GameObject prefabSkinElementDisplay; // The Displays Prefab for every skinElementDisplays
    public Transform SkinElementParent;
    public List<SkinElementDisplay> skinElementDisplays = new List<SkinElementDisplay>(); // The Displays for every ScriptableObject(Skin Element)
    public List<SkinElementDisplayMain> skinDisplayMains = new List<SkinElementDisplayMain>();
    public List<Vector3> positions = new List<Vector3>();
    private float size = 2.3f;
    private float displayMaxDistance = 2f;
    private InputSystem_Actions inputActions;
    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += ctx => CheckForPoint(ctx);
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.zero, size + 0.2f);
    }

    async Task CheckForPoint(CallbackContext ctx)
    {
        Debug.Log("Checking.....");
        var MousePos = inputActions.UI.Point.ReadValue<Vector2>();
        var WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        var weltPos2D = new Vector2(WorldPos.x, WorldPos.y);
        Debug.Log(WorldPos);
        var hit = Physics2D.Raycast(weltPos2D, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("Found hit");

            if (hit.collider.gameObject.tag == "StringPoint")
            {
                Debug.Log("Found obj");
                var Object = hit.collider.gameObject;
                Rigidbody2D Rigidbody2dString = Object.GetComponent<Rigidbody2D>();
                var parent = Object.transform.parent;
                var Startpos = Rigidbody2dString.position;
                var skinElementDisplay = skinElementDisplays.Find(x => x.Object == hit.collider.gameObject);
                
                while (inputActions.UI.Click.IsPressed())
                {
                    float speed = 10f;

                    MousePos = inputActions.UI.Point.ReadValue<Vector2>();
                    WorldPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Mathf.Abs(Camera.main.transform.position.z)));
                    var targetPos = new Vector2(WorldPos.x, WorldPos.y);

                    var newPos = Vector2.Lerp(Rigidbody2dString.position, targetPos, Time.deltaTime * speed);
                    Rigidbody2dString.MovePosition(newPos);


                    await Task.Yield();
                }
                var closestDisplay = skinDisplayMains
                    .OrderBy(x => Vector3.Distance(x.collider2DInPort.transform.position, Rigidbody2dString.position))
                    .FirstOrDefault();

                if (closestDisplay != null)
                {
                    Debug.Log("Found closest display");

                    var colliderPort = closestDisplay.collider2DInPort;
                    closestDisplay.SkinElement = skinElementDisplay.skinElement;

                    Rigidbody2dString.linearVelocity = Vector2.zero;
                    Rigidbody2dString.angularVelocity = 0f;

                    Rigidbody2dString.position = colliderPort.transform.position;
                }
                else
                {
                    Debug.LogWarning("No hit");
                }
            }
        }
    }
    void Start()
    {

        for (int i = 0; i < skinElements.Count; i++)
        {
            float dgre = (i / (float)skinElements.Count) * 360f;

            float angleRad = dgre * Mathf.Deg2Rad;

            float x = 0 + (size * Mathf.Cos(angleRad)) * 1.2f;
            float y = 0 + (size * Mathf.Sin(angleRad)) / 1.2f;

            Vector3 vector3 = new Vector3(x, y, 0);
            vector3 = vector3 * Random.Range(0.8f, 1.2f); // adds a bit of noise

            positions.Add(vector3);
        }
        float elementMultiplayer = 1;
        float distance = Vector3.Distance(positions[0], positions[1]);
        if (distance <= displayMaxDistance)
        {
            elementMultiplayer = distance / displayMaxDistance;
        }


        foreach (SkinElement element in skinElements)
        {
            var prefab = Instantiate(prefabSkinElementDisplay);
            var spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>();
            var portCollider = prefab.GetComponentInChildren<Collider2D>();  //searches for the collider which is used as a port
            prefab.transform.SetParent(SkinElementParent);
            prefab.transform.localScale = prefab.transform.localScale * (elementMultiplayer * Random.Range(0.8f, 1.2f)); // adds a bit of noise


            if (spriteRenderer == null)
            {
                Debug.Log("There is no sprite on prefab skin element display");
                continue;
            }
            var position = Vector3.one;
            if (positions.Count > 0)
            {
                position = positions[0];
            }
            positions.Remove(position);
            prefab.transform.position = position;
            spriteRenderer.sprite = element.sprite;

            SkinElementDisplay skinElement = new SkinElementDisplay
            {
                Object = prefab,
                skinType = element.skinType,
                DisplayImage = element.sprite,
                portCollider2d = portCollider,
                skinElement = element,

            };

            skinElementDisplays.Add(skinElement);
        }
        // Connects the Skin Element Display With the main skin elements
        foreach (SkinElementDisplay element in skinElementDisplays)
        {
            var mainDisplay = skinDisplayMains.Find(x => x.normalDisplay == null && x.skinType == element.skinType);
            if (mainDisplay != null)
            {
                mainDisplay.normalDisplay = element;

                skinChangerStrings.ConnectElements(element, mainDisplay);
            }
            else
            {
                Debug.Log("Nothing found to connect");
            }
        }
    }

}

public class SkinElementDisplay : ScriptableObject
{
    public GameObject Object;
    public Collider2D portCollider2d;
    public Sprite DisplayImage;
    public SkinType skinType;
    public SkinElement skinElement;
}
[System.Serializable]

public class SkinElementDisplayMain
{
    public Collider2D collider2DInPort;
    public GameObject Object;
    public SkinType skinType;
    public SpriteRenderer sprite;
    public SkinElement SkinElement;
    public SkinElementDisplay normalDisplay;
}
public enum SkinType
{
    Head,
    Body,
    ArmL,
    ArmR,
    LegL,
    LegR
}