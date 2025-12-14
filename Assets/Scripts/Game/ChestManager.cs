using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestManager : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    public Tilemap ChestTilemap;
    public AnimatedTile ChestAnimationTile;
    public Tile ChestNormalTile;
    public GameObject DisplayItem;
    public Inventory inventory;
    public void AddChest(Vector3 Position)
    {
        Vector3Int TilePos = new Vector3Int((int)Position.x, (int)Position.y, (int)Position.z);
        Vector3Int cellPosition = ChestTilemap.WorldToCell(TilePos);
        ChestTilemap.SetTile(cellPosition, ChestNormalTile);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            contactPoint.y -= 0.001f;
            Vector3Int cellPosition = ChestTilemap.WorldToCell(contactPoint);
            Debug.Log("interacting with chest!");
            if (ChestTilemap.GetTile(cellPosition) == ChestNormalTile)
            {
                ChestTilemap.SetTile(cellPosition, ChestAnimationTile);
                StartCoroutine(DeleteTile(cellPosition));
            }
            else
            {
                Debug.Log("The Current Tile Is not a Chest");
            }
        }
    }
    IEnumerator DeleteTile(Vector3Int Pos)
    {
        yield return new WaitForSeconds(1f);
        float ElapsedTime = 0;
        GameObject Display = Instantiate(DisplayItem);
        Display.transform.position = new Vector3(Pos.x, Pos.y + 1.5f, 0);
        int ItemNumber = 0;
        while (ElapsedTime < 1f)
        {
            ItemNumber = Random.Range(0, inventory.itemData.Count);
            Display.GetComponent<SpriteRenderer>().sprite = inventory.itemData[ItemNumber].ItemImagePrev;
            yield return new WaitForSeconds(0.12f);
            ElapsedTime += Time.deltaTime;
            ChestTilemap.SetTile(Pos, null);
        }
        Destroy(Display, 0.6f);
        inventory.AddItem(inventory.itemData[ItemNumber]);
    }
}
