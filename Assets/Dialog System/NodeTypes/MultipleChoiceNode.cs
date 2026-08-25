using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;





#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogSystem
{
    public class MultipleChoiceNode
    {
#if UNITY_EDITOR
        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {
            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);
            node.name = NodeTypes.MultiDialog.ToString();

            string textSpeed = data?.fields.Find(f => f.name == "TextSpeed")?.Value ?? "5";
            string narratorText = data?.fields.Find(f => f.name == "NarratorText")?.Value ?? "Main Text";
            string color = data?.fields.Find(f => f.name == "Color")?.Value ?? "#ffffff";


            visualFunctions.AddText(node, "TextSpeed:", textSpeed, ValueTypes.Float, "How fast each character is shown");
            visualFunctions.AddText(node, "NarratorText", narratorText, ValueTypes.String, "The Text of the Person who is talking to the player");
            visualFunctions.AddText(node, "Color", color, ValueTypes.Color, "The Color Of the text");

            List<string> TextList = new List<string>();
            if (data != null)
            {
                var listField = data.fields.Find(f => f.name == "DialogText" && f.type == ValueTypes.List);
                if (listField != null && !string.IsNullOrEmpty(listField.Value))
                {
                    var items = listField.Value.Split(';');
                    foreach (var item in items)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            TextList.Add(item.Split(':')[0]);
                        }
                    }
                }
            }

            visualFunctions.AddPortTextList(node, "DialogText", TextList, ValueTypes.String);
            visualFunctions.AddPort(node, Direction.Input, "In");
            visualFunctions.AddPort(node, Direction.Output, "Out");
        }
#endif

        public async Task<string> PlayNode(DialogNodeData nodeData, DialogElement dialogElement)
        {
            Debug.LogWarning("Working");
            if (nodeData.nodeTypes != NodeTypes.MultiDialog)
                return null;

            Color color = Color.clear;
            if (UnityEngine.ColorUtility.TryParseHtmlString(nodeData.fields.Find(f => f.name == "Color")?.Value, out Color parsedColor))
            {
                color = parsedColor;
            }

            TextMeshProUGUI textField = dialogElement.narratorText;
            var ChoicePrefab = dialogElement.ChoicePrefab;
            var ChoiceParent = dialogElement.ChoiceParent;

            bool wasSelected = false;
            string NextID = null;



            if (textField == null && ChoicePrefab == null && ChoiceParent == null)
            {
                Debug.LogWarning("dialogElement node has not all Components.");
                return null;
            }

            for (int i = 1; i < ChoiceParent.childCount; i++) // 1 because 0 is the prefab
            {
                UnityEngine.Object.Destroy(ChoiceParent.GetChild(i).gameObject);
            }

            textField.text = nodeData.fields.Find(f => f.name == "NarratorText").Value;

            var fieldSave = nodeData.fields.Find(f => f.name == "DialogText").Value;
            if (fieldSave == null)
            {
                Debug.LogError("There was no DialogText Saved in MultiDialog Saving didn't work ");
                return null;
            }
            var FieldSaveDatas = fieldSave.Split(";");
            foreach (var data in FieldSaveDatas)
            {
                Debug.Log("data is :" + data);
                var CurrentPrefab = UnityEngine.Object.Instantiate(ChoicePrefab);
                CurrentPrefab.SetActive(true);
                CurrentPrefab.transform.SetParent(ChoiceParent);
                var PrefabButton = CurrentPrefab.GetComponent<Button>();
                var PrefabText = CurrentPrefab.transform.GetComponentInChildren<TextMeshProUGUI>();
                var port = data.Split(':').Last();
                PrefabText.text = data.Split(':').First();
                PrefabText.color = color;
                if (port != null)
                {
                    PrefabButton.onClick.AddListener(() =>
                    {
                        NextID = port;
                        wasSelected = true;
                    });
                }
                else if (port == null)
                {
                    PrefabButton.onClick.AddListener(() =>
                    {
                        NextID = null;
                        wasSelected = true;
                    });
                }
            }
            while (wasSelected == false)
            {
                await Task.Yield();
            }

            for (int i = 1; i < ChoiceParent.childCount; i++) // 1 because 0 is the prefab
            {
                UnityEngine.Object.Destroy(ChoiceParent.GetChild(i).gameObject);
            }
            return NextID; // The Node IDs
        }
    }
}