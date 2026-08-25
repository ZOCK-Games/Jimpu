#if UNITY_EDITOR
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class DialogueGraphView : GraphView
    {
        public MainFunction mainFunction = new MainFunction();
        public VisualFunctions visualFunctions = new VisualFunctions();
        private Vector2 _lastMousePosition;
        private NodeSearchProvider _searchProvider;
        public DialogueGraphView()
        {
            GridBackground background = new GridBackground();
            Insert(0, background);
            background.StretchToParentSize();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            mainFunction.graphView = this;

            mainFunction.SetVisualFunctions(visualFunctions);
            visualFunctions.SetMainFunction(mainFunction);

            serializeGraphElements = SerializeGraphElementsImplementation;
            canPasteSerializedData = CanPasteSerializedDataImplementation;
            unserializeAndPaste = UnserializeAndPasteImplementation;

            var searchProvider = ScriptableObject.CreateInstance<NodeSearchProvider>();
            searchProvider.graphView = this;

            _searchProvider = ScriptableObject.CreateInstance<NodeSearchProvider>();
            _searchProvider.graphView = this;

            RegisterCallback<MouseMoveEvent>(evt =>
            {
                _lastMousePosition = contentViewContainer.WorldToLocal(evt.mousePosition);
            });

            nodeCreationRequest = context =>
            {
                _searchProvider.mousePosition = _lastMousePosition;
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchProvider);
            };
        }

        

        public void ClearAll()
        {
            graphElements.ForEach(elem =>
            {

                RemoveElement(elem);

            });
        }



        public override System.Collections.Generic.List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new System.Collections.Generic.List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        public void DuplicateSelectedNodes()
        {
            var selectedNodes = selection.OfType<Node>().ToList();
            if (selectedNodes.Count == 0)
            {
                return;
            }

            var duplicatedNodes = new System.Collections.Generic.List<Node>();
            foreach (var selectedNode in selectedNodes)
            {
                var duplicatedNode = mainFunction.DuplicateNode(selectedNode);
                if (duplicatedNode != null)
                {
                    duplicatedNodes.Add(duplicatedNode);
                }
            }

            if (duplicatedNodes.Count > 0)
            {
                ClearSelection();
                foreach (var node in duplicatedNodes)
                {
                    AddToSelection(node);
                }
            }
        }

        string SerializeGraphElementsImplementation(System.Collections.Generic.IEnumerable<GraphElement> elements)
        {
            var nodes = elements.OfType<Node>().Distinct().ToList();
            if (nodes.Count == 0)
            {
                return string.Empty;
            }

            var serialized = mainFunction.NodeToDialogNodeData(nodes);
            var allNodes = new AllNodes
            {
                dialogNodeDatas = serialized.Item1,
                ID = System.Guid.NewGuid().ToString()
            };
            return JsonUtility.ToJson(allNodes, true);
        }

        bool CanPasteSerializedDataImplementation(string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                return false;
            }

            serializedData = serializedData.Trim();
            if (!serializedData.StartsWith("{"))
            {
                return false;
            }

            try
            {
                var allNodes = JsonUtility.FromJson<AllNodes>(serializedData);
                return allNodes != null && allNodes.dialogNodeDatas != null && allNodes.dialogNodeDatas.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        void UnserializeAndPasteImplementation(string operationName, string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
            {
                return;
            }

            serializedData = serializedData.Trim();
            if (!serializedData.StartsWith("{"))
            {
                return;
            }

            try
            {
                var allNodes = JsonUtility.FromJson<AllNodes>(serializedData);
                if (allNodes == null || allNodes.dialogNodeDatas == null || allNodes.dialogNodeDatas.Count == 0)
                {
                    return;
                }

                var pastedNodes = mainFunction.DialogNodeDataToNode(allNodes.dialogNodeDatas);
                if (pastedNodes == null || pastedNodes.Count == 0)
                {
                    return;
                }

                foreach (var node in pastedNodes)
                {
                    var position = node.GetPosition();
                    node.SetPosition(new Rect(position.x + 40f, position.y + 40f, position.width, position.height));
                    node.viewDataKey = System.Guid.NewGuid().ToString();
                    node.userData = node.viewDataKey;
                    node.RefreshPorts();
                    node.RefreshExpandedState();
                    AddElement(node);
                }

                if (pastedNodes.Count > 0)
                {
                    ClearSelection();
                    foreach (var node in pastedNodes)
                    {
                        AddToSelection(node);
                    }
                }
            }
            catch
            {
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (selection.OfType<Node>().Any())
            {
                evt.menu.AppendAction("Duplicate", _ => DuplicateSelectedNodes(), DropdownMenuAction.AlwaysEnabled);
            }
        }
    }
    [System.Serializable]
    public class Nodes
    {
        public NodeTypes nodeType;
        [System.NonSerialized]
        public Node node;
    }
}
#endif
