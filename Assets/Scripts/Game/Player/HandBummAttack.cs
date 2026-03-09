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
            collision.gameObject.GetComponent<NPCManager>().TakeDamage(Damage);
            ParticelManager.instance.SpawnParticle(collision.transform.position, "Particle System Attack", 0.4f);
            CanDealDamage = false;
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
