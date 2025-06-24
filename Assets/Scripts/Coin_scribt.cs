using System;
using TMPro;
using UnityEngine;

public class Coin_scribt : MonoBehaviour
{
    public Collider2D Coin_Collider;
    public Collider2D PLayer_Collider;
    public TextMeshProUGUI PLayer_Coin_Counter;
    public int Coin_Count;

    void Start()

    {
        Coin_Count = 0;
 
    }

    // Update is called once per frame
    void Update()
    {
        if (PLayer_Collider.IsTouching(Coin_Collider))
        {
            Coin_Collider.gameObject.SetActive(false);
            Coin_Count += 1;
            PLayer_Coin_Counter.text = "Coins:" + Coin_Count;
        }
        
        
    }

    private void GetComponent(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
