using System.Collections;
using NavMeshPlus.Components;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.AI;

public class DrunkenJimpuManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform DestinationPosition;
    public PlayerControll playerControll;
    public float Speed;
    public GameObject Botel;
    void Start()
    {
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(DestinationPosition.position);
    }
    void Update()
    {
        float Distance = Vector3.Distance(playerControll.Player.transform.position, this.gameObject.transform.position);
        if (Distance < 3)
        {
            ThrowBotel();
        }
    }
    public IEnumerator ThrowBotel()
    {
        yield return null;
        float Distance = Vector3.Distance(Botel.transform.position, playerControll.Player.transform.position);
        Vector3 StartPosition = Botel.transform.position;
        Vector3 GoalPostion =  playerControll.Player.transform.position;
        while (Distance > 2 && Distance < 15)
        {
            Botel.transform.position = Vector3.Slerp(StartPosition, GoalPostion, Distance * Speed);
        }
    }
}
