using System;
using TMPro;
using UnityEngine;

public class Coin_scribt : MonoBehaviour
{
    public Collider2D Coin_Collider;
    public Collider2D PLayer_Collider;
    public TextMeshProUGUI PLayer_Coin_Counter;
    public PlayerStats PlayerStats;

    void Start()

    {
        PLayer_Coin_Counter.text = "Coins:" + PlayerStats.PlayerMoney;
    }

    // Update is called once per frame
    void Update()
    {
        if (PLayer_Collider.IsTouching(Coin_Collider))
        {
            Coin_Collider.gameObject.SetActive(false);
            PlayerStats.PlayerMoney += 1;
            PLayer_Coin_Counter.text = "Coins:" + PlayerStats.PlayerMoney;
        }
        
        
    }

    private void GetComponent(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
