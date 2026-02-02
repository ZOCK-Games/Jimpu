using System;
using TMPro;
using UnityEngine;

public class Coin_scribt : MonoBehaviour
{
    public Collider2D Coin_Collider;
    public Collider2D PLayer_Collider;
    public TextMeshProUGUI PLayer_Coin_Counter;

    void Start()

    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PLayer_Collider.IsTouching(Coin_Collider))
        {
            Coin_Collider.gameObject.SetActive(false);
        }
    }
    private void GetComponent(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
