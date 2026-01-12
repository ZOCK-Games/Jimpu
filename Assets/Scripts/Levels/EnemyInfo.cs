
using System.Collections;
using Unity.Mathematics;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInfo : MonoBehaviour
{
    public float EnemyHealt;
    public PlayerControll playerControll;
    public Transform Target;
    public GameObject JimpuObj;
    public float ViewField;
    public bool IsMoving;
    public Animator JimpuAnimator;
    private Rigidbody2D rb;
    private UniversalHealthInfo universalHealth;
    public NavMeshAgent meshAgent;
    public string Status = "null";
    void Start()
    {
        meshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        universalHealth = this.gameObject.GetComponent<UniversalHealthInfo>();
        JimpuObj = this.gameObject;
        JimpuAnimator = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
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
        if (meshAgent.isActiveAndEnabled)
        {
            meshAgent.Warp(transform.position);
            Target = TargetPosition.transform;
            Debug.LogError("Jimpu Is not placed on an Nav Mesh Surface");
        }
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
    }
}

