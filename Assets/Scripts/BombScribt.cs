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
        float TilePosY = Playerpos.y + Random.Range(5.5f, -5.5f);
        float TilePosX = Playerpos.x + Random.Range(5.5f, -5.5f);

        Vector3 TilePos3 = new Vector3(TilePosX ,TilePosY , 0);
        Vector3Int cellPos = BlockTilemap.WorldToCell(TilePos3);
        BlockTilemap.SetTile(cellPos, null);
        }
    }
}
