using System;
using TMPro;
using UnityEngine;

public class Coin_scribt : MonoBehaviour
{
    public Collider2D Coin_Collider;
    public Collider2D PLayer_Collider;
    public TextMeshProUGUI PLayer_Coin_Counter;
    public GameData gameData;

    void Start()

    {
        PLayer_Coin_Counter.text = "Coins:" + gameData.CoinValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (PLayer_Collider.IsTouching(Coin_Collider))
        {
            Coin_Collider.gameObject.SetActive(false);
            gameData.CoinValue += 1;
            PLayer_Coin_Counter.text = $"Coins:{gameData.CoinValue}";
        }
    }
    private void GetComponent(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
