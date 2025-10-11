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

    private GameObject Bullet;
    private Animator BulletAnimator;
    private GameObject Jimpu;
    private Transform Target;
    private bool IsLoading = false;
    private bool CanFire = false;
    private bool Shoting;
    void Start()
    {
        Jimpu = this.gameObject;
        Bullet = Jimpu.transform.GetChild(0).gameObject;
        BulletAnimator = Bullet.GetComponent<Animator>();
        Target = playerControll.Player.transform;
        Shoting = false;
    }
    void Update()
    {
        if (IsPlayerInReach() && !IsLoading && !Shoting)
        {
            StartCoroutine(LoadAttack());
        }

    }

    public bool IsPlayerInReach()
    {
        if (Target == null || Jimpu == null) return false;

        float distSq = (Target.position - Jimpu.transform.position).sqrMagnitude;
        float radiusSq = sichtRadius * sichtRadius;
        return distSq <= radiusSq;
    }


    public IEnumerator LoadAttack()
    {
        Debug.Log("Starting LoadAttack...");
        IsLoading = true;
        CanFire = false;

        if (BulletAnimator != null)
        {
            BulletAnimator.SetBool("Load", true);
        }

        yield return new WaitForSeconds(loadTime);

        CanFire = true;
        Debug.Log($"Attack window OPEN for {decisionTime} seconds.");

        float timer = 0f;

        while (timer < decisionTime && CanFire)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!CanFire)
        {
            StartCoroutine(ShotBullet());
        }
        else
        {
            Debug.Log("Attack window CLOSED");
            if (BulletAnimator != null)
            {
                BulletAnimator.SetBool("Load", false);
                BulletAnimator.Play("Exit");
            }
        }
        // reset
        IsLoading = false;
        CanFire = false;
    }

    public void TriggerShot()
    {
        if (CanFire)
        {
            CanFire = false;
        }
    }

    public IEnumerator ShotBullet()
    {
        Shoting = true;
        Debug.Log("Executing ShotBullet");
        if (BulletAnimator != null)
        {
            BulletAnimator.SetBool("Load", false);
            BulletAnimator.Play("JimpuAttacking");
        }

        if (Bullet == null || Target == null) yield break;

        Vector3 startPos = Bullet.transform.position;
        float s = 0;

        while (s < travelDuration)
        {
            float t = s / travelDuration;
            Bullet.transform.position = Vector3.Lerp(startPos, Target.position, t); // moving the object
            s += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Bullet reached target position");
        Shoting = false;
    }
}
        
