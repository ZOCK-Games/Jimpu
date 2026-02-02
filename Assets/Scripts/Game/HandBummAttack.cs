using UnityEngine;

public class HandBummAttack : MonoBehaviour
{
    public PlayerControll playerControll;
    public GameObject ParticleContainer;
    public float Damage;
    public GameObject HitParticle;
    private bool HandBummActive => playerControll.CurrentAttack == "HandBumm";
    private bool CanDealDamage; 
    void OnTriggerStay2D(Collider2D collision)
    {
        if (HandBummActive && collision.tag == "Attackable" && CanDealDamage)
        {
            collision.gameObject.GetComponent<UniversalHealthInfo>().Health -= Damage;
            GameObject Particle = Instantiate(HitParticle);
            Particle.transform.position = collision.transform.position;
            Destroy(Particle, 0.7f);
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
