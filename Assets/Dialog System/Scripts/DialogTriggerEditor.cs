#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DialogSystem
{
    [CustomEditor(typeof(DialogTrigger))]
    public class DialogTriggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DialogTrigger script = (DialogTrigger)target;

            // Draw DialogFile
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DialogFile"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxTrigger"));

            if (script.triggerOption == TriggerOption.Interaction_Distance_Vector3)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetVector3"));
            }

            if (script.triggerOption == TriggerOption.Interaction_Distance_Transform)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetTransform"));
            }
            // UseRadius only showses when needed
            if (script.triggerOption == TriggerOption.Interaction_Distance_Vector3 || script.triggerOption == TriggerOption.Interaction_Distance_Transform || script.triggerOption == TriggerOption.Events_OnEnter || script.triggerOption == TriggerOption.Events_OnExit)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Radius"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerOption"));

            if (script.triggerOption == TriggerOption.Interaction_KeyPress)
            {
#if ENABLE_INPUT_SYSTEM
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetetAction"));
#else
            EditorGUILayout.HelpBox("Input System package is missing. Install 'com.unity.inputsystem' or use another trigger.", MessageType.Info);
#endif
            }

            if (script.triggerOption == TriggerOption.Events_OnEnter)
            {
                bool has2DColliderTrigger = script.GetComponent<Collider2D>() != null && script.GetComponent<Collider2D>().isTrigger;
                bool has3DColliderTrigger = script.GetComponent<Collider>() != null && script.GetComponent<Collider>().isTrigger;
                if (!has2DColliderTrigger && !has3DColliderTrigger)
                {
                    EditorGUILayout.HelpBox("A Collider with 'Is Trigger' enabled is required for Events_OnEnter trigger option.", MessageType.Warning);
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogElement"));
            if (script.dialogElement == null)
            {
                EditorGUILayout.HelpBox("No DialogElement selected", MessageType.Warning);
            }


            if (script.triggerOption == TriggerOption.Events_OnExit)
            {
                bool has2DColliderTrigger = script.GetComponent<Collider2D>() != null && script.GetComponent<Collider2D>().isTrigger;
                bool has3DColliderTrigger = script.GetComponent<Collider>() != null && script.GetComponent<Collider>().isTrigger;
                if (!has2DColliderTrigger && !has3DColliderTrigger)
                {
                    EditorGUILayout.HelpBox("A Collider with 'Is Trigger' enabled is required for Events_OnExit trigger option.", MessageType.Warning);
                }
            }

            if (script.triggerOption == TriggerOption.Events_OnEnter_Tag || script.triggerOption == TriggerOption.Events_OnExit_Tag)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ObjectTag"));

                if (string.IsNullOrEmpty(script.ObjectTag))
                {
                    EditorGUILayout.HelpBox("A Tag Name us needed", MessageType.Warning);
                }

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
