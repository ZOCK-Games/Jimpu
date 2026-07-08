using System.Threading.Tasks;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    private ItemData LuckyBlockItem;
    private async Task Start()
    {
        ItemHolder holder;

        while ((holder = GetComponent<ItemHolder>()) == null || holder.ItemReference == null)
        {
            await Task.Yield();
        }

        LuckyBlockItem = holder.ItemReference;

        PlayerAttackManager.OnAttackTurn += CheckAttack;
    }

    public void SpawnChest()
    {
        Vector3 Position = new Vector3(playerControl.instance.Player.transform.position.x, playerControl.instance.Player.transform.position.y + 1.5f, 0);
        ChestManager chestManager = FindFirstObjectByType<ChestManager>();
        chestManager.AddChest(Position);
        gameObject.SetActive(false);
        Inventory.instance.RemoveItem(LuckyBlockItem, 1);
    }

    async void CheckAttack(ItemData itemData)
    {
        Debug.Log(itemData.ItemNameText);
        if (itemData == LuckyBlockItem)
        {
            SpawnChest();
        }
    }
}
