using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StartCrowMoving : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Rigidbody2D rigidbody2D;
    public float WalkY;
    private Vector3 StartPosition;
    void Start()
    {
        WalkY = 8.6f;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        StartPosition = transform.position;
        StartCoroutine(SetDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.velocity.x < 0.1)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (navMeshAgent.velocity.x > 0.1)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        float Distance = Vector3.Distance(StartPosition, transform.position);
        if (Distance > 18)
        {
            Destroy(gameObject);
        }
        animator.SetFloat("X", navMeshAgent.velocity.x);
        animator.SetFloat("Y", navMeshAgent.velocity.x);
    }
    IEnumerator SetDestination()
    {
        float DestinationType = Random.Range(0, 6);

        bool FoundPosition = false;
        int Tries = 0;
        Vector2 DestinationPosition = new Vector2();
        while (!FoundPosition && Tries < 50)
        {
            float x = Random.Range(-50, 50);
            float y = WalkY;
            if (DestinationType < 1)
            {
                y = 35;
                DestinationPosition = new Vector2(x, y);
                FoundPosition = true;
            }
            else if (navMeshAgent.destination.y != 100)
            {

                Vector2 Pos = new Vector2(x, y);
                Collider2D hit = Physics2D.OverlapPoint(Pos);
                if (hit != null && hit is BoxCollider2D)
                {
                    DestinationPosition = Pos;
                    FoundPosition = true;
                    Debug.Log("Found Position");
                }
                Debug.Log("Found  no Position");
                Tries += 1;
            }
        }
        if (FoundPosition)
        {
            navMeshAgent.SetDestination(DestinationPosition);
        }
        yield return new WaitForSeconds(Random.Range(2, 10));
        StartCoroutine(SetDestination());
    }
}
