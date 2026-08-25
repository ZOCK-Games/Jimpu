using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;


namespace DialogSystem
{
#if UNITY_EDITOR
    [Icon("Assets/Dialog System/Icons/DialogElementIcon.png")]
#endif
    public class DialogElement : MonoBehaviour
    {
        public TextMeshProUGUI narratorText;
        public List<Button> playerChoices = new List<Button>();
        public Image BackgroundImage;
        public Image CharacterImage;
        public TextMeshProUGUI CharacterNameText;
        public Transform ChoiceParent;
        public GameObject ChoicePrefab;

#if UNITY_EDITOR
        void Reset()
        {
            if (GetComponent<RectTransform>() == null)
            {
                gameObject.AddComponent<RectTransform>();
            }

            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            string[] dialogPrefabs = AssetDatabase.FindAssets("DialogElement t:Prefab");
            foreach (string guid in dialogPrefabs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (System.IO.Path.GetFileNameWithoutExtension(path) != "DialogElement") continue;

                GameObject dialogPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (dialogPrefab == null || dialogPrefab.GetComponent<DialogElement>() == null) continue;

                GameObject instantiatedGo = PrefabUtility.InstantiatePrefab(dialogPrefab) as GameObject;

                if (PrefabUtility.IsPartOfPrefabInstance(instantiatedGo))
                {
                    PrefabUtility.UnpackPrefabInstance(instantiatedGo, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }

                DialogElement instanceData = instantiatedGo.GetComponent<DialogElement>();

                this.narratorText = instanceData.narratorText;
                this.BackgroundImage = instanceData.BackgroundImage;
                this.CharacterImage = instanceData.CharacterImage;
                this.CharacterNameText = instanceData.CharacterNameText;
                this.playerChoices = new List<Button>(instanceData.playerChoices);
                this.ChoiceParent = instanceData.ChoiceParent;
                if (instanceData.ChoiceParent != null && instanceData.ChoiceParent.childCount > 0)
                {
                    GameObject childInScene = instanceData.ChoiceParent.GetChild(0).gameObject;
                    if (childInScene != null)
                    {
                        this.ChoicePrefab = childInScene;
                        this.ChoicePrefab.SetActive(false);
                    }
                }
                List<Transform> children = new List<Transform>();
                foreach (Transform child in instantiatedGo.transform)
                {
                    children.Add(child);
                }

                foreach (Transform child in children)
                {
                    child.SetParent(this.transform, false);
                }

                DestroyImmediate(instantiatedGo);

                EditorUtility.SetDirty(this);
                break;
            }
        }
#endif
    }
}