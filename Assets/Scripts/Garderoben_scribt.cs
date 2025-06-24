using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Garderoben_scribt : MonoBehaviour
{
    //public Image Skin1;
    public UnityEngine.UI.Button Select_button;
    public UnityEngine.UI.Button Equip_button;
    public UnityEngine.UI.Button Close_button;
    public Canvas Garderobe;
    public Canvas Settings_Canvas;
    public List<GameObject> PLayer_objekt;
    public SpriteRenderer Skin_player;
    public Sprite SKin_1;
    public Sprite SKin_2;
    public Sprite SKin_3;
    public Sprite SKin_4;

    public int Position_y;
    int Skin;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Select_button.onClick.AddListener(Select);
        Equip_button.onClick.AddListener(Equip);
        Close_button.onClick.AddListener(CLose_canvas);
        Garderobe.enabled = false;



    }

    // Update is called once per frame
    void Update()
    {



    }
    private void Select()
    {
        if (Skin == 0)
        {
            PLayer_objekt[1].SetActive(false);
            PLayer_objekt[2].SetActive(false);
            PLayer_objekt[3].SetActive(false);
            PLayer_objekt[0].SetActive(true);
            Skin_player.sprite = SKin_1;
            Skin = 1;
        }
        else if (Skin == 1)
        {
            PLayer_objekt[2].SetActive(false);
            PLayer_objekt[3].SetActive(false);
            PLayer_objekt[0].SetActive(false);
            PLayer_objekt[1].SetActive(true);
            Skin_player.sprite = SKin_2;
            Skin = 2;
        }
        else if (Skin == 2)
        {
            PLayer_objekt[0].SetActive(false);
            PLayer_objekt[1].SetActive(false);
            PLayer_objekt[3].SetActive(false);
            PLayer_objekt[2].SetActive(true);
            Skin_player.sprite = SKin_3;
            Skin = 3;
        }
        else if (Skin == 3)
        {
            PLayer_objekt[0].SetActive(false);
            PLayer_objekt[1].SetActive(false);
            PLayer_objekt[2].SetActive(false);
            PLayer_objekt[3].SetActive(true);
            Skin_player.sprite = SKin_4;
            Skin = 0;
        }




    }
    private void Equip()
    {
        Garderobe.enabled = false;
    }
    private void CLose_canvas()
    {
        Garderobe.enabled = false;
    }

}
