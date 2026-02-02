using System.Collections;
using NavMeshPlus.Components;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Matchmaker.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DrunkenJimpuManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public PlayerControll playerControll;
    public float Speed;
    private Rigidbody2D rigidbody2D;
    public Animator Animation;
    public GameObject Botel;
    public Transform BotelContainer;
    public TextMeshPro HealthText;
    private bool IsAttacking;
    private UniversalHealthInfo universalHealth;
    void Start()
    {
        universalHealth = this.gameObject.GetComponent<UniversalHealthInfo>();
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        this.gameObject.transform.rotation = new quaternion(0, 0, 0, 0);
        IsAttacking = false;
    }
    void Update()
    {
        navMeshAgent.SetDestination(playerControll.Player.transform.position);
        float Distance = Vector3.Distance(playerControll.Player.transform.position, this.gameObject.transform.position);
        if (Distance < 1.5 && !IsAttacking)
        {
            navMeshAgent.isStopped = true;
            StartCoroutine(Attack());
        }
        if (transform.position.x > playerControll.Player.transform.position.x)
        {
            // The player is on his right side
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (transform.position.x < playerControll.Player.transform.position.x)
        {
            // The player is on his left side
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        if (universalHealth.Health <= 0)
        {
            StopAllCoroutines();
        }
        HealthText.text = universalHealth.Health.ToString();

        Animation.SetFloat("Walk", rigidbody2D.linearVelocityX);
        Animation.SetFloat("Jump", rigidbody2D.linearVelocityY);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Animation.SetBool("Grounded", true);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Animation.SetBool("Grounded", false);
        }
    }
    public IEnumerator Attack()
    {
        IsAttacking = true;
        Animation.Play("JimpuAttackingWithBotel");
        yield return new WaitForSeconds(5f);
        StartCoroutine(ThrowBotel());
    }
    public IEnumerator ThrowBotel()
    {
        Animation.Play("JimpuTrowingBottel");
        GameObject BotelPrefab = Instantiate(Botel);
        BotelPrefab.transform.position = Botel.transform.position;
        BotelPrefab.transform.SetParent(BotelContainer);
        Rigidbody2D rb = BotelPrefab.AddComponent<Rigidbody2D>();
        BotelPrefab.AddComponent<CapsuleCollider2D>();

        Vector2 VelocityUP = new Vector2((playerControll.transform.position.x - BotelPrefab.transform.position.x) / 3, 2);
        rb.linearVelocity = (VelocityUP * Speed) / 1.8f;
        yield return new WaitForSeconds(0.15f);
        Vector2 Velocity = playerControll.transform.position - BotelPrefab.transform.position;
        rb.linearVelocity = Velocity * Speed;
        IsAttacking = false;
    }
}
