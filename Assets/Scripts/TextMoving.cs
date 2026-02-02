using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Points
{
    public Transform A;
    public Transform B;
    public float Time;
    public float speed;
    public bool AutoTime;
}

public class TextMoving : MonoBehaviour
{
    [SerializeField] private List<Points> MovingPoints;
    [SerializeField] private Transform ObjectToMove;
    void Start()
    {
        StartCoroutine(Moving(MovingPoints[0], 0));
    }
    IEnumerator Moving(Points points, int CoroutineCount)
    {
        float elapsedTime = 0;
        Transform A = points.A;
        Transform B = points.B;
        float duration = 0;
        if (points.AutoTime)
        {
            float Distance = Vector3.Distance(A.position, B.position);
            duration = Distance / points.speed;
        }
        else
        {
            duration = points.Time;
        }
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ObjectToMove.transform.position = Vector3.Lerp(A.position, B.position, (elapsedTime / duration));
            yield return null;
        }
        CoroutineCount += 1;
        if (MovingPoints[CoroutineCount] != null)
        {
            yield return StartCoroutine(Moving(MovingPoints[CoroutineCount], CoroutineCount));
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.lightBlue;
        for (int i = 0; i < MovingPoints.Count; i++)
        {
            Gizmos.DrawLine(MovingPoints[i].A.position, MovingPoints[i].B.position);
        }
    }
}
