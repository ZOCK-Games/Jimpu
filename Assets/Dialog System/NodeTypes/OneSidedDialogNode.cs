using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Direction = UnityEditor.Experimental.GraphView.Direction;
#endif

namespace DialogSystem
{
    public class OneSidedDialogNode : TextPlayingFunction
    {
#if UNITY_EDITOR
        public List<VisualElement> SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {
            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);
            node.name = NodeTypes.Dialog.ToString();

            string time = data?.fields.Find(f => f.name == "Time")?.Value ?? "5";
            string textSpeed = data?.fields.Find(f => f.name == "TextSpeed")?.Value ?? "1";
            string dialogText = data?.fields.Find(f => f.name == "NarratorText")?.Value ?? "Enter";
            string color = data?.fields.Find(f => f.name == "Color")?.Value ?? "#ffffff";

            var NarratorText = visualFunctions.AddText(node, "NarratorText", dialogText, ValueTypes.String, "The Text of the Person who is talking to the player");
            NarratorText.style.minHeight = 40;
            if (NarratorText is TextField x)
            {
                x.multiline = true;
            }
            List<VisualElement> visualElements = new List<VisualElement>
        {
            visualFunctions.AddText(node, "Time", time, ValueTypes.Float, "The  Time until the next node is starting after all the text was displayed", "Time"),
            visualFunctions.AddText(node, "TextSpeed", textSpeed, ValueTypes.Float, "How fast each character is shown", "Text Speed"),
            NarratorText,
            visualFunctions.AddText(node, "Color", color, ValueTypes.Color, "The Color Of the text", "Color"),
            visualFunctions.AddPort(node, Direction.Input, "In"),
            visualFunctions.AddPort(node, Direction.Output, "Out")
        };

            return visualElements;
        }
#endif

        public async Task PlayNode(DialogNodeData nodeData, DialogElement dialogElement)
        {
            if (nodeData.nodeTypes != NodeTypes.Dialog)
                return;

            string dialogText = nodeData.fields.Find(f => f.name == "NarratorText")?.Value;
            var ColorField = nodeData.fields.Find(f => f.name == "Color")?.Value;
            UnityEngine.ColorUtility.TryParseHtmlString(ColorField, out Color parsedColor);
            int textSpeed = 50;
            if (!float.TryParse(nodeData.fields.Find(f => f.name == "TextSpeed")?.Value, out float speed))
            {
                textSpeed = 1;
            }
            if (!float.TryParse(nodeData.fields.Find(f => f.name == "Time")?.Value, out float time))
            {
                time = 1f;
            }



            if (dialogElement != null)
            {
                await PlayText(dialogElement, dialogText, textSpeed, TextAnimations.TypewriterClean);
            }
            
            await Task.Delay((int)(time * 1000));
        }
    }
}