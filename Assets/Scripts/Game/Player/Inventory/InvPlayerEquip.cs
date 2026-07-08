using System.Threading.Tasks;
using UnityEngine;
public class InvPlayerEquip : MonoBehaviour
{
    public static InvPlayerEquip instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<GameObject> EquipItem(ItemData itemData)
    {
        if (itemData == null) return null;

        var Parent = playerControl.instance.playerBodyParts.playerBodyParts.Find(x => x.playerBodyPartTypes == itemData.playerBodyPartType);
        if (Parent == null) return null;

        var Object = await SpawningItem(itemData);
        Object.transform.SetParent(Parent.gameObject.transform, false);

        return Object;

    }

    async Task<GameObject> SpawningItem(ItemData itemData)
    {
        GameObject item = await itemData.ItemReference.InstantiateAsync().Task;

        item.AddComponent<ItemHolder>().ItemReference = itemData;

        return item;
    }
}

public class ItemHolder : MonoBehaviour
{
    public ItemData ItemReference;
    public bool IsActive;
}