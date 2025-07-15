using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player_move : MonoBehaviour

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
    public PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        UnityEngine.Debug.Log("Game Startet");

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
            UnityEngine.Debug.Log("Berührt Boden!");
        }
        else
        {
            Walkking_animation.SetActive(false);
        }
        Player.transform.rotation = Quaternion.Euler(0f, 0f, PlayerRotation);

        SkinChange(); //Skin changet Permanent

        if (playerStats.SkinNumber >= SkinSprite.Count)
        {
            playerStats.SkinNumber = 0;  //Setzt SkinNumber zurück wen größer alls die verhandenen bilder
        }

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

    private void SkinChange()
    {
        PlayerSpriteRenderer.sprite = SkinSprite[playerStats.SkinNumber];
        UnityEngine.Debug.Log("Skin Number" + playerStats.SkinNumber);

    } 
}
