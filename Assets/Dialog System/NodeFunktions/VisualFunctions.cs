#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class VisualFunctions
    {
        public MainFunction mainFunction;

        public void SetMainFunction(MainFunction main)
        {
            this.mainFunction = main;
        }
        public UnityEngine.UIElements.VisualElement AddText(Node node, string Name, string DefaultValue, ValueTypes type, string ToolTip, string label = null, bool ReadOnly = false)
        {
            UnityEngine.UIElements.VisualElement Field;
            switch (type)
            {
                case ValueTypes.String:
                    Field = new UnityEngine.UIElements.TextField();
                    break;
                case ValueTypes.Float:
                    Field = new UnityEngine.UIElements.FloatField();
                    break;
                case ValueTypes.Color:
                    Field = new UnityEditor.UIElements.ColorField();
                    Field.AddToClassList("unity-color-field");
                    break;
                case ValueTypes.Vector3:
                    Field = new UnityEngine.UIElements.Vector3Field();
                    break;
                default:
                    Field = new UnityEngine.UIElements.TextField();
                    break;
            }
            if (Field is UnityEngine.UIElements.FloatField floatField)
            {
                floatField.value = float.Parse(DefaultValue);
                floatField.label = label;
            }
            else if (Field is UnityEngine.UIElements.TextField textFiled)
            {
                textFiled.value = DefaultValue;
                textFiled.label = label;
            }
            else if (Field is UnityEditor.UIElements.ColorField colorFiled)
            {
                if (UnityEngine.ColorUtility.TryParseHtmlString(DefaultValue, out Color color))
                {
                    colorFiled.value = color;
                    colorFiled.style.minWidth = 100;
                    colorFiled.hdr = true;
                    colorFiled.label = label;
                    colorFiled.style.width = 200;
                }
            }

            if (ReadOnly)
            {
                Field.SetEnabled(false);
            }
            else
            {
                Field.SetEnabled(true);
            }
            Field.name = Name;
            Field.tooltip = ToolTip;
            node.extensionContainer.Add(Field);
            node.RefreshPorts();
            node.RefreshExpandedState();
            return Field;
        }


        public ListView AddPortTextList(Node node, string Name, List<string> items, ValueTypes type)
        {
            ListView listView = null;
            var portList = new List<Port>();
            
            // Initial ports for existing items
            for (int i = 0; i < items.Count; i++)
            {
                var port = AddPort(node, Direction.Output, "Choice");
                port.name = Name + "_" + i;
                port.portName = "";
                portList.Add(port);
            }

            listView = new ListView(
                items,
                itemHeight: 24,
                makeItem: () =>
                {
                    var row = new VisualElement();
                    row.style.flexDirection = FlexDirection.Row;
                    row.style.alignItems = Align.Center;
                    row.style.height = 24;

                    var text = new TextField()
                    {
                        name = "row-text-field",
                        style = { flexGrow = 1 }
                    };

                    var portContainer = new VisualElement()
                    {
                        name = "port-container",
                        style = { width = 20, alignItems = Align.Center}
                    };

                    row.Add(text);
                    row.Add(portContainer);

                    return row;
                },
                bindItem: (el, i) =>
                {
                    var currentItems = listView.itemsSource as List<string>;
                    if (currentItems == null || i < 0 || i >= currentItems.Count)
                        return;

                    while (portList.Count <= i)
                    {
                        var newPort = AddPort(node, Direction.Output, "Choice");
                        newPort.name = Name + "_" + i;
                        newPort.portName = "";
                        portList.Add(newPort);
                    }

                    var field = el.Q<TextField>("row-text-field");
                    if (field != null)
                    {
                        int index = i;
                        field.value = currentItems[index];

                        // Clean up old callback if present
                        if (field.userData is EventCallback<ChangeEvent<string>> oldCallback)
                        {
                            field.UnregisterValueChangedCallback(oldCallback);
                        }

                        EventCallback<ChangeEvent<string>> newCallback = evt =>
                        {
                            if (index < currentItems.Count) currentItems[index] = evt.newValue;
                        };

                        field.RegisterValueChangedCallback(newCallback);
                        field.userData = newCallback;
                    }

                    var portContainer = el.Q("port-container");
                    if (portContainer != null)
                    {
                        portContainer.Clear();
                        if (i < portList.Count)
                        {
                            portContainer.Add(portList[i]);
                        }
                    }
                }
            );
            
            listView.userData = portList;
            listView.name = Name;
            listView.showAddRemoveFooter = true;
            listView.reorderable = true;
            listView.style.minWidth = 200;

            var label = new Label(Name);
            node.extensionContainer.Add(label);
            node.extensionContainer.Add(listView);

            node.RefreshExpandedState();
            return listView;
        }
        public Port AddPort(Node node, UnityEditor.Experimental.GraphView.Direction direction, string PortName)
        {
            var listener = new EdgeConnectorListener();
            var connector = new EdgeConnector<UnityEditor.Experimental.GraphView.Edge>(listener);

            Port port = Port.Create<UnityEditor.Experimental.GraphView.Edge>(
                Orientation.Horizontal,
                direction,
                Port.Capacity.Multi,
                typeof(float)
            );

            port.RemoveManipulator(port.edgeConnector);
            port.AddManipulator(connector);
            port.name = PortName;
            port.portName = PortName;
            port.userData = node; 
            if (direction == UnityEditor.Experimental.GraphView.Direction.Input)
            {
                node.inputContainer.Add(port);
            }
            else if (direction == UnityEditor.Experimental.GraphView.Direction.Output)
            {
                node.outputContainer.Add(port);
            }
            node.RefreshPorts();
            node.RefreshExpandedState();
            return port;
        }

        public ObjectField AddObjectField(Node node, string name, Type objectType, UnityEngine.Object defaultValue = null)
        {
            var audioField = new ObjectField()
            {
                objectType = objectType,
                allowSceneObjects = false,
                value = defaultValue,
                name = name,
                tooltip = name
            };

            audioField.RegisterValueChangedCallback(evt =>
            {
                var clip = evt.newValue as AudioClip;
                Debug.Log("Selected clip: " + clip);
            });
            node.extensionContainer.Add(audioField);
            return audioField;
        }

        public DropdownField AddDropDownField(Node node, string name, List<string> Options = null, bool ShowName = false)
        {
            DropdownField dropdownField = new DropdownField
            {
                name = name,
                choices = Options ?? Enum.GetNames(typeof(NodeTypes)).ToList(),
                value = "Choose",
                tooltip = name
            };

            if (ShowName)
            {
                dropdownField.label = name;
            }

            node.extensionContainer.Add(dropdownField);
            return dropdownField;
        }

        public ValueTypes GetValueType(VisualElement visualElement)
        {
            switch (visualElement)
            {
                case UnityEngine.UIElements.TextField:
                    return ValueTypes.String;
                case UnityEngine.UIElements.FloatField:
                    return ValueTypes.Float;
                case UnityEditor.UIElements.ColorField:
                    return ValueTypes.Color;
                case UnityEngine.UIElements.Vector3Field:
                    return ValueTypes.Vector3;
                default:
                    return ValueTypes.nothing;
            }
        }
    }
}
#endif
