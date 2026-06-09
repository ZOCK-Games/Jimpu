using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using System.Drawing;

[System.Serializable]
public class StringElement
{
    public Transform pointA;
    public Transform pointB;
    public int SkinElement;
}

public class SkinChangerStrings : MonoBehaviour
{
    public class StringHolder : MonoBehaviour
    {
        public SkinElementDisplay normalDisplay;
        public SkinElementDisplayMain mainDisplay;
    }

    public SkinChangerManager skinChangerManager;
    public Transform parentTransform;
    public GameObject PrefabObject;
    public GameObject PrefabA;
    public GameObject PrefabB;
    public int MinPoint;

    /// <summary> Point A is The Normal Display And B the Main Display
    /// </summary>
    /// <param name="PointA"></param>
    /// <param name="PointB"></param>
    /// <returns>The two connected Points</returns>
    /// 
    public (GameObject PointA, GameObject PointB) ConnectElements(SkinElementDisplay normalDisplay, SkinElementDisplayMain mainDisplay, bool onlyNormal = false)
    {
        if (mainDisplay != null)
        {
            mainDisplay.normalDisplay = normalDisplay;
        }
        normalDisplay.connected = true;

        var Parent = new GameObject("StringParent").transform;
        Parent.transform.SetParent(parentTransform);
        Parent.transform.localScale = Vector3.one;
        Parent.localPosition = Vector3.zero;

        var holder = Parent.gameObject.AddComponent<StringHolder>();
        holder.normalDisplay = normalDisplay;
        holder.mainDisplay = mainDisplay;

        var pointA = Instantiate(PrefabA); // The Start Point
        var pointB = Instantiate(PrefabB); // The End Point


        pointA.transform.SetParent(Parent,true);
        pointA.name = "PointStartA";
        pointA.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Debug.Log("pointA position bevor" + normalDisplay.portCollider2d.transform.position);
        pointA.transform.position = normalDisplay.portCollider2d.transform.position;
        Debug.Log("pointA position avter" + pointA.transform.position);

        if (!onlyNormal)
        {
            pointB.transform.position = mainDisplay.collider2DInPort.transform.position;
        }
        else
        {
            pointB.transform.position = normalDisplay.portCollider2d.transform.position;
        }
        pointB.transform.SetParent(Parent, true);
        pointB.name = "PointEndB";
        pointB.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        pointB.tag = "StringPoint";

        float Distance = Vector3.Distance(pointA.transform.position, pointB.transform.position);
        int Points = (int)(Distance * 50); 

        if (Points < MinPoint)
        {
            Points = MinPoint;
        }

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

        return (pointA, pointB);
    }

    public async Task DestroyString(GameObject String)
    {
        // Clean up connection flags if present
        var holder = String.GetComponent<StringHolder>();
        if (holder != null)
        {
            if (holder.normalDisplay != null)
                holder.normalDisplay.connected = false;
            if (holder.mainDisplay != null)
            {
                holder.mainDisplay.normalDisplay = null;
                holder.mainDisplay.SkinElement = null;
            }
        }

        var PointB = String.transform.GetChild(1).gameObject;

        PointB.SetActive(false);


        for (int i = String.transform.childCount - 1; i >= 0; i = i - 3)
        {
            Destroy(String.transform.GetChild(i).gameObject);
            Destroy(String.transform.GetChild(i -1).gameObject);
            Destroy(String.transform.GetChild(i -2).gameObject);

            await Task.Delay(1); // Delay
        }
        Destroy(String);
    }
}

