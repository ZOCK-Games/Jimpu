using NavMeshPlus.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BombScribt : MonoBehaviour
{
    [SerializeField] private Tilemap BlockTilemap;
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private GameObject ExplosionBigPrefab;
    [SerializeField] private Button AddButton;
    [SerializeField] private Button SubtractButton;
    [SerializeField] private TextMeshProUGUI TextExplosionRadius;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public float ExplosionRadius;
    void Start()
    {
        TextExplosionRadius.text = ExplosionRadius.ToString();

        AddButton.onClick.AddListener(() =>
        {
            TextExplosionRadius.text = ExplosionRadius.ToString();
            ExplosionRadius += 0.5f;
        });
        SubtractButton.onClick.AddListener(() =>
        {
            TextExplosionRadius.text = ExplosionRadius.ToString();
            ExplosionRadius -= 0.5f;
        });
    }
    public void SetBombPosition()
    {
        GameObject BombPrefab = Instantiate(ExplosionBigPrefab);
        BombPrefab.transform.position = playerControll.Player.transform.position;
        UnityEngine.Vector3 Size = new UnityEngine.Vector3(ExplosionRadius, ExplosionRadius, 0);
        BombPrefab.transform.localScale = Size;
        Destroy(BombPrefab, 1.5f);
        for (int i = 0; i < 125 * ExplosionRadius; i++)
        {
            Vector2 Playerpos = playerControll.Player.transform.position;
            float TilePosY = Playerpos.y + Random.Range(ExplosionRadius, -ExplosionRadius);
            float TilePosX = Playerpos.x + Random.Range(ExplosionRadius, -ExplosionRadius);

            Vector3 TilePos3 = new Vector3(TilePosX, TilePosY, 0);
            Vector3Int cellPos = BlockTilemap.WorldToCell(TilePos3);
            BlockTilemap.SetTile(cellPos, null);
        }
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }
}
