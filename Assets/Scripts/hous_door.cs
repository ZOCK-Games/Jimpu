using System.Collections;
using TMPro;
using UnityEngine;

public class hous_door : MonoBehaviour
{
    public Collider2D Door;
    public Collider2D Player;
    public GameObject Speach_bubbel;
    public TextMeshPro text_speach_bubel;
    public SpriteRenderer image_press;
    public UnityEngine.UI.Image Darker;
    public float Speed = 1f;
    public Animator Dor_oppen;
    public Transform Position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Speach_bubbel.SetActive(false);
        Dor_oppen.StartPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsTouching(Door))
        {
            Speach_bubbel.SetActive(true);
            if (Player.IsTouching(Door) && Input.GetKey(KeyCode.E))
            {
                Color farbe = Darker.color; //KI gernerier
                farbe.a += Time.deltaTime * Speed;  //KI gernerier
                farbe.a = Mathf.Clamp01(farbe.a);  //KI gernerier
                Darker.color = farbe;   //KI gernerier
                Dor_oppen.StopPlayback();
                StartCoroutine(Door_wayt());  //KI gernerier
            }

            StartCoroutine(Text_wayt());
        }
        else
        {
            Speach_bubbel.SetActive(false);
        }



    }
    IEnumerator Text_wayt()
    {
        yield return new WaitForSeconds(1f);
        text_speach_bubel.text = "Press";
        yield return new WaitForSeconds(0.4f);
        image_press.enabled = true;
        yield return new WaitForSeconds(3f);

    }
    IEnumerator Door_wayt()
    {
        yield return new WaitForSeconds(0.307f);
        Dor_oppen.StartPlayback();
        Player.transform.Translate(+7f, 0.2f, 0);
        
       }

}
