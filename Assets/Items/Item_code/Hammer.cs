using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hammer : MonoBehaviour
{
    public GameObject HamerObjekt;
    public GameObject EnemyContainer;
    public Animator ItemAnimator;
    public int Demage = 1;
    public float Range = 0.8f;
    public Tilemap ObjektTilemap;
    public Tilemap BlocksTilemap;
    public AnimatedTile ExplsionTile;
    private bool CanAttack;
    private int currentenemy;
    public Inventory inventory;
    public GameObject ExplosionPrefab;
    public TilemapCollider2D GroundCollider;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void Start()
    {
        CanAttack = true;
    }
    void Update()
    {
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {     // Plays the attack hit animation when enemy is in range and the player presses E
            if (HamerObjekt.GetComponent<BoxCollider2D>().IsTouching(EnemyContainer.transform.GetChild(i).GetComponent<CapsuleCollider2D>()) && CanAttack && inputActions.Player.Interact.WasCompletedThisFrame())
            {
                inventory.Clear();
                currentenemy = i;
                StartCoroutine(Inaktive());

            }

            else if (inputActions.Player.Interact.WasCompletedThisFrame()) // Plays the attack animation but dose not hit
            {
                ItemAnimator.SetTrigger("HammerUse");
            }
        }
        

    }
public IEnumerator Inaktive()
{
    float NumY = 0.9f;
    Transform enemy = EnemyContainer.transform.GetChild(currentenemy);
    float PosY = enemy.position.y;
    float PosX = enemy.position.x;

    bool tileFound = false;
    Vector3Int cellPos = Vector3Int.zero;

    for (int i = 0; i < 125; i++) // Max 20 Versuche
    {
        PosY = enemy.position.y;
        PosX = enemy.position.x;
        float TileY = Random.Range(PosY, PosY + Range);
        float TileX = Random.Range(PosX, PosX + Range);

        Vector3 TilePos = new Vector3(TileX, TileY - NumY, 0);
        cellPos = BlocksTilemap.WorldToCell(TilePos);

        if (!BlocksTilemap.HasTile(cellPos))
        {
            tileFound = true;
            break;
        }

        NumY += 0.3f;
    }

    if (tileFound)
    {
        Debug.Log("Found Tile: " + BlocksTilemap);
        ObjektTilemap.SetTile(cellPos, ExplsionTile);
        ItemAnimator.SetTrigger("HammerHit");
        CanAttack = false;

        yield return new WaitForSeconds(0.4f);
        ObjektTilemap.SetTile(cellPos, null);
        enemy.GetComponent<EnemyInfo>().EnemyHealt -= 1;
        currentenemy = -1;
        yield return new WaitForSeconds(0.8f);

        CanAttack = true;
        HamerObjekt.SetActive(false);
    }
    else
    {
        Debug.LogWarning("No valid tile found after multiple attempts.");
    }
}

}
