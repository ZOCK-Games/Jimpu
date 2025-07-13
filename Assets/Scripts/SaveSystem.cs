using UnityEngine;
using UnityEngine.UI;

public class SaveSystem1 : MonoBehaviour
{
    public PlayerStats playerStats; // Referenz zu deinem Spieler-Skript im Inspector setzen
    public Button SaveButton;
    public Button LoadButton;

    private void Start()
    {
        SaveButton.onClick.AddListener(SaveDataOnClick);
        LoadButton.onClick.AddListener(LoadDataOnClick);
    }

    public void SaveDataOnClick()
    {
        SaveData.SavePlayer(playerStats);
        Debug.Log("Spieler gespeichert.");
    }

    public void LoadDataOnClick()
    {
        PlayerData data = SaveData.LoadPlayer();

        if (data != null)
        {
            playerStats.PlayerHealt = data.health;
            playerStats.PlayerMoney = data.money;

            Vector2 position = new Vector2(data.position[0], data.position[1]);
            playerStats.PlayerPosition = position;
            playerStats.transform.position = position;

            Debug.Log("Spieler geladen. Position: " + position + ". health " + playerStats.PlayerHealt + ". Money" + playerStats.PlayerMoney);
        }
        else
        {
            Debug.LogWarning("Keine Spieldaten gefunden.");
        }
    }
}
