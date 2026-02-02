
using System.Collections;
using Unity.Mathematics;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.AI;
using WebSocketSharp;

public class EnemyInfo : MonoBehaviour
{
    public float EnemyHealt;
    public string JimpuID;
    public PlayerControll playerControll;
    public Transform Target;
    public GameObject JimpuObj;
    public float ViewField;
    public bool IsMoving;
    public Animator JimpuAnimator;
    private Rigidbody2D rb;
    public UniversalHealthInfo universalHealth;
    public NavMeshAgent meshAgent;
    public string Status = "null";
    void Start()
    {
        universalHealth = this.gameObject.GetComponent<UniversalHealthInfo>();
        if (EnemyHealt != 0)
        {
            universalHealth.Health = EnemyHealt;
        }
        meshAgent = GetComponent<NavMeshAgent>();
        JimpuObj = this.gameObject;
        JimpuAnimator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (JimpuID.IsNullOrEmpty())
        {
            JimpuID = System.Guid.NewGuid().ToString();
        }
        if (Target != null)
        {
            meshAgent.SetDestination(playerControll.Player.transform.position);
        }
        if (!meshAgent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
            {
                meshAgent.Warp(hit.position);
            }
        }
    }
    public bool IsPlayerInReach()
    {
        if (Target == null || JimpuObj == null) return false;

        float distSq = (Target.position - JimpuObj.transform.position).sqrMagnitude;
        float radiusSq = ViewField * ViewField;
        return distSq <= radiusSq;
    }

    public void SetTarget(GameObject TargetPosition)
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
        Target = TargetPosition.transform;
    }
    void Update()
    {
        meshAgent.SetDestination(Target.position);

        if (EnemyHealt <= 0)
        {
            EnemyScript.JimpusInfos.Remove(this.gameObject.GetComponent<EnemyInfo>());
            JimpuAnimator.Play("Death");
            Destroy(this.gameObject, 0.8f);
        }

        EnemyHealt = universalHealth.Health;
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
    public IEnumerator IsOnNavMesh()
    {
        yield return new WaitForSeconds(0.5f);
        if (!meshAgent.isOnNavMesh)
        {
            Destroy(gameObject);
        }
    }
}

