using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> Item;
    public List<GameObject> Item_hand;
    public List<int> Item_Counter;
    public List<Button> Item_Buttons;
    public Button EquipButton;
    public Button UnequipButton;

    public List<bool> Button_Select_Item;
    public bool EquipButton_Bool;



    public bool Equip_Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Item_Counter[0] = 0;
        Item_Counter[1] = 0;
        Item_Counter[2] = 0;
        Item_Counter[3] = 0;
        Item_Counter[4] = 0;


        Button_Select_Item[0] = false;
        Button_Select_Item[1] = false;
        Button_Select_Item[2] = false;
        Button_Select_Item[3] = false;
        Button_Select_Item[4] = false;

        Item_Buttons[0].onClick.AddListener(Button_click_0);
        Item_Buttons[1].onClick.AddListener(Button_click_1);
        Item_Buttons[2].onClick.AddListener(Button_click_2);
        Item_Buttons[3].onClick.AddListener(Button_click_3);
        Item_Buttons[4].onClick.AddListener(Button_click_4);
    }

    // Update is called once per frame
    void Update()


    {
        if (Item_Counter[0] >= 1)
            Item[0].SetActive(true);
        if (Item_Counter[1] >= 1)
            Item[1].SetActive(true);
        if (Item_Counter[2] >= 1)
            Item[2].SetActive(true);
        if (Item_Counter[3] >= 1)
            Item[3].SetActive(true);
        if (Item_Counter[4] >= 1)
            Item[4].SetActive(true);


        if (Item_Counter[0] < 1)
            Item[0].SetActive(false);
        if (Item_Counter[1] < 1)
            Item[1].SetActive(false);
        if (Item_Counter[2] < 1)
            Item[2].SetActive(false);
        if (Item_Counter[3] < 1)
            Item[3].SetActive(false);
        if (Item_Counter[4] < 1)
            Item[4].SetActive(false);

        if (EquipButton.interactable == false)
            Debug.Log("Disabeld");


        if (EquipButton.interactable = false && Button_Select_Item[0] == true)
            {
                Item_hand[0].SetActive(true); //Setzt Das 0 Item Des Spilers Aktiv
                Debug.Log("Item_Hand [0] set aktive");
            }
        if (EquipButton == enabled && Button_Select_Item[1] == true)
        {
            Item_hand[1].SetActive(true); //Setzt Das 1 Item Des Spilers Aktiv
            Debug.Log("Item_Hand [1] set aktive");
        }
        if (EquipButton == enabled && Button_Select_Item[2] == true)
        {
            Item_hand[2].SetActive(true); //Setzt Das 2 Item Des Spilers Aktiv
            Debug.Log("Item_Hand [2] set aktive");
        }
        if (EquipButton == enabled && Button_Select_Item[3] == true)
        {
            Item_hand[3].SetActive(true); //Setzt Das 3 Item Des Spilers Aktiv
            Debug.Log("Item_Hand [3] set aktive");
        }
        if (EquipButton == enabled && Button_Select_Item[4] == true)
        {
            Item_hand[4].SetActive(true); //Setzt Das 4 Item Des Spilers Aktiv
            Debug.Log("Item_Hand [4] set aktive");
        }

    }
    public void Button_click_0()
    {
        Button_Select_Item[0] = true;
        Button_Select_Item[1] = false;
        Button_Select_Item[2] = false;
        Button_Select_Item[3] = false;
        Button_Select_Item[4] = false;
    }
    public void Button_click_1()
    {
        Button_Select_Item[0] = false;
        Button_Select_Item[1] = true;
        Button_Select_Item[2] = false;
        Button_Select_Item[3] = false;
        Button_Select_Item[4] = false;
    }
    public void Button_click_2()
    {
        Button_Select_Item[0] = false;
        Button_Select_Item[1] = false;
        Button_Select_Item[2] = true;
        Button_Select_Item[3] = false;
        Button_Select_Item[4] = false;
    }
    public void Button_click_3()
    {
        Button_Select_Item[0] = false;
        Button_Select_Item[1] = false;
        Button_Select_Item[2] = false;
        Button_Select_Item[3] = true;
        Button_Select_Item[4] = false;
    }
    public void Button_click_4()
    {
        Button_Select_Item[0] = false;
        Button_Select_Item[1] = false;
        Button_Select_Item[2] = false;
        Button_Select_Item[3] = false;
        Button_Select_Item[4] = true;
    }
}
