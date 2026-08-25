using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif


namespace DialogSystem
{
    public class StartNode
    {
            #if UNITY_EDITOR
        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {

            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);

            string description = data?.fields.Find(f => f.name == "Description")?.Value ?? "Description";

            visualFunctions.AddPort(node, Direction.Input, "Start");
            visualFunctions.AddPort(node, Direction.Output, "Out");
            visualFunctions.AddText(node, "Description", description, ValueTypes.String, "The Description of the current Dialog File");
            node.title = "Start";

        }
            #endif

        public void PlayNode(DialogElement dialogElement)
        {
            dialogElement.gameObject.SetActive(true);
        }
    }
}

