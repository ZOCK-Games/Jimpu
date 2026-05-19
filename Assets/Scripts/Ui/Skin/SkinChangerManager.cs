using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinChangerManager : MonoBehaviour
{
    public List<SkinElement> skinElements;
    public GameObject prefabSkinElementDisplay;
    protected List<SkinElementDisplay> skinElementDisplays = new List<SkinElementDisplay>();
    void Start()
    {
        foreach (SkinElement element in skinElements)
        {
            var prefab = Instantiate(prefabSkinElementDisplay);
            var spriteRenderer = prefab.GetComponent<SpriteRenderer>();
            var portCollider = prefab.GetComponentInChildren<Collider2D>();  //searches for the collider which is used as a port
            if (spriteRenderer == null)
            {
                Debug.Log("There is no sprite on prefab skin element display");
                continue;
            }
            spriteRenderer.sprite = element.sprite;

            SkinElementDisplay skinElement = new SkinElementDisplay
            {
                Object = prefab,
                skinType = element.skinType,
                DisplayImage = element.sprite,
                portCollider2d = portCollider
            };

            skinElementDisplays.Add(skinElement);
        }
    }

    void Update()
    {

    }
}


[System.Serializable]
public class SkinElementDisplay
{
    public GameObject Object;
    public Collider2D portCollider2d;
    public Sprite DisplayImage;
    public SkinType skinType;
}

public enum SkinType
{
    Head,
    Body,
    ArmL,
    ArmR,
    LegL,
    LegR
}