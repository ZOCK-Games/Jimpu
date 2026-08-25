#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class NodeSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public Vector2 mousePosition;
        public DialogueGraphView graphView;
        private Vector2 _lastMousePosition;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Nodes"), 0),
            new SearchTreeEntry(new GUIContent("Start"))      { level = 1, userData = NodeTypes.Start },
            new SearchTreeEntry(new GUIContent("Dialog"))      { level = 1, userData = NodeTypes.Dialog },
            new SearchTreeEntry(new GUIContent("MultiDialog")) { level = 1, userData = NodeTypes.MultiDialog },
            new SearchTreeEntry(new GUIContent("Audio"))       { level = 1, userData = NodeTypes.Audio },
            new SearchTreeEntry(new GUIContent("Action"))      { level = 1, userData = NodeTypes.Action },
            new SearchTreeEntry(new GUIContent("Wait"))      { level = 1, userData = NodeTypes.Wait },
            new SearchTreeEntry(new GUIContent("End"))      { level = 1, userData = NodeTypes.End },

        };
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            var nodeType = (NodeTypes)entry.userData;

            var node = graphView.mainFunction.CreateNewNode(
                nodeType.ToString(),
                new Rect(mousePosition, new Vector2(200, 150)),
                nodeType
            );
            var dropdown = node.extensionContainer.Q<UnityEngine.UIElements.DropdownField>();
            dropdown.value = nodeType.ToString();
            graphView.mainFunction.CheckDropDown(node);
            return true;
        }
    }
}
#endif
