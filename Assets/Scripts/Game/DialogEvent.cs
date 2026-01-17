using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
        [InspectorName("Tilemap")] TilemapCollision,

    }

    public TriggerType SelectedTriggerType;
    [Tooltip("What happens when the trigger is triggered")]
    public UnityEvent Action;

    [HideInInspector] public string CollisionTag = "Player";
    [HideInInspector] public Transform Object;
    [HideInInspector] public Vector3 Vector3Position;
    [HideInInspector] public float Distance = 1.0f;
    [HideInInspector] public Tile Tile;

    private bool Performed = false;
    private bool InCollision = false;
    private Tilemap tilemap;
    void OnEnable()
    {
        Performed = false;
        if (SelectedTriggerType == TriggerType.SceneStart)
        {
            Action.Invoke();
            Debug.Log("Executed Trigger");
        }
        if (this.gameObject.GetComponent<Tilemap>())
        {
            tilemap = this.gameObject.GetComponent<Tilemap>();
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
                        float DistanceTransform = Vector3.Distance(transform.position, Object.transform.position);
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
                        float DistanceTransform = Vector3.Distance(transform.position, Vector3Position);
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
                case TriggerType.TilemapCollision:
                    if (tilemap != null)
                    {

                    }
                    break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (SelectedTriggerType == TriggerType.PositionTransformDistance)
        {
            Gizmos.color = Color.skyBlue;
            Gizmos.DrawLine(transform.position, Object.transform.position);
        }
        if (SelectedTriggerType == TriggerType.PositionVector3Distance)
        {
            Gizmos.color = Color.skyBlue;
            Gizmos.DrawLine(transform.position, Vector3Position);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (SelectedTriggerType == TriggerType.OnCollisionEnter2d)
        {
            Action.Invoke();
            return;
        }
        if (SelectedTriggerType != TriggerType.OnCollisionEnter2d)
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            contactPoint.y -= 0.01f;
            Vector3Int cellPosition = tilemap.WorldToCell(contactPoint);
            if (tilemap.GetTile(cellPosition) == Tile)
            {
                Action.Invoke();
                return;
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        InCollision = false;
        if (SelectedTriggerType == TriggerType.OnCollisionExit2d)
        {
            Action.Invoke();
            return;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (SelectedTriggerType == TriggerType.OnCollisionStay2d && !InCollision)
        {
            InCollision = true;
            Action.Invoke();
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