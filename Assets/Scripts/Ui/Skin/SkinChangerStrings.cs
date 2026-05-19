using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;

[System.Serializable]
public class StringElement
{
    public Transform pointA;
    public Transform pointB;
    public int SkinElement;
}

public class SkinChangerStrings : MonoBehaviour
{
    public SkinChangerManager skinChangerManager;
    public Transform parentTransform;
    public GameObject PrefabObject;
    public GameObject PrefabA;
    public GameObject PrefabB;
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
    void Start()
    {
        SetStringPosition(new Vector3(0, 0, 0), new Vector3(3, 1, 0));
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
                var Rigidbody2d = Object.GetComponent<Rigidbody2D>();
                var parent = Object.transform.parent;
                var Startpos = Rigidbody2d.position;
                bool foundPort = false;
                var A = parent.GetChild(0);
                var B = parent.GetChild(1);

                while (inputActions.UI.Click.IsPressed())
                {
                    float speed = 10f;

                    MousePos = inputActions.UI.Point.ReadValue<Vector2>();
                    WorldPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Mathf.Abs(Camera.main.transform.position.z)));
                    var targetPos = new Vector2(WorldPos.x, WorldPos.y);

                    var newPos = Vector2.Lerp(Rigidbody2d.position, targetPos, Time.deltaTime * speed);
                    Rigidbody2d.MovePosition(newPos);


                    await Task.Yield();
                }
                var closestDisplay = skinChangerManager.skinElementDisplays
                    .OrderBy(x => Vector3.Distance(x.Object.transform.position, Rigidbody2d.position))
                    .FirstOrDefault();

                if (closestDisplay != null)
                {
                    Debug.Log("Found closest display");

                    var colliderPort = closestDisplay.portCollider2d;

                    Rigidbody2d.linearVelocity = Vector2.zero;
                    Rigidbody2d.angularVelocity = 0f;

                    Rigidbody2d.position = colliderPort.transform.position;
                }
                else
                {
                    Debug.Log("No hit");
                }
            }
        }
    }

    void SetStringPosition(Vector3 PointA, Vector3 PointB)
    {
        var Parent = new GameObject().transform;
        Parent.transform.SetParent(parentTransform);
        Parent.transform.localScale = Vector3.one;

        var pointA = Instantiate(PrefabA);
        var pointB = Instantiate(PrefabB);


        pointA.transform.position = PointA;
        pointA.transform.SetParent(Parent);
        pointA.name = "PointStartA";
        pointA.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        pointA.tag = "StringPoint";

        pointB.transform.position = PointB;
        pointB.transform.SetParent(Parent);
        pointB.name = "PointEndB";
        pointB.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        pointB.tag = "StringPoint";

        float Distance = Vector3.Distance(PointA, PointB);
        int Points = (int)(Distance * 50); // the number defines how many points are spawned

        float width = PrefabObject.GetComponent<SpriteRenderer>().bounds.size.x;

        Rigidbody2D rigidbody2DBefore = null;

        for (int i = 0; i < Points; i++)
        {
            float t = (float)i / (Points - 1);
            var Position = Vector3.Lerp(PointA, PointB, t);
            var Point = Instantiate(PrefabObject);
            if (i == 0)
            {
                rigidbody2DBefore = pointA.GetComponent<Rigidbody2D>();
            }
            Point.transform.position = Position;
            Point.transform.SetParent(Parent);
            Point.name = $"Point{i}";
            Point.GetComponent<HingeJoint2D>().connectedBody = rigidbody2DBefore;
            DistanceJoint2D DistanceJoint = Point.GetComponent<DistanceJoint2D>();
            DistanceJoint.connectedBody = rigidbody2DBefore;
            DistanceJoint.autoConfigureDistance = false;
            DistanceJoint.distance = width;
            DistanceJoint.maxDistanceOnly = true;
            rigidbody2DBefore = Point.GetComponent<Rigidbody2D>();

        }
        pointB.GetComponent<HingeJoint2D>().connectedBody = rigidbody2DBefore;
        pointB.GetComponent<DistanceJoint2D>().connectedBody = rigidbody2DBefore;
    }
}
