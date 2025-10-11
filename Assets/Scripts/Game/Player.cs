using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour, IDataPersitence

{
    public GameObject Player;
    public Animator PlayerAniamtor;
    public float PlayerRotation;
    public GameObject CollidersGameObjekt; // the TilemapContainer
    public Camera Camera1;
    public float move_speed_R = 3.5f;
    public float move_speed_L = 3.5f;
    public float Jump_speed = 350;
    public bool CanMove = true;
    public int run_speed = 5;
    public Rigidbody2D rb;
    public GameObject Walkking_animation;
    public List<Sprite> SkinSprite;
    private int SkinIndex;
    public GameObject BodyPartsContainer;

    public GarderobenScribt garderobenScribt;
    [Header("Can Player perform this inputs?")]
    public bool MovePlayerR;
    public bool MovePlayerL;
    public bool MovePlayerUP;
    public int PlayerHealth;
    public List<TilemapCollider2D> Grounds;
    [SerializeField] private List<Skins> PlayerSkins;
    public bool PlayerIsTouchingGround; // if the player is touching a ground tile collider
    [Header("Player Hold On to settings")]
    public GameObject RobeSagment;
    public float HoldOnRadius;
    private bool IsHoldingOn;
    private PolygonCollider2D playerCollider;
    private Rigidbody2D PlayerRb;
    private void Start()
    {
        playerCollider = Player.GetComponent<PolygonCollider2D>();
        PlayerRb = Player.GetComponent<Rigidbody2D>();

        PlayerIsTouchingGround = false;
        UnityEngine.Debug.Log("Tutorial Game Has Startet");
        rb = Player.GetComponent<Rigidbody2D>();
        PlayerAniamtor.SetBool("Walk", false);

        for (int i = 0; i < CollidersGameObjekt.transform.childCount - 1; i++)
        {
            if (CollidersGameObjekt.transform.GetChild(i).gameObject.CompareTag("Ground"))
            {
                Grounds.Add(CollidersGameObjekt.transform.GetChild(i).gameObject.GetComponent<TilemapCollider2D>());
                Debug.LogError("Found Ground: " + CollidersGameObjekt.transform.GetChild(i).gameObject.name);
            }
            else if (Grounds == null)
            {
                Debug.LogWarning("NoGroundFound");
            }
            else
            {
                Debug.Log("An unexpected error accrued");
            }
        }
        CheckSkin();
    }


    // Update is called once per frame
    void Update()
    {
        /*/if (PlayerHealth == 0)              // Aktiviert Den Dead Screen
        SceneManager.LoadScene("Death");*/

        PlayerIsTouchingGround = false; 

        foreach (var ground in Grounds)
        {
            if (ground != null && playerCollider.IsTouching(ground))
            {
                PlayerIsTouchingGround = true; 
                break; 
            }
        }

        Vector2 position = Player.transform.position;

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftControl) && CanMove || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftControl) && CanMove || MovePlayerR == true && CanMove)
        {
            PlayerAniamtor.SetBool("Walk", true);
            position.x += run_speed * Time.deltaTime;
            Player.transform.position = position;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && CanMove || Input.GetKey(KeyCode.D) && CanMove || MovePlayerR == true && CanMove)
        {
            PlayerAniamtor.SetBool("Walk", true);
            position.x += move_speed_R * Time.deltaTime;
            Player.transform.position = position;//position.x = x cordinaten Time.deltaTime weil sonst per frames abhängig 
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && CanMove || Input.GetKey(KeyCode.A) && CanMove || MovePlayerL == true && CanMove)
        {
            position.x -= move_speed_L * Time.deltaTime;
            Player.transform.position = position;
            PlayerAniamtor.SetBool("Walk", true);
        }
        else
        {
            PlayerAniamtor.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsHoldingOn)
        {
            IsHoldingOn = false;
            CanMove = true;
            PlayerRb.constraints = RigidbodyConstraints2D.None;
            Debug.Log("Player is no longer holding on");

        }
        if (Input.GetKeyDown(KeyCode.Space) && PlayerIsTouchingGround && CanMove || MovePlayerUP && CanMove)
        {
            rb.AddForce(new Vector2(0, Jump_speed));
            PlayerAniamtor.SetTrigger("Jump");
        }

        if (Input.GetKey(KeyCode.F))
        {

            // Checks if there is a tilemap near the player to hold on to
            if (PlayerIsTouchingGround && !IsHoldingOn)
            {
                float Posy = UnityEngine.Random.Range(Player.transform.position.y, Player.transform.position.y + HoldOnRadius);
                float Posx = UnityEngine.Random.Range(Player.transform.position.x, Player.transform.position.x + HoldOnRadius);

                Vector3 PosTile = new Vector3(Posx, Posy, 0);
                Vector3Int cellPos = CollidersGameObjekt.transform.GetChild(1).GetComponent<Tilemap>().WorldToCell(PosTile);
                Debug.Log("Player Can Hold On To Position: {cellPos}");
                PlayerRb.constraints = RigidbodyConstraints2D.FreezeAll;
                Player.transform.position = cellPos;
                CanMove = false;
                Debug.DrawRay(cellPos, cellPos * 2, Color.red);
                IsHoldingOn = true;
                GameObject previousSegment = null;
            }
        }


        Player.transform.rotation = Quaternion.Euler(0f, 0f, PlayerRotation);
    }
    public void CheckSkin()
    {
        BodyPartsContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Head;
        BodyPartsContainer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Body;
        BodyPartsContainer.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftArm;
        BodyPartsContainer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightArm;
        BodyPartsContainer.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftLeg;
        BodyPartsContainer.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightLeg;
    }


    public void LoadGame(GameData data)
    {
        Player.transform.localPosition = new Vector3(data.PlayerPositionX, data.PlayerPositionY, 0);
        SkinIndex = data.SkinIndex;
        CheckSkin();
        //Player.GetComponent<SpriteRenderer>().sprite = SkinSprite[SkinIndex];
        if (UnityEngine.ColorUtility.TryParseHtmlString("#" + data.colorhex, out Color colorHex))
            for (int i = 0; i < BodyPartsContainer.transform.childCount; i++)
            {
                BodyPartsContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorHex;
            }
        PlayerHealth = data.Health;

    }
    public void SaveGame(ref GameData data) // Save the current Data to GameData
    {
        data.PlayerPositionX = this.Player.transform.localPosition.x;
        data.PlayerPositionY = this.Player.transform.localPosition.y;
        data.Health = this.PlayerHealth;

        data.SkinIndex = SkinIndex;
    }
}
