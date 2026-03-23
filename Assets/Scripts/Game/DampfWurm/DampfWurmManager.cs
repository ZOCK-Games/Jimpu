using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;

public class DampfWurmManager : EntityManager
{
    [SerializeField] private List<GameObject> ChildObjects = new List<GameObject>();
    [SerializeField] private List<Rigidbody2D> rbs = new List<Rigidbody2D>();
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] public bool PlayerInRange;
    [SerializeField] private Animator animator;
    [SerializeField] public bool Attacking;
    protected override void Start()
    {
        Attacking = false;
        animator = this.gameObject.GetComponent<Animator>();
        foreach (Transform child in transform)
        {
            ChildObjects.Add(child.gameObject);
        }
        foreach (GameObject gameObject in ChildObjects)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.angularDamping = 1000f;
            rb.gravityScale = 0.1f;
            rbs.Add(rb);

        }
        circleCollider2D = new GameObject().AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = 2;
        circleCollider2D.gameObject.transform.SetParent(transform);
        SetHealth(Random.Range(5, 8));
        npcName = "DampfWurm";
        base.Start();
    }
    protected virtual async Task Attack()
    {
        animator?.SetBool("Attack", true);
        Attacking = true;
        await Task.Delay((int)(0.3f * 1000));
        animator?.SetBool("Attack", false);
        Attacking = false;
    }
    protected override void Die()
    {
        foreach (Rigidbody2D rigidbody in rbs)
        {
            rigidbody.gravityScale = 0.4f;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        Destroy(GetComponent<Animator>());
        DestroyTime = 8f;
        base.Die();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (Attacking)
        {
            EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
            entity?.TakeDamage(2);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (circleCollider2D != null && circleCollider2D.IsTouching(collision))
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerInRange = true;
            }
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (circleCollider2D != null && !circleCollider2D.IsTouching(collision))
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerInRange = false;
            }
        }
    }
}
