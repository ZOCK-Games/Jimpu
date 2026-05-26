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

    /// <summary> Point A is The Normal Display And B the Main Display
    /// </summary>
    /// <param name="PointA"></param>
    /// <param name="PointB"></param>
    /// <returns>The two connected Points</returns>
    /// 
    public void ConnectElements(SkinElementDisplay normalDisplay, SkinElementDisplayMain mainDisplay)
    {
        mainDisplay.normalDisplay = normalDisplay;

        var Parent = new GameObject().transform;
        Parent.transform.SetParent(parentTransform);
        Parent.transform.localScale = Vector3.one;

        var pointA = Instantiate(PrefabA); // The Start Point
        var pointB = Instantiate(PrefabB); // The End Point


        pointA.transform.position = normalDisplay.portCollider2d.transform.position;
        pointA.transform.SetParent(Parent);
        pointA.name = "PointStartA";
        pointA.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        pointA.tag = "StringPoint";

        pointB.transform.position = mainDisplay.collider2DInPort.transform.position;
        pointB.transform.SetParent(Parent);
        pointB.name = "PointEndB";
        pointB.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        pointB.tag = "StringPoint";

        float Distance = Vector3.Distance(pointA.transform.position, pointB.transform.position);
        int Points = (int)(Distance * 50); // the number defines how many points are spawned

        float width = PrefabObject.GetComponent<SpriteRenderer>().bounds.size.x;

        Rigidbody2D rigidbody2DBefore = null;

        for (int i = 0; i < Points; i++)
        {
            float t = (float)i / (Points - 1);
            var Position = Vector3.Lerp(pointA.transform.position, pointB.transform.position, t);
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
