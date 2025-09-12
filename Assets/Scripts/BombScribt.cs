using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombScribt : MonoBehaviour
{
    [SerializeField] private Tilemap BlockTilemap;
    [SerializeField] private PlayerControll playerControll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetBombPosition()
    {
        for (int i = 0; i < 25; i++)
{
        Vector2 Playerpos = playerControll.Player.transform.position;
        int TilePos2 = Random.Range(5, 0);
        Vector3 TilePos3 = new Vector3(Playerpos.x + TilePos2, Playerpos.y + TilePos2, 0);
        Vector3Int cellPos = BlockTilemap.WorldToCell(TilePos3);
        BlockTilemap.SetTile(cellPos, null);
        }
    }
}
