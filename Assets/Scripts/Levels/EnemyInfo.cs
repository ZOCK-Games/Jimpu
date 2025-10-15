
using System.Collections;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int EnemyHealt;
    public Vector3 EnemyPosition;
    public Transform Target;
    public GameObject JimpuObj;
    public float ViewField;
    public bool IsMoving;
    public bool IsCharging;
    public bool IsAttacking;
    public Animator JimpuAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool WantsToAttack;
    void Start()
    {
        JimpuObj = this.gameObject;
        JimpuAnimator = this.gameObject.GetComponent<Animator>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        WantsToAttack = false;
    }
    public bool IsPlayerInReach()
    {
        if (Target == null || JimpuObj == null) return false;

        float distSq = (Target.position - JimpuObj.transform.position).sqrMagnitude;
        float radiusSq = ViewField * ViewField;
        return distSq <= radiusSq;
    }
    void Update()
    {
        EnemyPosition = JimpuObj.transform.position;
        Vector3 PlayerPosBevore = this.gameObject.transform.position;
        if (this.gameObject.transform.position == PlayerPosBevore)
        {
            JimpuAnimator.SetBool("Walk", true);
        }
        JimpuAnimator.SetBool("Walk", false);

        if (rb.linearVelocity.x > 0)
        {
            sr.flipX = true;
        }
        else if (rb.linearVelocity.x < 0)
        {
            sr.flipX = false;
        }
    }
}

