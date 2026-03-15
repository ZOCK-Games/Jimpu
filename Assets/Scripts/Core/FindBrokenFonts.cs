#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

public class FindBrokenFonts : MonoBehaviour
{
    [MenuItem("Tools/Find Broken Fonts")]
    static void Find()
    {
        string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TMP_FontAsset font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            
            if (font.atlasTextures == null || font.atlasTextures.Length == 0)
                Debug.LogError("KAPUTTE FONT: " + path);
            else
                Debug.Log("OK: " + path);
        }
    }
}
#endif