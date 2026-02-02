using UnityEngine;

public class DamageRoll : MonoBehaviour
{
    public PlayerControll playerControll;
    public bool DamageRollActive => playerControll.CurrentAttack == "DodgeRoll";
    public float Damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DamageRollActive && collision.gameObject.tag == "Attackable")
        {
            collision.gameObject.GetComponent<UniversalHealthInfo>().Health -= Damage;
        }
    }

}
