using System.Collections;
using UnityEngine;

public class Gravity_Changer : MonoBehaviour
{
    public PlayerControll PlayerScribt;
    public GameObject GravityChangingObjekt;
    public Rigidbody2D PlayerRigidbody2D;
    public GameObject PlayerGameObjekt;
    public GameObject PowerAnimationObjekt;
    public bool IsAktive1;
    public bool reset;
    public bool BlockUse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsAktive1 = false;
        reset = false;
        PowerAnimationObjekt.SetActive(false);
        BlockUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GravityChangingObjekt.activeSelf && IsAktive1 == false && Input.GetKey(KeyCode.E) && BlockUse == false)
        {
            PlayerRigidbody2D.gravityScale = -0.25f;
            PlayerScribt.PlayerRotation = 180;
            PlayerScribt.Jump_speed = -55;
            PowerAnimationObjekt.SetActive(true);
            StartCoroutine(IsAktive());

        }
        if (reset == true && Input.GetKey(KeyCode.E))
        {
            PlayerScribt.PlayerRotation = 0f;
            PlayerRigidbody2D.gravityScale = 1f;
            PlayerScribt.Jump_speed = 55;
            PowerAnimationObjekt.SetActive(false);
            StartCoroutine(GoDown());
            Debug.Log("Reset");
        }
    }
    public IEnumerator IsAktive()
    {
        yield return new WaitForSeconds(0.3f);
        reset = true;
        Debug.Log("IsAktive = true");
        IsAktive1 = true;
    }
    public IEnumerator GoDown()
    {
        yield return new WaitForSeconds(0.1f);
        IsAktive1 = false;
        reset = false;
        Debug.Log("IsAktive = false");
        StartCoroutine(Wayt());

    }
    public IEnumerator Wayt()
    {
        BlockUse = true;
        Debug.Log("Wayt start");
        yield return new WaitForSeconds(2);
        BlockUse = false;
        GravityChangingObjekt.SetActive(false);
    }
}
