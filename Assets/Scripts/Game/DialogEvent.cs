using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Icon("Assets/Gizmos/DialogTrigger")]
public class DialogEvent : MonoBehaviour
{
    public enum TriggerType
    {
        /// <summary>
        /// Collision settings
        /// </summary>
        [InspectorName("Scene/Start")] SceneStart,
        [InspectorName("Scene/SceneExit")] SceneExit,
        [InspectorName("Collision/Enter 2D")] OnCollisionEnter2d,
        [InspectorName("Collision/Exit 2D")] OnCollisionExit2d,
        [InspectorName("Collision/Stay 2D")] OnCollisionStay2d,
        [InspectorName("Position/Vector3/Distance")] PositionVector3Distance,
        [InspectorName("Position/Transform/Distance")] PositionTransformDistance,

    }
    public TriggerType SelectedTriggerType;
    [Tooltip("What happens when the trigger is triggered")]
    public UnityEvent Action;

    [HideInInspector] public string CollisionTag = "Player";
    [HideInInspector] public Transform Object;
    [HideInInspector] public Vector3 Vector3Position;
    [HideInInspector] public float Distance = 1.0f;
    private bool Performed = false;
    void OnEnable()
    {
        Performed = false;
        if (SelectedTriggerType == TriggerType.SceneStart)
        {
            Action.Invoke();
            Debug.Log("Executed Trigger");
        }
    }
    void OnDisable()
    {
        if (SelectedTriggerType == TriggerType.SceneExit)
        {
            Action.Invoke();
            Debug.Log("Executed Trigger");
        }
    }
    void Update()
    {
        if (Action.GetPersistentEventCount() > 0)
        {
            switch (SelectedTriggerType)
            {
                case TriggerType.PositionTransformDistance:

                    if (Object != null)
                    {
                        float DistanceTransform = Vector2.Distance(transform.position, Object.transform.position);
                        if (DistanceTransform <= Distance && !Performed)
                        {
                            Action.Invoke();
                            Debug.Log("Executed Trigger Pos");
                            Performed = true;
                        }
                        if (DistanceTransform > Distance)
                        {
                            Performed = false;
                        }

                    }
                    break;
                case TriggerType.PositionVector3Distance:
                    if (Object != null)
                    {
                        float DistanceTransform = Vector2.Distance(transform.position, Vector3Position);
                        if (DistanceTransform <= Distance && !Performed)
                        {
                            Action.Invoke();
                            Debug.Log("Executed Trigger Pos");
                            Performed = true;
                        }
                        if (DistanceTransform > Distance)
                        {
                            Performed = false;
                        }
                    }
                    break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (SelectedTriggerType == TriggerType.PositionTransformDistance || SelectedTriggerType == TriggerType.PositionVector3Distance)
        {
            Gizmos.color = Color.skyBlue;
            Gizmos.DrawLine(transform.position, Object.transform.position);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (SelectedTriggerType != TriggerType.OnCollisionEnter2d)
        {
            return;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (SelectedTriggerType != TriggerType.OnCollisionExit2d)
        {
            return;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (SelectedTriggerType != TriggerType.OnCollisionStay2d)
        {
            return;
        }
    }
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(DialogEvent))]
public class UniversalTriggerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DialogEvent script = (DialogEvent)target;

        // Zeichne das Standard-Dropdown
        script.SelectedTriggerType = (DialogEvent.TriggerType)UnityEditor.EditorGUILayout.EnumPopup("Modus", script.SelectedTriggerType);

        UnityEditor.EditorGUILayout.Space();

        if (script.SelectedTriggerType == DialogEvent.TriggerType.OnCollisionEnter2d ||
         script.SelectedTriggerType == DialogEvent.TriggerType.OnCollisionExit2d ||
         script.SelectedTriggerType == DialogEvent.TriggerType.OnCollisionStay2d)
        {
            script.CollisionTag = UnityEditor.EditorGUILayout.TagField("Tag", script.CollisionTag);
        }
        else if (script.SelectedTriggerType == DialogEvent.TriggerType.PositionTransformDistance)
        {
            script.Object = (Transform)UnityEditor.EditorGUILayout.ObjectField("Object", script.Object, typeof(Transform), true);
            script.Distance = UnityEditor.EditorGUILayout.FloatField("Distance", script.Distance);
        }
        else if (script.SelectedTriggerType == DialogEvent.TriggerType.PositionVector3Distance)
        {
            script.Vector3Position = UnityEditor.EditorGUILayout.Vector3Field("Object", script.Vector3Position);
            script.Distance = UnityEditor.EditorGUILayout.FloatField("Distance", script.Distance);
        }
        UnityEditor.EditorGUILayout.Space();

        UnityEditor.SerializedProperty aktionProp = serializedObject.FindProperty("Action");

        serializedObject.Update();
        UnityEditor.EditorGUILayout.PropertyField(aktionProp);
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            UnityEditor.EditorUtility.SetDirty(script);
        }
    }
}
#endif