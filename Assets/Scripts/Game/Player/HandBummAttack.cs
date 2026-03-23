using UnityEngine;

public class HandBummAttack : MonoBehaviour
{
    public PlayerControll playerControll;
    public GameObject ParticleContainer;
    public float Damage;
    private bool HandBummActive => playerControll.CurrentAttack == "HandBumm";
    private bool CanDealDamage;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (HandBummActive && collision.tag == "Attackable" && CanDealDamage)
        {
            EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
            if (entity == null)
            {
                entity = collision.transform.GetComponentInParent<EntityManager>();
            }
            if (entity != null)
            {
                entity.TakeDamage(Damage);
                ParticelManager.instance.SpawnParticle(collision.transform.position, "Particle System Attack", 0.4f);
                CanDealDamage = false;
            }
            else
            {
                Debug.Log("There Has been a problem with finding the enmity Manager");
            }
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
