using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class GarderobenScribt : MonoBehaviour, IDataPersitence
{
    //public Image Skin1;
    public UnityEngine.UI.Button NextSkinButton;
    public UnityEngine.UI.Button CloseButton;
    public GameObject Garderobe;
    public int SkinIndex = 0;
    [Header("Skin Sprites")]
    public UnityEngine.UI.Button SkinSaveButton;
    [Header("Skins")]
    [SerializeField] public List<Skins> PlayerSkins;

    [Header("Skin Body parts")]
    [SerializeField] public Image Head;
    [SerializeField] public Image Body;
    [SerializeField] public Image LeftArm;
    [SerializeField] public Image RightArm;
    [SerializeField] public Image LeftLeg;
    [SerializeField] public Image RightLeg;

    void Start()
    {
        NextSkinButton.onClick.AddListener(NextSkin);
        CloseButton.onClick.AddListener(CloseGarderobe);
        SkinSaveButton.onClick.AddListener(() => SaveManager.instance.Save());
        LoadSkin();
    }

    // Update is called once per frame
    void NextSkin()
    {
        SkinIndex++;
        if (SkinIndex >= PlayerSkins.Count)
        {
            SkinIndex = 0;
        }
        Debug.Log("Current Skin Index: " + SkinIndex);
        LoadSkin();
    }
    private void CloseGarderobe()
    {
        SceneManager.LoadScene("Start Screen");
    }

    private void LoadSkin()
    {
        Head.sprite = PlayerSkins[SkinIndex].Head;
        Body.sprite = PlayerSkins[SkinIndex].Body;
        LeftArm.sprite = PlayerSkins[SkinIndex].LeftArm;
        RightArm.sprite = PlayerSkins[SkinIndex].RightArm;
        LeftLeg.sprite = PlayerSkins[SkinIndex].LeftLeg;
        RightLeg.sprite = PlayerSkins[SkinIndex].RightLeg;
    }




    public void LoadData(SaveManager manager)
    {
        this.SkinIndex = manager.playerDataSO.SkinIndex;
        LoadSkin();
    }

    public void SaveData(SaveManager manager)
    {
        manager.playerDataSO.SkinIndex = this.SkinIndex;
    }
}
