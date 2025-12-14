using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemInfoManager : MonoBehaviour //Can be used by the items for getting information's
{
    public PlayerControll playerControll;
    public Inventory inventory;
    public ChestManager chestManager;
    public EnemyScript enemyScript;
    public Tilemap DecoTilemap;
    public Tilemap BlockTilemapDestroyable;
    public TilemapCollider2D GroundCollider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
