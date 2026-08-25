using System;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogSystem
{
    public class AudioNode
    {
#if UNITY_EDITOR
        public void SetDialogNode(Node node, MainFunction mainFunction, VisualFunctions visualFunctions, DialogNodeData data = null)
        {
            mainFunction.RemoveExtensionElements(node);
            mainFunction.RemovePorts(node, Direction.Input);
            mainFunction.RemovePorts(node, Direction.Output);
            node.name = NodeTypes.Audio.ToString();
            visualFunctions.AddPort(node, Direction.Input, "In");
            visualFunctions.AddPort(node, Direction.Output, "Out");

            string position = data?.fields.Find(f => f.type == ValueTypes.Vector3)?.Value ?? "(0,0,0)";
            string volume = data?.fields.Find(f => f.name == "Volume")?.Value ?? "1";
            AudioClip clip = null;
            if (data != null)
            {
                var objectData = data.objectSaveDatas.Find(o => o.name == "AudioField");
                if (objectData != null)
                {
                    // Note: We can't easily load the asset here without AssetDatabase, 
                    // but AddObjectField will handle it if we pass null and it's later set by DialogNodeDataToNode
                }
            }

            visualFunctions.AddText(node, "Position", position, ValueTypes.Vector3, "The position where the audio is being played");
            visualFunctions.AddText(node, "Volume", volume, ValueTypes.Float, "The Volume of the audio clip", "Volume");
            visualFunctions.AddObjectField(node, "AudioField", typeof(AudioClip));
        }
#endif

        public void PlayNode(DialogNodeData node)
        {
            try
            {
                if (node.nodeTypes != NodeTypes.Audio)
                {
                    return;
                }
                if (node.objectSaveDatas == null || node.objectSaveDatas.Count == 0)
                {
                    Debug.LogError("Audio node has no object fields.");
                    return;
                }

                ObjectSaveData audioField =
                    node.objectSaveDatas.Find(o => o.name == "AudioField")
                    ?? node.objectSaveDatas[0];

                string path = audioField.path;

                int resourcesIndex = path.IndexOf("/Resources/");

                if (resourcesIndex == -1)
                {
                    Debug.LogError($"Audio file is not inside a Resources folder: {path}");
                    return;
                }

                path = path.Substring(resourcesIndex + "/Resources/".Length);
                path = Path.ChangeExtension(path, null);

                AudioClip audioClip = Resources.Load<AudioClip>(path);

                if (audioClip == null)
                {
                    Debug.LogError($"Could not load AudioClip from Resources: {path}");
                }

                Vector3 audioPosition = Vector3.zero;
                var positionField = node.fields.Find(f => f.type == ValueTypes.Vector3);
                if (positionField != null)
                {
                    string rawPosition = positionField.Value
                        .Replace("(", "")
                        .Replace(")", "")
                        .Trim();
                    var parts = rawPosition.Split(",");
                    if (parts.Length == 3
                        && float.TryParse(parts[0].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float x)
                        && float.TryParse(parts[1].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float y)
                        && float.TryParse(parts[2].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float z))
                    {
                        audioPosition = new Vector3(x, y, z);
                    }
                }

                float volume = 1f;
                var volumeField = node.fields.Find(f => f.name == "Volume");
                if (volumeField != null
                    && !float.TryParse(volumeField.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out volume))
                {
                    volume = 1f;
                }

                AudioSource.PlayClipAtPoint(audioClip, audioPosition, volume);
                Debug.Log("Audio played: " + audioClip.name);
            }
            catch (Exception e)
            {
                Debug.LogError("AudioNode Error: " + e);
            }
        }
    }
}