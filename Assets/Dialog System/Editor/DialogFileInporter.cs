#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace DialogSystem
{
    [ScriptedImporter(1, "dialog")]
    public class DialogFileImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var dialogFile = ScriptableObject.CreateInstance<DialogFile>();
            dialogFile.sourceAssetPath = ctx.assetPath;

            string fullPath = ctx.assetPath;
            if (fullPath.StartsWith("Assets"))
            {
                fullPath = Path.Combine(Application.dataPath, fullPath.Substring("Assets".Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            }

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"Dialog Importer missing source file: {fullPath}");
                dialogFile.jsonContent = string.Empty;
            }
            else
            {
                dialogFile.jsonContent = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(dialogFile.jsonContent))
                {
                    dialogFile.dialogData = JsonUtility.FromJson<AllNodes>(dialogFile.jsonContent);
                }
            }

            ctx.AddObjectToAsset("asset", dialogFile);
            ctx.SetMainObject(dialogFile);
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Dialog System/Icons/Icon.png"
            );
            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(dialogFile, icon);
            }
        }
    }
}
#endif
