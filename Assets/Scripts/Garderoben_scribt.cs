using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GarderobenScribt : MonoBehaviour
{
    //public Image Skin1;
    public UnityEngine.UI.Button NextSkinButton;
    public UnityEngine.UI.Button CloseButton;
    public GameObject Garderobe;
    public int SkinIndex = 0;
    [Header("Skin Sprites")]
    public Image SkinShowcase;
    public List<Sprite> SkinSprites;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NextSkinButton.onClick.AddListener(NextSkin);
        CloseButton.onClick.AddListener(CloseGarderobe);
        Garderobe.SetActive(false);
        LoadSkin();
    }

    // Update is called once per frame
    void NextSkin()
    {
        SkinIndex += 1;
        SkinIndex = (SkinIndex >= SkinSprites.Count) ? 0 : SkinIndex;
        Debug.Log("Current Skin Index: " + SkinIndex);
        LoadSkin();
    }
    private void CloseGarderobe()
    {
        Garderobe.SetActive(false);
    }

    private void LoadSkin()
    {
        SkinShowcase.sprite = SkinSprites[SkinIndex];
    }
    public void LoadGame(GameData data) // Load the value from GameData
    {
        this.SkinIndex = data.SkinIndex; // Load value from GameData
        LoadSkin();
        Debug.Log("Loaded Skin Index: " + SkinIndex);
    }
        public void SaveGame(ref GameData data) // Save the current value to GameData
    {
        data.SkinIndex = this.SkinIndex; //Value to saved data
    }

}
