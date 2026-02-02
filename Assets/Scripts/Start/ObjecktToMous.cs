using UnityEngine;

public class ObjecktToMous : MonoBehaviour
{
    public GameObject ObjektToMove;
    public bool MoveObjToCursor;
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveObjToCursor == true)
        {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Abstand der Kamera zum Objekt
        ObjektToMove.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        
    }
}
