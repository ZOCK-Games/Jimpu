using System.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DanmpfWurmAggressive : DampfWurmManager
{
    private CircleCollider2D circleCollider2D;
    private bool IsDamage;
    protected override void Start()
    {
        base.Start();
        IsDamage = false;
        circleCollider2D = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = 10;
        SetHealth(3);
        canTakeDamage = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
        if (entity == null)
        {
            entity = collision.gameObject.GetComponentInParent<EntityManager>();
        }

        if (entity != null && !IsDamage)
        {
            {
                StartCoroutine(Damage(entity));
            }
        }
    }
    IEnumerator Damage(EntityManager entity)
    {
        IsDamage = true;
        yield return new WaitForSeconds(1.3f);
        entity.TakeDamage(2);
        IsDamage = false;
    }

    void Update()
    {
        if (PlayerInRange && !Attacking)
        {
            Attack();
        }
    }
}
