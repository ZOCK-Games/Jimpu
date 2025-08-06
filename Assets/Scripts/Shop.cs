using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IDataPersitence
{
    [Header("The ItemPaper Buttons must be in the same order as the ItemData")]
    public List<Button> ItemPaperButtons; // Buttons for selecting items in the shop
    public List<ItemData> Itemdata; // Images of the items in the shop
    [Header("Shop Images and texts")]
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfos;
    public Image ItemImage;
    public List<Sprite> PaperImages;
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
        DataPersitenceManger.Instance.LoadGame();

        ShopMassage.text = "";
        ShopMassage.color = Color.white;

        ItemPaperButtons[0].onClick.AddListener(() => { CurentPaperButton = 0; SelectedItem(); });
        ItemPaperButtons[1].onClick.AddListener(() => { CurentPaperButton = 1; SelectedItem(); });
        ItemPaperButtons[2].onClick.AddListener(() => { CurentPaperButton = 2; SelectedItem(); });
        ItemPaperButtons[3].onClick.AddListener(() => { CurentPaperButton = 3; SelectedItem(); });

        CloseShop.onClick.AddListener(closeshop);
        BuyButton.onClick.AddListener(BuyButtonClick);
        for (int i = 0; i < ItemPaperButtons.Count; i++) //Setzt Die Images Der Paper Buttons
        {
            ItemPaperButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = Itemdata[i].ItemImagePrev1;

            ItemPaperButtons[i].image.sprite = PaperImages[i];        
            Debug.Log("Curent Image: " + Itemdata[i].ItemImagePrev1.name);
        }

    }

    // Update is called once per frame
    public void closeshop()
    {
        SceneManager.LoadScene(PrevScene);
    }
    public void SelectedItem()                  // This function is called when the Player selects an item in the shop
    {
        ShopKeeper.GetComponent<Animator>().SetTrigger("Bounce");
        ShopMassage.text = "You have selected an item!";
        ShopMassage.color = Color.white;
        StartCoroutine(TextFade());
        ItemImage.sprite = Itemdata[CurentPaperButton].ItemImagePrev1;
        ItemName.SetText(Itemdata[CurentPaperButton].ItemNameText);
        CurentItemPrice = Itemdata[CurentPaperButton].ItemPriceInt;
        ItemPrice.SetText(Itemdata[CurentPaperButton].Price + "$");
        ItemInfos.SetText(Itemdata[CurentPaperButton].ItemInfosText);
    }
    public void BuyButtonClick() // This function is called when the Player clicks the buy button in the shop
    {
        if (CoinValue >= CurentItemPrice)
        {
            CurentItem = Itemdata[CurentPaperButton].name;
            Debug.Log("Das Aktuelle Item Ist: " + CurentItem + "Preis: " + CurentItemPrice);

            ShopMassage.text = "You have sucesfully bought an item!";
            ShopMassage.color = Color.green;
            CoinValue -= CurentItemPrice;
            DataPersitenceManger.Instance.SaveGame();


        }
        else // If the Player does not have enough money to buy the item or an error occurs this message will be displayed
        {
            ShopMassage.text = "You don't have enough money! or an error occured!";
            ShopMassage.color = Color.red;

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