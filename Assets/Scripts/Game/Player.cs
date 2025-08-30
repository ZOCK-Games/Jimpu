using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour, IDataPersitence

{
    public GameObject Player;
    public Animator PlayerAniamtor;
    public float PlayerRotation;
    public TilemapCollider2D Ground_Collider;
    public Tilemap GroudTilemap;
    public Button Player_pos_reset;
    public Camera Camera1;
    public float move_speed_R = 3.5f;
    public float move_speed_L = 3.5f;
    public float Jump_speed = 20f;
    public bool CanMove = true;
    public int run_speed = 5;
    public Rigidbody2D rb;
    public GameObject Walkking_animation;
    public List<Sprite> SkinSprite;
    private int SkinIndex;
    public GameObject BodyPartsContainer;
    [Header("Can Player perform this inputs?")]
    public bool MovePlayerR;
    public bool MovePlayerL;
    public bool MovePlayerUP;
    public int PlayerHealth;
    [Header("Player Hold On to settings")]
    public GameObject RobeSagment;
    public float HoldOnRadius;
    private bool IsHoldingOn;
    private void Start()
    {
        UnityEngine.Debug.Log("Tutorial Game Has Startet");
        Player_pos_reset.onClick.AddListener(ResetButtonClick);
        rb = Player.GetComponent<Rigidbody2D>();
        PlayerAniamtor.SetBool("Walk", false);  
    }


    // Update is called once per frame
    void Update()
    {
        /*/if (PlayerHealth == 0)              // Aktiviert Den Dead Screen
        SceneManager.LoadScene("Death");*/

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
            Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
            Debug.Log("Player is no longer holding on");

        }
        if (Input.GetKeyDown(KeyCode.Space) && Player.GetComponent<Collider2D>().IsTouching(Ground_Collider) && CanMove || MovePlayerUP && CanMove)
        {
            rb.AddForce(new Vector2(0, Jump_speed));
            PlayerAniamtor.SetTrigger("Jump");
        }

        if (Input.GetKey(KeyCode.F))
        {

            // Checks if there is a tilemap near the player to hold on to
            if (Player.GetComponent<BoxCollider2D>().IsTouching(Ground_Collider) && !IsHoldingOn)
            {
                float Posy = Random.Range(Player.transform.position.y, Player.transform.position.y + HoldOnRadius);
                float Posx = Random.Range(Player.transform.position.x, Player.transform.position.x + HoldOnRadius);

                Vector3 PosTile = new Vector3(Posx, Posy, 0);
                Vector3Int cellPos = GroudTilemap.WorldToCell(PosTile);
                Debug.Log("Player Can Hold On To Position : " + cellPos);
                Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Player.transform.position = cellPos;
                CanMove = false;
                Debug.DrawRay(cellPos, cellPos * 2, Color.red);
                IsHoldingOn = true;
                GameObject previousSegment = null;
            }
        }


        Player.transform.rotation = Quaternion.Euler(0f, 0f, PlayerRotation);
    }
    private IEnumerator Reset()
    {
        Player.transform.Translate(0, 2, 0);
        Player.transform.eulerAngles = new Vector3(0, 0, 0);
        Player_pos_reset.interactable = false;
        yield return new WaitForSeconds(30f);
        Player_pos_reset.interactable = true;

    }

     private void ResetButtonClick()
    {
        StartCoroutine(Reset());
    }

    public void LoadGame(GameData data)
    {
        Player.transform.localPosition = new Vector3(data.PlayerPositionX, data.PlayerPositionY, 0);
        SkinIndex = data.SkinIndex;
        Player.GetComponent<SpriteRenderer>().sprite = SkinSprite[SkinIndex];
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
