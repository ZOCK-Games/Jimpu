using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class linie : MonoBehaviour

{
    public GameObject Player;
    public Collider2D Player_collider;
    public Collider2D Ground_Collider;
    public Button Player_pos_reset;
    public Camera Camera1;
    public float move_speed_R = 3.5f;
    public float move_speed_L = 3.5f;
    public float Jump_speed = 4f;
    public int run_speed = 10;
    public Rigidbody2D rb;
    public GameObject Walkking_animation;
    public Tilemap PLayer_1_head;

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
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            position.x += move_speed_R * Time.deltaTime;
            Player.transform.position = position;//position.x = x cordinaten Time.deltaTime weil sonst per frames abhängig 
            Walkking_animation.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            position.x -= move_speed_L * Time.deltaTime;
            Player.transform.position = position;
            Walkking_animation.SetActive(true);
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
    


    
}
