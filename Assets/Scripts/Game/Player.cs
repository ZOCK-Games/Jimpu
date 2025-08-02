using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour, IDataPersitence

{
    public GameObject Player;
    public SpriteRenderer PlayerSpriteRenderer;
    public float PlayerRotation;
    public Collider2D Player_collider;
    public TilemapCollider2D Ground_Collider;
    public Button Player_pos_reset;
    public Camera Camera1;
    public float move_speed_R = 3.5f;
    public float move_speed_L = 3.5f;
    public float Jump_speed = 20f;
    public int run_speed = 5;
    public Rigidbody2D rb;
    public GameObject Walkking_animation;
    public List<Sprite> SkinSprite;
    private int SkinIndex;
    private void Start()
    {
        UnityEngine.Debug.Log("Tutorial Game Has Startet");
        Player_pos_reset.onClick.AddListener(ResetButtonClick);
        rb = Player.GetComponent<Rigidbody2D>();
        Walkking_animation.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        Vector2 position = Player.transform.position;

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            position.x += run_speed * Time.deltaTime;
            Player.transform.position = position;
            Walkking_animation.SetActive(true);
            PlayerSpriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            position.x += move_speed_R * Time.deltaTime;
            Player.transform.position = position;//position.x = x cordinaten Time.deltaTime weil sonst per frames abhängig 
            Walkking_animation.SetActive(true);
            PlayerSpriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            position.x -= move_speed_L * Time.deltaTime;
            Player.transform.position = position;
            Walkking_animation.SetActive(true);
            PlayerSpriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.Space) && Player_collider.IsTouching(Ground_Collider))
        {
            rb.AddForce(new Vector2(0, Jump_speed));
        }
        else
        {
            Walkking_animation.SetActive(false);
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
        Player.GetComponent<SpriteRenderer>().color = colorHex;
        
    }
    public void SaveGame(ref GameData data) // Save the current Data to GameData
    {
        data.PlayerPositionX = this.Player.transform.localPosition.x;
        data.PlayerPositionY = this.Player.transform.localPosition.y;

        data.SkinIndex = SkinIndex;
    }
}
