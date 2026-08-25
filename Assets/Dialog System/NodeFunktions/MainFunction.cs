#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class MainFunction
    {
        public VisualFunctions visualFunctions;
        public GraphView graphView;

        public void SetVisualFunctions(VisualFunctions visual)
        {
            this.visualFunctions = visual;
        }


        public Node CreateNewNode(string Title, Rect rect, NodeTypes nodeTypes)
        {
            NormalNode NewNode = new NormalNode
            {
                name = nodeTypes.ToString(),
                title = Title,
                nodeID = System.Guid.NewGuid().ToString()
            };

            NewNode.viewDataKey = NewNode.nodeID;
            NewNode.userData = NewNode.nodeID;
            visualFunctions.AddDropDownField(NewNode, Title + "DropDown");
            NewNode.SetPosition(rect);
            graphView.AddElement(NewNode);
            Nodes node = new Nodes
            {
                node = NewNode,
                nodeType = nodeTypes
            };
            DialogSaver.instance.nodes.Add(node);
            NewNode.RefreshPorts();
            NewNode.RefreshExpandedState();
            return NewNode;
        }

        public Node DuplicateNode(Node sourceNode)
        {
            if (sourceNode == null)
            {
                return null;
            }

            var serializedNode = NodeToDialogNodeData(new List<Node> { sourceNode });
            if (serializedNode.Item1 == null || serializedNode.Item1.Count == 0)
            {
                return null;
            }

            var clonedData = serializedNode.Item1[0];
            clonedData.NodeID = System.Guid.NewGuid().ToString();

            var duplicatedNode = DialogNodeDataToNode(new List<DialogNodeData> { clonedData }).FirstOrDefault();
            if (duplicatedNode == null)
            {
                return null;
            }

            var sourceRect = sourceNode.GetPosition();
            duplicatedNode.SetPosition(new Rect(sourceRect.x + 40f, sourceRect.y + 40f, sourceRect.width, sourceRect.height));
            duplicatedNode.viewDataKey = clonedData.NodeID;
            duplicatedNode.userData = clonedData.NodeID;

            if (graphView != null)
            {
                graphView.AddElement(duplicatedNode);
                graphView.ClearSelection();
                graphView.AddToSelection(duplicatedNode);
            }

            return duplicatedNode;
        }

        public void RemoveExtensionElement(Node node, String ElementName = "DropDownField")
        {
            List<VisualElement> visualElements = new List<VisualElement>();
            foreach (var element in node.extensionContainer.Children())
            {
                if (element.name == ElementName)
                {
                    if (node.extensionContainer.Contains(element))
                    {
                        visualElements.Add(element);
                    }
                }
            }
            foreach (VisualElement visualElement in visualElements)
            {
                node.extensionContainer.Remove(visualElement);
            }
        }

        public void RemoveExtensionElements(Node node, String Exclude = "DropDownField")
        {
            List<VisualElement> visualElements = new List<VisualElement>();
            foreach (var element in node.extensionContainer.Children())
            {
                if (element.name != Exclude)
                {
                    if (node.extensionContainer.Contains(element))
                    {
                        visualElements.Add(element);
                    }
                }
            }
            foreach (VisualElement visualElement in visualElements)
            {
                node.extensionContainer.Remove(visualElement);
            }
        }
        public void RemovePorts(Node node, UnityEditor.Experimental.GraphView.Direction direction, String Exclude = "DropDownField")
        {
            VisualElement container = direction == UnityEditor.Experimental.GraphView.Direction.Input
        ? node.inputContainer
        : node.outputContainer;
            List<Port> Ports = new List<Port>();
            foreach (Port ports in container.Children())
            {
                if (ports.name != Exclude)
                {
                    if (container.Contains(ports))
                    {
                        Ports.Add(ports);
                    }
                }
            }
            foreach (Port port in Ports)
            {
                container.Remove(port);
            }
        }
        #region Check Drop Down
        public NodeTypes CheckDropDown(Node node, DialogNodeData data = null)
        {
            if (node != null)
            {
                switch (node.name)
                {
                    case "Start":
                        StartNode startNode = new StartNode();
                        startNode.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.Start;
                    case "Dialog":
                        OneSidedDialogNode dialogNode = new OneSidedDialogNode();
                        dialogNode.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.Dialog;
                    case "MultiDialog":
                        MultipleChoiceNode multipleChoice = new MultipleChoiceNode();
                        multipleChoice.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.MultiDialog;
                    case "Audio":
                        AudioNode audioNode = new AudioNode();
                        audioNode.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.Audio;
                    case "Action":
                        ActionNode actionNode = new ActionNode();
                        actionNode.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.Action;
                    case "Wait":
                        WaitNode waitNode = new WaitNode();
                        waitNode.SetDialogNode(node, this, visualFunctions, data);
                        return NodeTypes.Wait;
                    case "End":
                        EndNode endNode = new EndNode();
                        endNode.SetDialogNode(node, this, visualFunctions);
                        return NodeTypes.End;
                    default:
                        return NodeTypes.nothing;
                }
            }
            else
            {
                Debug.LogWarning("There is no dropdownField");
                return NodeTypes.Start;
            }
        }
        #endregion

        public ValueTypes GetValueType(VisualElement visualElement)
        {
            switch (visualElement.GetType())
            {
                case Type t when t == typeof(UnityEngine.UIElements.TextField):
                    return ValueTypes.String;
                case Type t when t == typeof(UnityEngine.UIElements.FloatField):
                    return ValueTypes.Float;
                case Type t when t == typeof(UnityEditor.UIElements.ColorField):
                    return ValueTypes.Color;
                case Type t when t == typeof(UnityEngine.UIElements.Vector3Field):
                    return ValueTypes.Vector3;
                default:
                    return ValueTypes.nothing;
            }
        }

        /// <summary>
        /// Converts a Node To node data 
        /// for saving
        /// </summary>
        /// <param name="Node To Dialog Node Data"></param>
        /// <returns></returns>
        #region  Node To Node Data
        public (List<DialogNodeData>, List<EdgeSaveData>) NodeToDialogNodeData(List<Node> node)
        {
            List<DialogNodeData> dialogNodeDatas = new List<DialogNodeData>();

            for (int i = 0; i < node.Count; i++)
            {
                var outputPorts = node[i].outputContainer.Children().OfType<Port>().ToList();
                var inputPorts = node[i].inputContainer.Children().OfType<Port>().ToList();
                Debug.Log($"Node '{node[i].title}': {outputPorts.Count} output ports [{string.Join(", ", outputPorts.Select(p => p.name))}], {inputPorts.Count} input ports [{string.Join(", ", inputPorts.Select(p => p.name))}]");

                List<FieldSaveData> fieldSave = new List<FieldSaveData>();
                #region Save Fields
                foreach (var field in node[i].extensionContainer.Children().OfType<UnityEngine.UIElements.TextField>())
                {
                    fieldSave.Add(new FieldSaveData
                    {
                        name = field.name,
                        Value = field.value,
                        type = ValueTypes.String
                    });
                }

                foreach (var field in node[i].extensionContainer.Children().OfType<UnityEngine.UIElements.FloatField>())
                {
                    fieldSave.Add(new FieldSaveData
                    {
                        name = field.name,
                        Value = field.value.ToString(),
                        type = ValueTypes.Float
                    });
                }

                foreach (var field in node[i].extensionContainer.Children().OfType<UnityEngine.UIElements.Vector3Field>())
                {
                    fieldSave.Add(new FieldSaveData
                    {
                        name = field.name,
                        Value = field.value.ToString(),
                        type = ValueTypes.Vector3
                    });
                }

                foreach (var field in node[i].extensionContainer.Children().OfType<UnityEditor.UIElements.ColorField>())
                {
                    fieldSave.Add(new FieldSaveData
                    {
                        name = field.name,
                        Value = "#" + ColorUtility.ToHtmlStringRGBA(field.value),
                        type = ValueTypes.Color
                    });
                }

                foreach (var listView in node[i].extensionContainer.Children().OfType<ListView>())
                {
                    List<string> list = new List<string>();

                    if (listView.itemsSource is List<string> sourceData)
                    {
                        var choicePorts = listView.Query<Port>().Where(p => p.direction == UnityEditor.Experimental.GraphView.Direction.Output).ToList();

                        for (int x = 0; x < sourceData.Count; x++)
                        {
                            string textValue = sourceData[x];
                            string connectedNodeID = "";

                            Port foundPort = x < choicePorts.Count ? choicePorts[x] : null;

                            if (foundPort != null && foundPort.connections != null)
                            {
                                var edge = foundPort.connections.FirstOrDefault();
                                if (edge != null && edge.input != null)
                                {
                                    var inputNode = (edge.input.node as NormalNode) ?? (edge.input.userData as NormalNode);
                                    if (inputNode != null)
                                    {
                                        connectedNodeID = inputNode.viewDataKey;
                                    }
                                }
                            }

                            string data = $"{textValue}:{connectedNodeID}";
                            list.Add(data);
                        }
                    }

                    string valueString = string.Join(";", list);

                    fieldSave.Add(new FieldSaveData
                    {
                        name = listView.name,
                        Value = valueString,
                        type = ValueTypes.List
                    });
                }
                #endregion

                List<DropDownFieldData> dropDownFieldData = new List<DropDownFieldData>();
                foreach (var dropdownField in node[i].extensionContainer.Children().OfType<DropdownField>())
                {
                    dropDownFieldData.Add(new DropDownFieldData
                    {
                        Value = dropdownField.value,
                        name = dropdownField.name,
                        DropDownChoices = dropdownField.choices
                    });
                }

                List<ObjectSaveData> objectSaveDatas = new List<ObjectSaveData>();
                foreach (var objectField in node[i].extensionContainer.Children().OfType<ObjectField>())
                {
                    bool isAsset = !(objectField.value is UnityEngine.Component);
                    objectSaveDatas.Add(new ObjectSaveData
                    {
                        name = objectField.name,
                        path = isAsset
        ? AssetDatabase.GetAssetPath(objectField.value)
        : "No Path Found",
                        typeName = objectField.objectType.AssemblyQualifiedName,
                        isAsset = isAsset
                    });
                }

                List<EdgeSaveData> edgeSaveDatas = new List<EdgeSaveData>();
                var allOutputPorts = node[i].mainContainer.Query<Port>().Where(p => p.direction == UnityEditor.Experimental.GraphView.Direction.Output).ToList();

                foreach (var port in allOutputPorts)
                {
                    if (port.connections != null && port.connections.Count() > 0)
                    {
                        foreach (var edge in port.connections)
                        {
                            if (edge.input != null && edge.output != null)
                            {
                                var inputNode = (edge.input.node as Node) ?? (edge.input.userData as Node);
                                var outputNode = (edge.output.node as Node) ?? (edge.output.userData as Node);

                                if (inputNode != null && outputNode != null)
                                {
                                    EdgeSaveData edgeSaveData = new EdgeSaveData
                                    {
                                        InputNodeID = inputNode.viewDataKey,
                                        InputPortName = edge.input.name,
                                        OutputPortName = edge.output.name
                                    };
                                    edgeSaveDatas.Add(edgeSaveData);
                                    Debug.Log($"Node '{node[i].title}' Output-Edge: {edge.output.name} -> {inputNode.title}.{edge.input.name}");
                                }
                            }
                        }
                    }
                }

                Debug.Log($"Node '{node[i].title}' hat {edgeSaveDatas.Count} edges");

                dialogNodeDatas.Add(new DialogNodeData
                {
                    NodeName = node[i].title,
                    nodeTypes = CheckDropDown(node[i]),
                    PosX = node[i].GetPosition().x,
                    PosY = node[i].GetPosition().y,
                    height = node[i].GetPosition().height,
                    width = node[i].GetPosition().width,
                    fields = fieldSave,
                    dropDownFields = dropDownFieldData,
                    objectSaveDatas = objectSaveDatas,
                    NodeID = node[i].viewDataKey,
                    ConnectedNodes = new List<DialogNodeData>(),
                    edgeSaveDatas = edgeSaveDatas
                });
            }

            return (dialogNodeDatas, new List<EdgeSaveData>());
        }
        #endregion

        #region DialogNodeDataToNode
        public List<Node> DialogNodeDataToNode(List<DialogNodeData> nodeDatas)
        {
            List<Node> nodes = new List<Node>();
            foreach (var node in nodeDatas)
            {
                if (nodes.Any(n => n.viewDataKey == node.NodeID))
                {
                    continue;
                }
                Rect rect = new Rect
                {
                    width = node.width,
                    height = node.height,
                    x = node.PosX,
                    y = node.PosY
                };

                Node NewNode = CreateNewNode(node.NodeName, rect, node.nodeTypes);
                NewNode.viewDataKey = node.NodeID;
                DropdownField dropdownField = visualFunctions.AddDropDownField(NewNode, "DropDown");
                dropdownField.value = node.nodeTypes.ToString();

                CheckDropDown(NewNode, node);
                #region Fields
                foreach (FieldSaveData field in node.fields)
                {
                    if (field.type == ValueTypes.String)
                    {
                        var textField = NewNode.extensionContainer.Q<UnityEngine.UIElements.TextField>(field.name);
                        if (textField != null)
                        {
                            textField.value = field.Value;
                        }
                    }
                    else if (field.type == ValueTypes.Float)
                    {
                        var floatField = NewNode.extensionContainer.Q<UnityEngine.UIElements.FloatField>(field.name);
                        if (floatField != null)
                        {
                            floatField.value = float.Parse(field.Value);
                        }
                    }
                    else if (field.type == ValueTypes.Color)
                    {
                        var colorField = NewNode.extensionContainer.Q<UnityEditor.UIElements.ColorField>(field.name);
                        if (colorField != null)
                        {
                            colorField.value = UnityEngine.ColorUtility.TryParseHtmlString(field.Value, out Color parsedColor) ? parsedColor : Color.clear;
                        }
                    }
                    else if (field.type == ValueTypes.Vector3)
                    {
                        var vectorField = NewNode.extensionContainer.Q<UnityEngine.UIElements.Vector3Field>(field.name);
                        if (vectorField != null)
                        {
                            string sVector = field.Value;

                            sVector = sVector.Replace("(", "").Replace(")", "");

                            string[] sArray = sVector.Split(',');

                            if (sArray.Length == 3)
                            {
                                float x = float.Parse(sArray[0], CultureInfo.InvariantCulture);
                                float y = float.Parse(sArray[1], CultureInfo.InvariantCulture);
                                float z = float.Parse(sArray[2], CultureInfo.InvariantCulture);

                                vectorField.value = new Vector3(x, y, z);
                            }
                        }
                    }
                    else if (field.type == ValueTypes.List)
                    {
                        var dataList = new List<string>();
                        if (field.Value != null)
                        {
                            var items = field.Value.Split(';');
                            for (int itemIdx = 0; itemIdx < items.Length; itemIdx++)
                            {
                                string item = items[itemIdx];
                                if (!string.IsNullOrEmpty(item))
                                {
                                    string[] parts = item.Split(':');
                                    string textName = parts[0];
                                    dataList.Add(textName);
                                }
                            }
                        }
                        var listView = NewNode.extensionContainer.Q<ListView>();
                        if (listView != null)
                        {
                            listView.name = field.name;
                            listView.itemsSource = dataList;
                            listView.Rebuild();
                        }
                    }
                }
                #endregion

                foreach (DropDownFieldData dropDownField in node.dropDownFields)
                {
                    var ddField = NewNode.extensionContainer.Q<DropdownField>(dropDownField.name);
                    if (ddField != null)
                    {
                        ddField.value = dropDownField.Value;
                        ddField.choices = dropDownField.DropDownChoices;
                    }
                }

                foreach (ObjectSaveData objectSave in node.objectSaveDatas)
                {
                    Type type = Type.GetType(objectSave.typeName);
                    UnityEngine.Object obj = null;
                    if (objectSave.isAsset)
                    {
                        obj = UnityEditor.AssetDatabase.LoadAssetAtPath(objectSave.path, type);
                    }
                    else
                    {
                        obj = UnityEngine.GameObject.Find(objectSave.path)?.GetComponent(type);
                    }
                    objectSave.Value = obj;
                    var objField = NewNode.extensionContainer.Q<ObjectField>(objectSave.name);
                    if (objField != null)
                    {
                        objField.value = obj;
                    }
                }
                nodes.Add(NewNode);
            }
            return nodes;
        }
        #endregion

    }
}
#endif
