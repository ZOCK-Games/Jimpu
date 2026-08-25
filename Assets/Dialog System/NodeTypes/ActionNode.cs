using UnityEngine;



#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogSystem
{
    public class ActionNode
    {
#if UNITY_EDITOR
        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {
            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);
            node.name = NodeTypes.Action.ToString();

            visualFunctions.AddObjectField(node, "Character", typeof(CharacterData)).label = "Character";
            visualFunctions.AddPort(node, Direction.Input, "In");
            visualFunctions.AddPort(node, Direction.Output, "Out");
        }
#endif

        public void PlayNode(DialogNodeData node, DialogElement dialogElement)
        {
            if (node.nodeTypes == NodeTypes.Action && dialogElement != null)
            {
                var CharacterData = node.objectSaveDatas.Find(o => o.name == "Character");

                var ResourcesPath = CharacterData.path.Replace("Assets/Resources/", "").Replace(".asset", "");

                var Character = Resources.Load(ResourcesPath);

                if (Character != null && Character is CharacterData data)
                {
                    

                    if (dialogElement.CharacterImage != null)
                    {
                        dialogElement.CharacterImage.sprite = data.Image;
                    }
                    else
                    {
                        Debug.LogWarning("Cant set Character Image because there was no image found");
                    }

                    if (dialogElement.CharacterNameText != null)
                    {
                        dialogElement.CharacterNameText.text = data.Name;
                    }
                    else
                    {
                        Debug.LogWarning($"Cant set Character Name because there was no Char. Name Text found in {dialogElement}");
                    }
                }
                else
                {
                    Debug.LogWarning($"There was no character data found in {node.objectSaveDatas} or The found Character: {Character} is not a  CharacterData");
                }
            }
        }
    }
}