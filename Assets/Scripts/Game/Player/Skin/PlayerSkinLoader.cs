using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour, IDataPersitence
{
    public List<PlayerSkinElement> playerSkinElements = new List<PlayerSkinElement>();
    private string SkinElementPath = "SkinChanger";

    public void LoadData(SaveManager manager)
    {
        foreach (var part in manager.dataSOs.playerDataSO.bodyParts)
        {
            var skinPart = playerSkinElements.Find(x => x.skinType.ToString() == part.BodyPartType);

            if (skinPart != null)
            {
                List<SkinElement> skinElements = Resources.LoadAll<SkinElement>(SkinElementPath).ToList();

                SkinElement element = skinElements.Find(x => x.ID == part.BodyElementID);

                if (element != null && ColorUtility.TryParseHtmlString("#" + part.ColorHex, out Color partColor))
                {
                    skinPart.sprite.color = partColor;
                    skinPart.sprite.sprite = element.sprite;
                }
                else
                {
                    Debug.LogWarning("There was an error while loading the skin");
                }

            }
        }
    }

    public void SaveData(SaveManager manager)
    {
    }
}
[System.Serializable]
public class PlayerSkinElement
{
    public SkinType skinType;
    public SpriteRenderer sprite;
}
