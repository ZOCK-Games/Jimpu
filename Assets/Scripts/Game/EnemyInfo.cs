
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
using WebSocketSharp;

public class EnemyInfo : NPCManager
{
    public string JimpuID;
    public PlayerControll playerControll;
    public Transform Target;
    public GameObject JimpuObj;
    public float ViewField = 6;
    public bool IsMoving;
    public Animator JimpuAnimator;
    private Rigidbody2D rb;
    public NavMeshAgent meshAgent;
    public string Status = "null";

    protected override void Start()
    {
        base.Start();
        ViewField = 25;

        meshAgent = GetComponent<NavMeshAgent>();
        JimpuObj = this.gameObject;
        JimpuAnimator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (JimpuID.IsNullOrEmpty())
        {
            JimpuID = System.Guid.NewGuid().ToString();
        }
        if (!meshAgent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
            {
                meshAgent.Warp(hit.position);
            }
        }
        StartCoroutine(CheckDistanceRoutine());
    }
    public void SetTarget(Transform TargetPosition)
    {
        if (TargetPosition != null)
        {
            if (meshAgent == null)
            {
                meshAgent = GetComponent<NavMeshAgent>();
            }
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                meshAgent.Warp(hit.position);

            }
            Target = TargetPosition;
        }
        else
        {
            Debug.LogWarning("There is no TargetPosition on the Jimpu Manager!");
        }

    }
    protected void Update()
    {
        if (Target != null && meshAgent.isOnNavMesh)
        {
            meshAgent.SetDestination(Target.position);
        }
        JimpuAnimator.SetFloat("VelocityX", meshAgent.velocity.magnitude);
        JimpuAnimator.SetFloat("VelocityY", meshAgent.velocity.y);

        if (transform.position.x > playerControll.Player.transform.position.x)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (transform.position.x < playerControll.Player.transform.position.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        if (!meshAgent.isOnNavMesh)
        {
            StartCoroutine(IsOnNavMesh());
        }
    }
    IEnumerator CheckDistanceRoutine()
    {
        while (true)
        {
            if (meshAgent.isOnNavMesh)
            {
                float sqrDistance = (playerControll.Player.transform.position - transform.position).sqrMagnitude;
                meshAgent.isStopped = sqrDistance > (ViewField * ViewField);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public IEnumerator IsOnNavMesh()
    {
        yield return new WaitForSeconds(0.5f);
        if (!meshAgent.isOnNavMesh)
        {
            Destroy(gameObject);
        }
    }
}

