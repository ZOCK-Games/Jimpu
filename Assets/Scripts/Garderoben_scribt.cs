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
    public GameObject Garderobe;
    public GameObject Settings_Canvas;
    public List<GameObject> PLayer_objekt;
    public SpriteRenderer Skin_player;

    public int Position_y;
    public PlayerData playerData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Select_button.onClick.AddListener(Select);
        Equip_button.onClick.AddListener(Equip);
        Close_button.onClick.AddListener(CLose_canvas);
        Garderobe.SetActive(false);
    }

    // Update is called once per frame
    void Select()
    {
        playerData.Skin =+ 1;
        playerData.Skin = (playerData.Skin >= 3) ? 0 : playerData.Skin;
    }
    private void Equip()
    {
        Garderobe.SetActive(false);
    }
    private void CLose_canvas()
    {
        Garderobe.SetActive(false);
    }

}
