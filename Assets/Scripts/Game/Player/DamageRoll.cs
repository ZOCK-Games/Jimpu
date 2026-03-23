using UnityEngine;

public class DamageRoll : MonoBehaviour
{
    public PlayerControll playerControll;
    public bool DamageRollActive => playerControll.CurrentAttack == "DodgeRoll";
    public float Damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DamageRollActive && collision.tag == "Attackable")
        {
            EntityManager entity = collision.gameObject.GetComponent<EntityManager>();
            if (entity == null)
            {
                entity = collision.gameObject.transform.GetComponentInParent<EntityManager>();
            }
            if (entity != null)
            {
                entity.TakeDamage(Damage);
                ParticelManager.instance.SpawnParticle(collision.transform.position, "Particle System Attack", 0.4f);
            }
            else
            {
                Debug.Log("There Has been a problem with finding the enmity Manager");
            }
        }
    }

}
