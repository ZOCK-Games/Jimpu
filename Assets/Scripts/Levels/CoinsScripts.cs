using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinsScripts : MonoBehaviour
{
    public Tilemap CoinTilemap;
    public PolygonCollider2D PlayerPolygonCollider;
    public GameData gameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CoinTilemap.GetComponent<TilemapCollider2D>().IsTouching(PlayerPolygonCollider)) //check if the player is touching the coin tilemap
        {
            gameData.CoinValue += 1; // Increment the coin count in gameData
            CoinTilemap.enabled = false; // Disable the coin tilemap to prevent further collection
        }
        
    }
}
