using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DanmpfWurmTraveler : DampfWurmManager
{
    public Vector3 goalPosition = Vector3.zero;
    private Vector3 startPosition;
    private CircleCollider2D circleCollider2D;
    public UnityEvent EventOnGaol;
    private bool isTraveling;

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
        if (Bool && !isTraveling)
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
            Debug.Log($"Current: {transform.position}");
    Debug.Log($"Goal: {goalPosition}");
    Debug.Log($"Distance: {Vector3.Distance(transform.position, goalPosition)}");
        isTraveling = true;
        while (Vector3.Distance(transform.position, goalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, 1f * Time.deltaTime);
            yield return null;
        }
            Debug.Log("Invoke Event");
        isTraveling = false;

        EventOnGaol?.Invoke();
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition, goalPosition);
        Gizmos.color = Color.limeGreen;
    }
}
