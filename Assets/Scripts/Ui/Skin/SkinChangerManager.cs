using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class SkinChangerManager : MonoBehaviour, IDataPersitence
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
        inputActions.UI.Click.performed += ctx => _ = CheckForPoint();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.zero, size + 0.2f);
    }

    async Task CheckForPoint(GameObject hitObject = null, SkinElementDisplay sourceDisplay = null)
    {
        Debug.Log("Checking.....");
        var MousePos = inputActions.UI.Point.ReadValue<Vector2>();
        var WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        var weltPos2D = new Vector2(WorldPos.x, WorldPos.y);
        Debug.Log(WorldPos);

        GameObject clickedObj = hitObject;
        var hit = Physics2D.Raycast(weltPos2D, Vector2.zero);
        if (clickedObj == null && hit.collider != null)
        {
            clickedObj = hit.collider.gameObject;
        }

        if (clickedObj != null)
        {
            Debug.Log("Found hit");

            bool isStringPoint = clickedObj.CompareTag("StringPoint");
            if (hitObject != null || isStringPoint)
            {
                GameObject Object = clickedObj;
                Debug.Log("Found obj");

                Rigidbody2D Rigidbody2dString = Object.GetComponent<Rigidbody2D>();
                var parent = Object.transform.parent;
                var stringParent = FindStringParent(Object);
                var Startpos = Rigidbody2dString.position;

                GameObject hitGameObject = clickedObj;

                SkinChangerStrings.StringHolder stringHolder = FindStringHolder(Object);
                SkinElementDisplay skinElementDisplay = sourceDisplay;
                if (skinElementDisplay == null && hitGameObject != null)
                {
                    skinElementDisplay = skinElementDisplays.Find(x =>
                        x.Object == hitGameObject ||
                        (x.portCollider2d != null && x.portCollider2d.gameObject == hitGameObject) ||
                        (x.Object != null && hitGameObject.transform.IsChildOf(x.Object.transform))
                    );
                }
                if (skinElementDisplay == null && stringHolder != null)
                {
                    skinElementDisplay = stringHolder.normalDisplay;
                }

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
                var targetType = skinElementDisplay != null ? skinElementDisplay.skinType : stringHolder?.normalDisplay?.skinType;
                var allTypeMains = targetType != null ? skinDisplayMains.FindAll(x => x.skinType == targetType) : new List<SkinElementDisplayMain>();

                if (allTypeMains != null)
                {
                    var closestDisplay = allTypeMains
                        .Where(x => x != null && x.collider2DInPort != null)
                        .OrderBy(x => Vector3.Distance(x.collider2DInPort.transform.position, Rigidbody2dString.position))
                        .FirstOrDefault();

                    float closestDistance = closestDisplay != null ? Vector3.Distance(closestDisplay.collider2DInPort.transform.position, Rigidbody2dString.position) : float.MaxValue;
                    if (closestDisplay != null && closestDistance < 1.5f)
                    {
                        Debug.Log("Found closest display");

                        var colliderPort = closestDisplay.collider2DInPort;
                        ClearMainConnection(closestDisplay);
                        closestDisplay.SkinElement = (skinElementDisplay != null) ? skinElementDisplay.skinElement : null;
                        if (skinElementDisplay != null)
                        {
                            closestDisplay.normalDisplay = skinElementDisplay;
                            skinElementDisplay.connected = true;
                            if (closestDisplay.sprite != null && skinElementDisplay.DisplayImage != null)
                            {
                                closestDisplay.sprite.sprite = skinElementDisplay.DisplayImage;
                            }
                        }

                        Rigidbody2dString.linearVelocity = Vector2.zero;
                        Rigidbody2dString.angularVelocity = 0f;

                        Rigidbody2dString.position = colliderPort.transform.position;
                    }
                    else
                    {
                        if (skinElementDisplay != null)
                        {
                            skinElementDisplay.connected = false;
                            DisconnectSourceDisplay(skinElementDisplay);
                        }
                        else if (stringHolder != null && stringHolder.normalDisplay != null)
                        {
                            stringHolder.normalDisplay.connected = false;
                            DisconnectSourceDisplay(stringHolder.normalDisplay);
                        }
                        else
                        {
                            Debug.Log("No matching skin element display found to disconnect");
                        }
                        GameObject destroyTarget = stringParent != null ? stringParent : parent?.gameObject;
                        if (destroyTarget != null)
                        {
                            await skinChangerStrings.DestroyString(destroyTarget);
                        }
                        else
                        {
                            Debug.LogWarning("DestroyString: could not determine destroy target for object " + Object.name);
                        }
                        Debug.Log("No hit returned to origin, distance=" + closestDistance);
                    }
                }
                else
                {
                    if (skinElementDisplay != null)
                    {
                        skinElementDisplay.connected = false;
                    }
                    else
                    {
                        Debug.Log("No matching skin element display found to disconnect");
                    }
                    if (stringParent != null)
                    {
                        _ = skinChangerStrings.DestroyString(stringParent);
                    }
                    else
                    {
                        Debug.LogWarning("DestroyString: StringParent not found for object " + Object.name);
                    }
                    Debug.Log("No hit returned to origin");
                }
            }
            else
            {
                var skinDisplay = skinElementDisplays.Find(x => x.portCollider2d == (hit.collider));
                if (skinDisplay != null && !skinDisplay.connected)
                {
                    var connection = skinChangerStrings.ConnectElements(skinDisplay, null, true);
                    GameObject PointB = connection.PointB;
                    _ = CheckForPoint(PointB, skinDisplay);
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
            if (string.IsNullOrEmpty(element.ID))
            {
                element.ID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(element);
#endif
            }
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

            SkinElementDisplay skinElement = ScriptableObject.CreateInstance<SkinElementDisplay>();

            skinElement.Object = prefab;
            skinElement.skinType = element.skinType;
            skinElement.DisplayImage = element.sprite;
            skinElement.portCollider2d = portCollider;
            skinElement.skinElement = element;


            skinElementDisplays.Add(skinElement);
        }
        foreach (SkinElementDisplayMain element in skinDisplayMains)
        {
            var normalDisplayList = skinElementDisplays.FindAll(x => x.skinType == element.skinType);
            var normalDisplay = normalDisplayList[Random.Range(0, normalDisplayList.Count)];
            if (normalDisplay != null)
            {
                ClearMainConnection(element);
                element.normalDisplay = normalDisplay;

                skinChangerStrings.ConnectElements(normalDisplay, element);
                element.SkinElement = normalDisplay.skinElement;
                if (element.sprite != null && normalDisplay.DisplayImage != null)
                {
                    element.sprite.sprite = normalDisplay.DisplayImage;
                }
            }
            else
            {
                Debug.Log("Nothing found to connect");
            }
        }
    }

    private GameObject FindStringParent(SkinElementDisplayMain main)
    {
        if (skinChangerStrings == null || skinChangerStrings.parentTransform == null)
            return null;

        for (int i = 0; i < skinChangerStrings.parentTransform.childCount; i++)
        {
            var child = skinChangerStrings.parentTransform.GetChild(i);
            var holder = child.GetComponent<SkinChangerStrings.StringHolder>();
            if (holder != null && holder.mainDisplay == main)
                return child.gameObject;
        }
        return null;
    }

    private GameObject FindStringParent(GameObject childObject)
    {
        if (childObject == null)
            return null;

        var current = childObject.transform;
        while (current != null)
        {
            var holder = current.GetComponent<SkinChangerStrings.StringHolder>();
            if (holder != null)
                return current.gameObject;
            current = current.parent;
        }
        return null;
    }

    private SkinChangerStrings.StringHolder FindStringHolder(GameObject childObject)
    {
        if (childObject == null)
            return null;

        var current = childObject.transform;
        while (current != null)
        {
            var holder = current.GetComponent<SkinChangerStrings.StringHolder>();
            if (holder != null)
                return holder;
            current = current.parent;
        }
        return null;
    }

    private void DisconnectSourceDisplay(SkinElementDisplay source)
    {
        if (source == null)
            return;

        var main = skinDisplayMains.Find(x => x.normalDisplay == source);
        if (main != null)
        {
            main.normalDisplay = null;
            main.SkinElement = null;
        }
    }

    private void ClearMainConnection(SkinElementDisplayMain main)
    {
        if (main == null)
            return;

        if (main.normalDisplay != null)
        {
            var stringParent = FindStringParent(main);
            if (stringParent != null)
            {
                _ = skinChangerStrings.DestroyString(stringParent);
            }
            else
            {
                main.normalDisplay.connected = false;
            }
            main.normalDisplay = null;
            main.SkinElement = null;
        }
    }

    public void LoadData(SaveManager manager)
    {
        if (manager == null || manager.dataSOs == null || manager.dataSOs.playerDataSO == null || manager.dataSOs.playerDataSO.bodyParts == null)
        {
            return;
        }

        foreach (var part in manager.dataSOs.playerDataSO.bodyParts)
        {
            if (string.IsNullOrEmpty(part.BodyPartType))
                continue;
            if (!System.Enum.TryParse(part.BodyPartType, out SkinType type))
                continue;

            var mainDisplay = skinDisplayMains.Find(x => x.skinType == type);
            if (mainDisplay == null)
                continue;

            ClearMainConnection(mainDisplay);

            if (string.IsNullOrEmpty(part.BodyElementID))
                continue;

            var element = skinElements.Find(e => e != null && e.ID == part.BodyElementID);
            if (element == null)
                continue;

            var normal = skinElementDisplays.Find(x => x != null && x.skinElement != null && x.skinElement.ID == element.ID);
            if (normal != null)
            {
                skinChangerStrings.ConnectElements(normal, mainDisplay);
                mainDisplay.SkinElement = element;
                mainDisplay.normalDisplay = normal;
                normal.connected = true;
                if (mainDisplay.sprite != null && normal.DisplayImage != null)
                {
                    mainDisplay.sprite.sprite = normal.DisplayImage;
                }
            }
            else
            {
                mainDisplay.SkinElement = element;
                if (mainDisplay.sprite != null && element.sprite != null)
                {
                    mainDisplay.sprite.sprite = element.sprite;
                }
            }
        }
    }

    public void SaveData(SaveManager manager)
    {
        List<BodyPart> bodyParts = new List<BodyPart>();

        foreach (var mainDisplay in skinDisplayMains)
        {
            BodyPart part = new BodyPart
            {
                BodyPartType = mainDisplay.skinType.ToString(),
                ColorHex = ColorUtility.ToHtmlStringRGB(mainDisplay.sprite.color),

            };

            if (mainDisplay.SkinElement != null)
            {
                part.BodyElementID = mainDisplay.SkinElement.ID;
            }
            ;

            bodyParts.Add(part);
        }
        manager.dataSOs.playerDataSO.bodyParts = bodyParts;
    }
}

public class SkinElementDisplay : ScriptableObject
{
    public GameObject Object;
    public Collider2D portCollider2d;
    public Sprite DisplayImage;
    public SkinType skinType;
    public SkinElement skinElement;
    public bool connected = false;

    void OnEnable()
    {
        connected = false;
    }
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