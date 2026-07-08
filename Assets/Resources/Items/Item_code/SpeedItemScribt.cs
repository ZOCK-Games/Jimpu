using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SpeedItemScribt : MonoBehaviour
{
    [SerializeField] private float Adding = 2;
    public Inventory inventory;
    private bool PowerAktive;
    private float MoveBevore;

    private ItemData SpeedItem;

    async Task Start()
    {
        ItemHolder holder;

        while ((holder = GetComponent<ItemHolder>()) == null || holder.ItemReference == null)
        {
            await Task.Yield();
        }

        SpeedItem = holder.ItemReference;

        PlayerAttackManager.OnAttackTurn += CheckAttack;

        Adding = 2;
        PowerAktive = false;
    }
    void CheckInput()
    {
        if (gameObject.activeSelf && !PowerAktive)
        {
            PowerAktive = true;
            MoveBevore = playerControl.instance.PlayerMovement.MoveSpeed;

            playerControl.instance.PlayerMovement.MoveSpeed += Adding;
            Debug.Log("Power Used Power is aktive");
            StartCoroutine(Waiting());
        }


    }
    public void ResetStats()
    {
        playerControl.instance.PlayerMovement.MoveSpeed = MoveBevore;
        PowerAktive = false;
        inventory.RemoveItem(inventory.HandSlot.ItemStored, -10, null);
        Debug.Log("Power Used Power is Disabled & reset");
        playerControl.instance.PlayerMovement.JumpForce = 350; // to prevent it from making it to low soe how..
        gameObject.SetActive(false);
    }



    public IEnumerator Waiting()
    {
        Debug.Log("Wayt Stardet 5 sec to reset");
        yield return new WaitForSeconds(5);
        Debug.Log("Wayt_Aktive end");
        ResetStats();
    }

    async void CheckAttack(ItemData itemData)
    {
        Debug.Log(itemData.ItemNameText);
        if (itemData == SpeedItem)
        {
            CheckInput();
        }
    }

}
