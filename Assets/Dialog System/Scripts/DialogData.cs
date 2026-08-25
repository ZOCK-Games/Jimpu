using System.Collections.Generic;

namespace DialogSystem
{
    [System.Serializable]
    public class AllNodes
    {
        public List<DialogNodeData> dialogNodeDatas = new List<DialogNodeData>();
        public string desciption;
        public string ID;

    }
    /// <summary>
    /// Can Only Be used for One Sided Dialogs
    /// and Multiple Choice Dialogs but not 
    /// other node types
    /// </summary>
    [System.Serializable]
    public class DialogNodeData
    {
        public string NodeName;
        public NodeTypes nodeTypes;
        public float PosX, PosY;
        public float width, height;
        public List<FieldSaveData> fields = new List<FieldSaveData>();
        public List<DropDownFieldData> dropDownFields = new List<DropDownFieldData>();
        public List<ObjectSaveData> objectSaveDatas = new List<ObjectSaveData>();
        public string NodeID;
        public List<EdgeSaveData> edgeSaveDatas = new List<EdgeSaveData>();
        [System.NonSerialized]
        public List<DialogNodeData> ConnectedNodes = new List<DialogNodeData>();
    }


    [System.Serializable]
    public class EdgeSaveData
    {
        public string InputNodeID;        // Node die die Verbindung empfängt
        public string InputPortName;      // Input-Port der Ziel-Node
        public string OutputPortName;     // Output-Port des aktuellen Nodes (der Output-Node speichert das)
    }

    /// <summary>
    /// For InputFields
    /// (int, string float)
    /// </summary>
    [System.Serializable]
    public class FieldSaveData
    {
        public string name;
        public string Value;
        public ValueTypes type;
    }
    [System.Serializable]
    public class ObjectSaveData
    {
        public string name;
        public string path;
        public string typeName;
        public bool isAsset;
        [System.NonSerialized]
        public UnityEngine.Object Value;
    }
    [System.Serializable]
    public class DropDownFieldData
    {
        public string Value; ////////////////////////////////////////////
        public string name;
        public List<string> DropDownChoices = new List<string>();
    }

}