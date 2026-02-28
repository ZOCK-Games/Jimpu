using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour 
{
    /// <summary>
    /// This Script is used 
    /// by the Narrator but can be uses
    /// by any other object that uses NavMeshAgent
    /// </summary>
    public GameObject Narrator;
    public float SpeedMax;
    public float SpeedMin;
    public Transform MainPosition;
    private NavMeshAgent navMeshAgent;
    private bool IsGoingToPlayer;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, MainPosition.position);

        if (distance > 0f)
        {
            navMeshAgent.SetDestination(MainPosition.position);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, MainPosition.transform.position);
    }
}
