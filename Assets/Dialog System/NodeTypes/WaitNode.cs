using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif


namespace DialogSystem
{
    public class WaitNode
    {
        #if UNITY_EDITOR

        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {

            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);

            string time = data?.fields.Find(f => f.name == "Time")?.Value ?? "0";


            visualFunctions.AddPort(node, Direction.Input, "Start");
            visualFunctions.AddPort(node, Direction.Output, "Out");
            visualFunctions.AddText(node, "Time", time, ValueTypes.Float, "The Time to wait till the next dialog");
            node.title = "Wait";
        }
        #endif

        public async Task PlayNode(DialogNodeData data)
        {
            var x = data.fields.Find(x => x.type == ValueTypes.Float && x.name == "Time");
            if (x != null)
            {
                float time = float.Parse(x.Value);
                int intTime = (int)(time * 1000);
                await Task.Delay(intTime);
            }
        }
    }
}

