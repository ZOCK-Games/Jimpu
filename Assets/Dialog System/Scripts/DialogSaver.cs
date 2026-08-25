#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem
{
    public class DialogSaver
    {
        private static DialogSaver _instance;
        private AllNodes allNodes = new AllNodes();
        public List<UnityEditor.Experimental.GraphView.Edge> allEdges = new List<UnityEditor.Experimental.GraphView.Edge>();
        public MainFunction mainFunction;
        public DialogueGraphView dialogueGraphView;
        public VisualFunctions visualFunctions = new VisualFunctions();
        public static Action<string> loadedNodePath;
        public string currentNodeID;
        public static DialogSaver instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new DialogSaver();
                }
                return _instance;
            }
        }
        public List<Nodes> nodes = new List<Nodes>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetStaticData()
        {
            _instance = null;
            loadedNodePath = null;
        }


        #region  Save System
        public void SaveGraph()
        {
            if (dialogueGraphView != null)
            {
                var graphElementEdges = dialogueGraphView.graphElements.OfType<UnityEditor.Experimental.GraphView.Edge>().ToList();
                var edgesProperty = dialogueGraphView.edges;

                Debug.Log($"GraphView graphElements edge count: {graphElementEdges.Count}");
                Debug.Log(edgesProperty == null ? "dialogueGraphView.edges is null" : $"dialogueGraphView.edges count: {edgesProperty.Count()}");

                allEdges = edgesProperty
                    .Where(e =>
                    {
                        bool hasInput = e.input != null;
                        bool hasOutput = e.output != null;

                        var inputNode = (e.input?.node as Node) ?? (e.input?.userData as Node);
                        var outputNode = (e.output?.node as Node) ?? (e.output?.userData as Node);

                        bool inputHasNode = inputNode != null;
                        bool outputHasNode = outputNode != null;

                        if (!hasInput || !hasOutput || !inputHasNode || !outputHasNode)
                        {
                            Debug.LogWarning($"Filtering out edge: input={hasInput} output={hasOutput} inputNode={inputHasNode} outputNode={outputHasNode}");
                            return false;
                        }
                        return true;
                    })
                    .ToList();

                if (allEdges.Count == 0)
                {
                    var portEdges = dialogueGraphView.graphElements.OfType<Node>()
                        .SelectMany(n => n.outputContainer.Children().OfType<Port>())
                        .SelectMany(p => p.connections)
                        .Distinct()
                        .Where(e => e.input != null && e.output != null
                                 && e.input.node != null && e.output.node != null)
                        .ToList();

                    Debug.Log($"Port-derived edge count: {portEdges.Count}");
                    if (portEdges.Count > 0)
                    {
                        allEdges = portEdges;
                    }
                }

                Debug.Log($"Filtered Edges: {allEdges.Count}");

                foreach (var edge in allEdges)
                {
                    Debug.Log($"Valid edge: input node={edge.input?.node?.name} output node={edge.output?.node?.name}");
                }
            }
            else
            {
                Debug.LogError("There is no DialogGraph");
            }

            List<Node> NodeList = dialogueGraphView.nodes.ToList().Cast<Node>().ToList();

            Debug.Log($"SaveGraph: NodeList count = {NodeList.Count}, mainFunction = {mainFunction}");
            var (nodesData, _) = mainFunction.NodeToDialogNodeData(NodeList);
            allNodes.dialogNodeDatas = nodesData;
            var Start = allNodes.dialogNodeDatas.FirstOrDefault(n => n.nodeTypes == NodeTypes.Start);
            if (Start != null)
            {
                // Searches for a text field named "Description
                var description = Start.fields.Find(f => f.name == "Description");
                if (description != null && !string.IsNullOrEmpty(description.Value))
                {
                    allNodes.desciption = description.Value;
                }
                else
                {
                    Debug.LogWarning("Description field is empty or not found in Start node.");
                }
                if (!string.IsNullOrEmpty(description.Value))
                {
                    allNodes.desciption = description.Value;
                }
                else
                {
                    Debug.LogWarning("Description field is empty or not found in Start node.");
                }
            }
            // Save dialog graph inside the project using a normal Assets folder.
            // StreamingAssets is not a reliable location for ScriptedImporters in this setup.
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }



            string savePath = EditorUtility.SaveFilePanelInProject(
                "Save Dialog Graph",
                "DialogNodes",
                "dialog",
                "Save dialog graph inside the Unity project.",
                "Assets/Resources"
            );

            if (string.IsNullOrEmpty(savePath))
            {
                savePath = "Assets/Resources/DialogNodes.dialog";
                var defaultFolder = System.IO.Path.GetDirectoryName(savePath);
                if (!Directory.Exists(defaultFolder))
                {
                    Directory.CreateDirectory(defaultFolder);
                }
            }

            if (savePath.Contains("/StreamingAssets/") || savePath.StartsWith("Assets/StreamingAssets"))
            {
                Debug.LogWarning("StreamingAssets is not reliable for .dialog ScriptedImporter assets. Saving to Assets/Resources/DialogNodes.dialog instead.");
                savePath = "Assets/Resources/DialogNodes.dialog";
            }

            if (string.IsNullOrEmpty(System.IO.Path.GetExtension(savePath)))
            {
                savePath = System.IO.Path.ChangeExtension(savePath, ".dialog");
            }

            if (savePath.StartsWith(Application.dataPath))
            {
                savePath = "Assets" + savePath.Substring(Application.dataPath.Length);
            }

            if (!savePath.StartsWith("Assets"))
            {
                Debug.LogWarning($"Dialog file path was not inside Assets; using fallback path instead: {savePath}");
                savePath = "Assets/Resources/DialogNodes.dialog";
            }

            WriteData(allNodes, savePath, true);
        }
        #endregion

        public AllNodes GetData(string FilePath)
        {
            if (dialogueGraphView != null)
            {
                dialogueGraphView.ClearAll();
            }
            else
            {
                dialogueGraphView =  DialogueEditor.instance.dialogueGraphView;
            }
            nodes.Clear();
            allNodes = new AllNodes();

            string FullPath = Path.Combine(Application.dataPath, FilePath.Substring(7));
            if (File.Exists(FullPath))
            {
                string JsonContent = File.ReadAllText(FullPath);
                JsonUtility.FromJsonOverwrite(JsonContent, allNodes);
                RebuildConnectedNodes(allNodes);

                return allNodes;
            }

            else
            {
                Debug.LogWarning("there is no saved data");
                return null;
            }
        }
        public AllNodes LoadData(string FilePath)
        {
            loadedNodePath.Invoke(FilePath);
            dialogueGraphView.ClearAll();
            nodes.Clear();
            allNodes = new AllNodes();

            string FullPath = Path.Combine(Application.dataPath, FilePath.Substring(7));
            if (File.Exists(FullPath))
            {
                string JsonContent = File.ReadAllText(FullPath);
                JsonUtility.FromJsonOverwrite(JsonContent, allNodes);
                RebuildConnectedNodes(allNodes);

                List<Node> loadedNodes = mainFunction.DialogNodeDataToNode(allNodes.dialogNodeDatas);

                foreach (var node in loadedNodes)
                {
                    dialogueGraphView.AddElement(node);
                }

                for (int i = 0; i < loadedNodes.Count; i++)
                {
                    if (allNodes.dialogNodeDatas[i].edgeSaveDatas != null && allNodes.dialogNodeDatas[i].edgeSaveDatas.Count > 0)
                    {
                        RestoreEdgesForNode(loadedNodes[i], allNodes.dialogNodeDatas[i].edgeSaveDatas, loadedNodes);
                    }
                }
                currentNodeID = allNodes.ID;

                return allNodes;
            }

            else
            {
                Debug.LogWarning("there is no saved data");
                return null;
            }
        }
        void WriteData(object data, string FilePath, bool SaveInResources = false)
        {
            string JsonFile = JsonUtility.ToJson(data, true);

            string assetPath = FilePath;
            string fullPath = FilePath;
            if (assetPath.StartsWith("Assets"))
            {
                fullPath = Path.Combine(Application.dataPath, assetPath.Substring("Assets".Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            }
            else if (assetPath.StartsWith(Application.dataPath))
            {
                assetPath = "Assets" + assetPath.Substring(Application.dataPath.Length);
            }
            else
            {
                Debug.LogWarning($"Dialog file must be saved inside the Assets folder for runtime inclusion. Falling back to Assets/Resources/DialogNodes.dialog.");
                assetPath = "Assets/Resources/DialogNodes.dialog";
                fullPath = Path.Combine(Application.dataPath, "Resources/DialogNodes.dialog");
            }

            assetPath = assetPath.Replace('\\', '/');

            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                fullPath,
                JsonFile,
                new System.Text.UTF8Encoding(false)
            );

            Debug.Log($"Dialog file written: fullPath={fullPath}, assetPath={assetPath}");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);

            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                Debug.Log($"Saving and reimporting asset importer for {assetPath}");
                importer.SaveAndReimport();
            }
            else
            {
                Debug.LogWarning($"No AssetImporter found for {assetPath}. The file may not be recognized as a Unity asset.");
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"Saved {assetPath}");
        }

        private void RebuildConnectedNodes(AllNodes savedData)
        {
            if (savedData == null || savedData.dialogNodeDatas == null)
                return;

            foreach (var node in savedData.dialogNodeDatas)
            {
                node.ConnectedNodes = new List<DialogNodeData>();
            }

            var nodeMap = savedData.dialogNodeDatas.ToDictionary(n => n.NodeID, n => n);
        }

        private void RestoreEdgesForNode(Node outputNode, List<EdgeSaveData> edgeSaveDatas, List<Node> allLoadedNodes)
        {
            if (edgeSaveDatas == null || edgeSaveDatas.Count == 0)
                return;

            var nodeDict = allLoadedNodes.ToDictionary(n => n.viewDataKey, n => n);

            foreach (var edgeData in edgeSaveDatas)
            {

                if (nodeDict.TryGetValue(edgeData.InputNodeID, out var inputNode))
                {
                    var outputPort = outputNode.mainContainer.Query<Port>().ToList()
                        .FirstOrDefault(p => p.name == edgeData.OutputPortName && p.direction == Direction.Output);

                    var inputPort = inputNode.mainContainer.Query<Port>().ToList()
                        .FirstOrDefault(p => p.name == edgeData.InputPortName && p.direction == Direction.Input);

                    if (outputPort != null && inputPort != null)
                    {
                        var edge = new UnityEditor.Experimental.GraphView.Edge
                        {
                            input = inputPort,
                            output = outputPort
                        };
                        inputPort.Connect(edge);
                        outputPort.Connect(edge);
                        dialogueGraphView.AddElement(edge);
                    }
                }
                else
                {
                    Debug.LogWarning($"InputNode not found: {edgeData.InputNodeID}");
                }
            }
        }

    }
}
#endif
