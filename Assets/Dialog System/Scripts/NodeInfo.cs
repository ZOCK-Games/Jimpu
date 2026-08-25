#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

namespace DialogSystem
{
    public class NormalNode : Node
    {
        public string nodeID;

        public NormalNode()
        {
            nodeID = System.Guid.NewGuid().ToString();
        }
    }
}
#endif
