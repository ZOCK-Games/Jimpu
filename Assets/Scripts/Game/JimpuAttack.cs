using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Services.Lobbies.Models;

public class EnemyAttackController : MonoBehaviour
{
    // --- UNITY-REFERENCES ---
    [Header("Target & Movement")]

    [Header("Attack Settings")]
    public float sichtRadius = 10f;
    public float loadTime = 1f;
    public float decisionTime = 3f;
    public float travelDuration = 3f;
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private EnemyScript enemyScript;

    private GameObject Bullet;
    private Animator BulletAnimator;
    private GameObject Jimpu;
    private Transform Target;
    private bool Shoting;
    private CapsuleCollider2D BulletCollider;
    void Start()
    {
        Jimpu = this.gameObject;
        BulletCollider = Bullet.GetComponent<CapsuleCollider2D>();
        Bullet = Jimpu.transform.GetChild(0).gameObject;
        BulletAnimator = Bullet.GetComponent<Animator>();
        Target = playerControll.Player.transform;
        Shoting = false;
    }
    void Update()
    {
        if (IsPlayerInReach() && !Shoting)
        {
            StartCoroutine(ShotBullet());
        }
        if (playerControll.playerCollider.IsTouching(BulletCollider))
        {
            StartCoroutine(enemyScript.DamagePlayer());
        }

    }

    public bool IsPlayerInReach()
    {
        if (Target == null || Jimpu == null) return false;

        float distSq = (Target.position - Jimpu.transform.position).sqrMagnitude;
        float radiusSq = sichtRadius * sichtRadius;
        return distSq <= radiusSq;
    }
    public IEnumerator ShotBullet()
    {
        Shoting = true;
        Debug.Log("Executing ShotBullet");
        if (BulletAnimator != null)
        {
            BulletAnimator.SetBool("Attack", true);
        }

        if (Bullet == null || Target == null) yield break;

        Vector3 startPos = Bullet.transform.position;
        Vector3 TargetPosition = Target.position;
        float s = 0;
        StartCoroutine(DestroyTimer());
        while (s < travelDuration)
        {
            float t = s / travelDuration;
            Bullet.transform.position = Vector3.Lerp(startPos, TargetPosition, t); // moving the object
            s += Time.deltaTime;
            yield return null;
        }
        BulletAnimator.SetBool("Attack", false);
        Debug.Log("Bullet reached target position");
        Shoting = false;
    }
    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(travelDuration, travelDuration * 1.1f));
    }
}
        
