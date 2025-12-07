using System.Collections;
using UnityEditor;
using UnityEngine;

public class FolowPlayer : MonoBehaviour
{
    public GameObject Narrator;
    public float SpeedMax;
    public float SpeedMin;
    public Transform MainPosition;
    private bool IsGoingToPlayer;
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, MainPosition.position);

        if (distance > 0f)
        {
            float speed = (SpeedMin + SpeedMax) / 2f;

            transform.position = Vector3.MoveTowards(
                transform.position,
                MainPosition.position,
                speed * Time.fixedDeltaTime
            );
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, MainPosition.transform.position);
    }
}
