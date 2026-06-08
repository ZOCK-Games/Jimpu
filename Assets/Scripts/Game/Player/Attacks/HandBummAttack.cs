using UnityEngine;

public class HandBummAttack : MonoBehaviour
{
    private float damage;
    private bool HandBummActive => playerControl.instance.PlayerState.CurrentAttack == "HandBumm";
    private bool CanDealDamage;
    void Start()
    {
        damage = playerControl.instance.PlayerMovement.HandBummAttackDamage;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!HandBummActive || collision.tag != "Attackable" || !CanDealDamage)
            return;

        EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
        if (entity == null)
        {
            entity = collision.transform.GetComponentInParent<EntityManager>();
        }
        if (entity != null)
        {
            entity.TakeDamage(damage);
            ParticelManager.instance.SpawnParticle(collision.transform.position, "Particle System Attack", 0.4f);
            CanDealDamage = false;
        }
        else
        {
            Debug.Log("There Has been a problem with finding the enmity Manager");
        }
    }
    void Update()
    {
        if (!CanDealDamage && !HandBummActive)
        {
            CanDealDamage = true;
        }
    }
}
