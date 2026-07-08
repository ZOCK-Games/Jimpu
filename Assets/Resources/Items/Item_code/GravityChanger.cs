using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private bool reset;
    private bool IsActive;
    private bool BlockUse;
    private Inventory inventory;
    private ItemData GravityItem;

    async Task Start()
    {
        reset = false;
        BlockUse = false;

        ItemHolder holder;

        while ((holder = GetComponent<ItemHolder>()) == null || holder.ItemReference == null)
        {
            await Task.Yield();
        }

        GravityItem = holder.ItemReference;

        PlayerAttackManager.OnAttackTurn += CheckAttack;
    }

    void ToggleState()
    {
        if(BlockUse) return;

        float DefaultJump = playerControl.instance.PlayerMovement.JumpForce;
        if (gameObject.activeSelf && !IsActive)
        {
            playerControl.instance.rb.gravityScale = -0.25f;
            playerControl.instance.transform.rotation = new Quaternion(180, 0, 0, 0);
            playerControl.instance.PlayerMovement.JumpForce = -DefaultJump / 3;
            StartCoroutine(IsAktive());
        }
        else if (gameObject.activeSelf && IsActive)
        {
            playerControl.instance.transform.rotation = new Quaternion(0, 0, 0, 0);
            playerControl.instance.rb.gravityScale = 1f;
            playerControl.instance.PlayerMovement.JumpForce = 350;
            StartCoroutine(GoDown());
            Debug.Log("Reset");
        }
    }
    public IEnumerator IsAktive()
    {
        IsActive = true;
        yield return new WaitForSeconds(0.3f);
        reset = true;
        Debug.Log("IsAktive = true");
    }
    public IEnumerator GoDown()
    {
        yield return new WaitForSeconds(0.1f);
        IsActive = false;
        reset = false;
        Debug.Log("IsAktive = false");
        StartCoroutine(Wayt());

    }
    public IEnumerator Wayt()
    {
        BlockUse = true;
        Debug.Log("Wayt start");
        yield return new WaitForSeconds(1.3f);
        BlockUse = false;
        Inventory.instance.RemoveItem(GravityItem, 1);
        gameObject.SetActive(false);
    }

    async void CheckAttack(ItemData itemData)
    {
        Debug.Log(itemData.ItemNameText);
        if (itemData == GravityItem)
        {
            ToggleState();
        }
    }
}
