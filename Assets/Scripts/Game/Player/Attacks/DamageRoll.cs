using System.Collections;
using UnityEngine;

public class DamageRoll : MonoBehaviour
{
    private bool DamageRollActive => playerControl.instance.PlayerState.CurrentAttack == "DodgeRoll";
    private float damage;
    private bool canDamageTimeLimiter;
    void Start()
    {
        damage = playerControl.instance.PlayerMovement.AttackRollDamage;
        canDamageTimeLimiter = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Attackable") || !DamageRollActive || !canDamageTimeLimiter)
        {
            return;
        }
        EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
        if (entity == null)
        {
            entity = collision.gameObject.transform.GetComponentInParent<EntityManager>();
        }
        if (entity != null)
        {
            entity.TakeDamage(damage);
            ParticelManager.instance.SpawnParticle(collision.transform.position, "Particle System Attack", 0.4f);
            StartCoroutine(damageLimiter());
        }
        else
        {
            Debug.Log("There Has been a problem with finding the entity Manager");
        }
    }
    IEnumerator damageLimiter()
    {
        canDamageTimeLimiter = false;
        yield return new WaitForSeconds(0.2f);
        canDamageTimeLimiter = true;
    }

}
