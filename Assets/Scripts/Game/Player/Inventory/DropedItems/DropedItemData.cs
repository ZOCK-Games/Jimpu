using UnityEngine;

public class DroppedItemData : MonoBehaviour
{
    public ItemData itemData;
    private GameObject info;

    void Start()
    {
        gameObject.tag = "Item";

        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.AddComponent<SpriteRenderer>().sprite = itemData.ItemImagePrev;
        gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        var hitBox = new GameObject("hitBox");
        hitBox.AddComponent<CircleCollider2D>().radius = 0.1f;
        hitBox.transform.SetParent(transform);
        info = Instantiate(Inventory.instance.PickUpItemInfo);
        info.transform.parent = transform;
        info.transform.position = new Vector3(0,3,0);
        info.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !info.activeSelf)
        {
            info.SetActive(true);
            Inventory.instance.AddItem(itemData, null, 1);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && info.activeSelf)
        {
            info.SetActive(false);
        }
    }
}
