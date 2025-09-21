using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IDataPersitence
{
    [Header("The ItemPaper Buttons must be in the same order as the ItemData")]
    public List<GameObject> ItemPaperButtons; // Buttons for selecting items in the shop
    public List<ItemData> Itemdata; // Images of the items in the shop
    [Header("Shop Images and texts")]
    public TextMeshProUGUI ItemName; // the name of the item that is currently selected in the shop
    public Image ItemImage; // Image of the item that is currently selected in the shop
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfos;
    public TextMeshProUGUI ShopMassage;
    public Button BuyButton;
    public Button CloseShop;

    public string CurentItem;
    public int CurentItemPrice;
    public GameObject ShopGameObjekt;
    public int CurentPaperButton;
    public GameObject ShopKeeper;
    public string PrevScene;
    public int CoinValue; //The money the player has 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    DataPersitenceManger.Instance.SaveGame();
    DataPersitenceManger.Instance.LoadGame();

        SelectedItem(); // Initialize the first item as selected

     Debug.LogError("DataPersitenceManger Instance is null! Make sure it is in the scene!");

        for (int i = 0; i < ItemPaperButtons.Count; i++)
        {
            ItemPaperButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = Itemdata[i].ItemImagePrev1;
        }

              
        

        ShopMassage.text = "";
        ShopMassage.color = Color.white;
        for (int i = 0; i < ItemPaperButtons.Count; i++)
        {
            int index = i;  // Lokale Kopie
            ItemPaperButtons[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                CurentPaperButton = index;
                SelectedItem();
            });

        }
        CloseShop.onClick.AddListener(closeshop);
        BuyButton.onClick.AddListener(BuyButtonClick);
    }

    // Update is called once per frame
    public void closeshop()
    {
        SceneManager.LoadScene(PrevScene);
    }
    public void SelectedItem()                  // This function is called when the Player selects an item in the shop
    {
        ShopKeeper.GetComponent<Animator>().SetTrigger("Bounce");
        ShopMassage.color = Color.white;
        StartCoroutine(TextFade());
        ItemImage.sprite = Itemdata[CurentPaperButton].ItemImagePrev1;
        ItemName.SetText(Itemdata[CurentPaperButton].ItemNameText);
        CurentItemPrice = Itemdata[CurentPaperButton].ItemPriceInt;
        ItemPrice.SetText(Itemdata[CurentPaperButton].Price + "$");
        ItemInfos.SetText(Itemdata[CurentPaperButton].ItemInfosText);

        if (CurentItem == "")
        {
            ShopMassage.color = Color.yellow;
            ShopMassage.text = "You already have an item!";
            Debug.Log("You already have an item!");
            ShopKeeper.GetComponent<Animator>().SetTrigger("AlreadyHasItem");
        }
    }
    public void BuyButtonClick() // This function is called when the Player clicks the buy button in the shop
    {
        if (CoinValue >= CurentItemPrice && CurentItem == "")
        {
            CoinValue -= CurentItemPrice;

            CurentItem = Itemdata[CurentPaperButton].name;
            Debug.Log("Das Aktuelle Item Ist: " + CurentItem + "Preis: " + CurentItemPrice);

            ShopMassage.color = Color.green;
            ShopMassage.text = "You have sucesfully bought an item!";
            DataPersitenceManger.Instance.SaveGame();


        }
        else // If the Player does not have enough money to buy the item or an error occurs this message will be displayed
        {
            ShopMassage.text = "You don't have enough money! or an error occured!";
            ShopMassage.color = Color.red;
            StartCoroutine(TextFade());
            Debug.LogError("You don't have enough money! or an error occured!");
            ShopKeeper.GetComponent<Animator>().SetTrigger("Shake");

        }

    }
    private IEnumerator TextFade()
    {
        Debug.Log("Fade Text Stardet!");
        yield return new WaitForSeconds(3);
        ShopMassage.GetComponent<Animation>().Play("Fade Away Animation");
        yield return new WaitForSeconds(2);
        ShopMassage.text = "";
        ShopMassage.color = Color.white;


    }

    public void LoadGame(GameData data)
    {
        this.PrevScene = data.CurentScene;
        this.CoinValue = data.CoinValue;
        this.CurentItem = data.CurentItem;
    }

    public void SaveGame(ref GameData data)
    {
        data.CoinValue = this.CoinValue;
        data.CurentItem = this.CurentItem;
    }
}