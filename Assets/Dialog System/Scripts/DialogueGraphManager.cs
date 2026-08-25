using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogSystem
{
    [AddComponentMenu("")]
    public class DialogueGraphManager : MonoBehaviour
    {
        public static DialogueGraphManager instance { get; private set; }
        private AllNodes allNodes;
        private bool IsPlaying;
        public bool IsDialogPlaying => IsPlaying;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            instance = null;
            if (FindAnyObjectByType<DialogueGraphManager>() == null)
            {
                GameObject DGM = new GameObject("DialogueGraphManager");
                instance = DGM.AddComponent<DialogueGraphManager>();
                UnityEngine.Object.DontDestroyOnLoad(DGM);
            }
        }
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public static void PlayNode(DialogFile dialogFile, DialogElement dialogElement)
        {
            _ = DialogueGraphManager.instance.Play(dialogFile, dialogElement);
        }
        public async Task Play(DialogFile dialogFile, DialogElement dialogElement)
        {
            if (IsPlaying)
                return;
            if (dialogFile == null)
            {
                Debug.LogWarning("DialogFile is null.");
                return;
            }

            allNodes = dialogFile.GetDialogData();
            if (allNodes == null || allNodes.dialogNodeDatas == null || allNodes.dialogNodeDatas.Count == 0)
            {
                Debug.LogWarning("There is no saved data in DialogFile: " + dialogFile.name);
                return;
            }

            var nodeLookup = dialogFile.NodeLookup;
            DialogNodeData startNode = allNodes.dialogNodeDatas.Find(node => node.nodeTypes == NodeTypes.Start);
            if (startNode == null)
            {
                Debug.LogError($"[Dialogue] No start node found in graph '{dialogFile.name}'!"); return;
            }

            if (startNode.edgeSaveDatas != null && startNode.edgeSaveDatas.Count > 0 && startNode.edgeSaveDatas[0] != null)
            {
                IsPlaying = true;
                dialogElement.gameObject.SetActive(true);
                try
                {
                    string firstNodeId = startNode.edgeSaveDatas[0].InputNodeID;
                    if (nodeLookup.TryGetValue(firstNodeId, out var firstNode))
                    {
                        await PlayNode(firstNode, dialogElement, nodeLookup);
                    }
                    else
                    {
                        Debug.LogWarning($"Start node points to unknown node ID '{firstNodeId}' in DialogFile: {dialogFile.name}");
                    }
                }
                finally
                {
                    IsPlaying = false;
                }
            }
            else
            {
                Debug.LogWarning("There is no connected node to start node cant play: " + dialogFile.name);
            }
        }

        async Task PlayNode(DialogNodeData currentNode, DialogElement dialogElement, Dictionary<string, DialogNodeData> nodeLookup)
        {
            while (currentNode != null)
            {
                string nextNodeId = null;
                switch (currentNode.nodeTypes)
                {
                    case NodeTypes.Dialog:
                        var oneSidedDialog = new OneSidedDialogNode();
                        await oneSidedDialog.PlayNode(currentNode, dialogElement);
                        break;
                    case NodeTypes.MultiDialog:
                        var multipleChoiceNode = new MultipleChoiceNode();
                        nextNodeId = await multipleChoiceNode.PlayNode(currentNode, dialogElement);
                        break;
                    case NodeTypes.Audio:
                        var audioNode = new AudioNode();
                        audioNode.PlayNode(currentNode);
                        break;
                    case NodeTypes.Action:
                        var actionNode = new ActionNode();
                        actionNode.PlayNode(currentNode, dialogElement);
                        break;
                    case NodeTypes.Wait:
                        var waitNode = new WaitNode();
                        await waitNode.PlayNode(currentNode);
                        break;
                    case NodeTypes.End:
                        var endNode = new EndNode();
                        endNode.PlayNode(dialogElement);
                        currentNode = null;
                        IsPlaying = false;
                        return;
                    case NodeTypes.Start:
                        var startNode = new StartNode();
                        startNode.PlayNode(dialogElement);
                        break;
                }

                if (string.IsNullOrEmpty(nextNodeId) && currentNode.edgeSaveDatas != null && currentNode.edgeSaveDatas.Count > 0)
                {
                    nextNodeId = currentNode.edgeSaveDatas[0].InputNodeID;
                }

                if (!string.IsNullOrEmpty(nextNodeId) && nodeLookup.TryGetValue(nextNodeId, out var nextNode))
                {
                    currentNode = nextNode;
                }
                else
                {
                    currentNode = null;
                }

                ClearDialogElement(dialogElement);
            }
            IsPlaying = false;
        }
        private void ClearDialogElement(DialogElement dialogElement)
        {
            if (!Application.isPlaying)
                return;
            dialogElement.narratorText.text = null;
            dialogElement.narratorText.color = Color.black;
            for (int i = 1; i < dialogElement.ChoiceParent.childCount; i++)
            {
                Destroy(dialogElement.ChoiceParent.GetChild(i).gameObject);
            }
        }
    }
}
namespace DialogSystem
{
    public enum NodeTypes
    {
        Start,
        Dialog,
        MultiDialog,
        Audio,
        Action,
        Wait,
        End,
        nothing
    }
}
namespace DialogSystem
{
    public enum ValueTypes
    {
        String,
        Float,
        Color,
        Vector3,
        List,
        nothing
    }
}