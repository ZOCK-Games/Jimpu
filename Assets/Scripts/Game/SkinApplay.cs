using System.Collections.Generic;
using UnityEngine;

public class SkinApplay : MonoBehaviour
{
    public GameObject BodyPartsContainer;
    public List<Skins> PlayerSkins;
    private int SkinIndex;
    public void CheckSkin()
    {
        BodyPartsContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Head;
        BodyPartsContainer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Body;
        BodyPartsContainer.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftArm;
        BodyPartsContainer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightArm;
        BodyPartsContainer.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftLeg;
        BodyPartsContainer.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightLeg;
    }
        public void LoadGame(SaveManager manager)
    {
        SkinIndex = manager.playerDataSO.SkinIndex;
        CheckSkin();
        //Player.GetComponent<SpriteRenderer>().sprite = SkinSprite[SkinIndex];
        if (UnityEngine.ColorUtility.TryParseHtmlString("#" + manager.playerDataSO.colorHex, out Color colorHex))
            for (int i = 0; i < BodyPartsContainer.transform.childCount; i++)
            {
                BodyPartsContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorHex;
            }
    }
}
