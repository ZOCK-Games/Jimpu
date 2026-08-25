#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace DialogSystem
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        public void OnDrop(GraphView graphView, Edge edge)
        {
            Debug.Log($"EdgeConnectorListener OnDrop: input={(edge.input?.node != null ? edge.input.node.name : "null")} output={(edge.output?.node != null ? edge.output.node.name : "null")}");
            edge.input.Connect(edge);
            edge.output.Connect(edge);

            graphView.AddElement(edge);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {

        }
    }
}
#endif
