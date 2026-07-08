using System.Threading.Tasks;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Animator ItemAnimator;
    [SerializeField] private bool CanAttack;
    public GameObject ExplosionPrefab;
    [SerializeField] private ItemData itemDataHammer;
    private async void Start()
    {
        ItemAnimator = GetComponentInParent<Animator>();

        ItemHolder holder;

        while ((holder = GetComponent<ItemHolder>()) == null || holder.ItemReference == null)
        {
            await Task.Yield();
        }

         itemDataHammer = holder.ItemReference;
         
        CanAttack = false;

        PlayerAttackManager.OnAttackTurn += CheckAttack;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanAttack) return;

        EntityManager entity = collision.gameObject.GetComponent<EntityManager>();

        if (entity == null) return;

        CanAttack = false;
        entity.TakeDamage(5);
        ItemAnimator.SetTrigger("HammerUse");
    }

    async void CheckAttack(ItemData itemData)
    {
        Debug.Log(itemData.ItemNameText);
        if (itemData == itemDataHammer)
        {
            CanAttack = true;
        }
    }
}
