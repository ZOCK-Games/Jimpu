using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogSystem
{
    public class DialogFile : ScriptableObject
    {
        [TextArea(5, 30)]
        public string jsonContent;
        public string sourceAssetPath;
        public AllNodes dialogData;

        [System.NonSerialized]
        private Dictionary<string, DialogNodeData> nodeLookup;

        public bool HasDialogData => dialogData != null && dialogData.dialogNodeDatas != null && dialogData.dialogNodeDatas.Count > 0;

        public AllNodes GetDialogData()
        {
            if (!HasDialogData && !string.IsNullOrWhiteSpace(jsonContent))
            {
                dialogData = JsonUtility.FromJson<AllNodes>(jsonContent);
                nodeLookup = null;
            }
            return dialogData;
        }

        public Dictionary<string, DialogNodeData> NodeLookup
        {
            get
            {
                if (nodeLookup == null)
                {
                    var data = GetDialogData();
                    if (data != null && data.dialogNodeDatas != null)
                    {
                        nodeLookup = data.dialogNodeDatas.ToDictionary(n => n.NodeID, n => n);
                    }
                    else
                    {
                        nodeLookup = new Dictionary<string, DialogNodeData>();
                    }
                }
                return nodeLookup;
            }
        }

        public DialogNodeData FindNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                return null;
            NodeLookup.TryGetValue(nodeId, out var node);
            return node;
        }
    }
}
