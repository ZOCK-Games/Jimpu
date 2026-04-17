using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class DanmpfWurmTraveler : DampfWurmManager
{
    public Vector3 goalPosition = Vector3.zero;
    private Vector3 startPosition;
    private CircleCollider2D circleCollider2D;
    public UnityEvent Event;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        canBeSeated = true;
        circleCollider2D = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = 10;
        SetHealth(3);
        canTakeDamage = true;
    }

    public override void SetIsSeated(bool Bool)
    {
        Debug.Log("Is Seated: " + Bool);
        base.SetIsSeated(Bool);
        if (Bool)
        {
            StartCoroutine(GoToGoalPosition());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void SetGoalPosition(Vector3 position)
    {
        goalPosition = position;
        startPosition = transform.position;
    }

    public IEnumerator GoToGoalPosition()
    {
        while (Vector3.Distance(transform.position, goalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, 1f * Time.deltaTime);
            yield return null;
        }
        Event.Invoke();
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition, goalPosition);
        Gizmos.color = Color.limeGreen;
    }
}
