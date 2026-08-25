using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogSystem
{
    public class EndNode
    {
#if UNITY_EDITOR
        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions)
        {
            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            node.name = NodeTypes.End.ToString();

            visualFunctions.AddPort(node, Direction.Input, "In");
        }
#endif
        public void PlayNode(DialogElement dialogElement)
        {
            Debug.Log("Dialog ended!");
            dialogElement.CharacterImage.sprite = null;
            dialogElement.CharacterNameText.text = null;
            dialogElement.narratorText.text = null;
            dialogElement.narratorText.color = Color.black;
            for (int i = 1; i < dialogElement.ChoiceParent.childCount; i++) // 1 because 0 is the prefab
            {
                UnityEngine.Object.Destroy(dialogElement.ChoiceParent.GetChild(i).gameObject);
            }
            dialogElement.gameObject.SetActive(false);
        }

    }
}