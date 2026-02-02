using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class CrowStartManager : MonoBehaviour
{
    public float MaxCrows;
    public float MinCrows;
    public GameObject CrowPrefab;
    public GameObject CrowContainer;
    public List<Transform> SpawnPoints;
    public List<NavMeshAgent> CrowAgents;
    void Start()
    {
        CrowAgents.Clear();
        StartCoroutine(AutoSpawn());
    }

    // Update is called once per frame
    IEnumerator AutoSpawn()
    {
        if (CrowContainer.transform.childCount != MaxCrows)
        {
            SpawnCrow();
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(AutoSpawn());
    }

    void SpawnCrow()
    {
        GameObject Crow = Instantiate(CrowPrefab);
        Crow.name = $"Crow ({CrowContainer.transform.childCount})";
        Crow.transform.parent = CrowContainer.transform;
        Vector3 Position = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;
        Crow.transform.position = Position;
        if (Crow.GetComponent<NavMeshAgent>() == null)
        {
            Crow.AddComponent<NavMeshAgent>();
        }
        NavMeshAgent CrowNav = Crow.GetComponent<NavMeshAgent>();
        CrowAgents.Add(CrowNav);
        NavMeshHit NavHit;
        if (NavMesh.SamplePosition(transform.position, out NavHit, 5.0f, NavMesh.AllAreas))
        {
            CrowNav.Warp(NavHit.position);
        }
        CrowNav.updateRotation = false;
        CrowNav.updateUpAxis = false;
    }
}
