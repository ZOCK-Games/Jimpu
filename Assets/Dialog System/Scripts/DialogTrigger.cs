using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DialogSystem
{
    public enum TriggerOption
    {
        Interaction_KeyPress,
        Interaction_Distance_Vector3,
        Interaction_Distance_Transform,
        Events_OnEnter,
        Events_OnEnter_Tag,
        Events_OnExit,
        Events_OnExit_Tag,
        On_Event
    }

    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private DialogFile DialogFile;
        public Vector3 TargetVector3;
        public string ObjectTag;
        public Transform TargetTransform;
        public float Radius = 1f;
        public int MaxTrigger = 1;
        private int triggers = 0;
        private bool hasTriggered = false;
        [Header("Events")]
        [SerializeField] private UnityEvent OnDialogTriggered;
        public TriggerOption triggerOption = TriggerOption.Events_OnEnter;
#if ENABLE_INPUT_SYSTEM
        public InputAction TargetetAction; // new input system action for key press interaction
#endif
        public DialogElement dialogElement;

        void Start()
        {
            hasTriggered = false;
#if ENABLE_INPUT_SYSTEM
            if (TargetetAction != null)
            {
                TargetetAction.performed += _ =>
                {
                    if (triggerOption == TriggerOption.Interaction_KeyPress && !hasTriggered)
                    {
                        TriggerDialog();
                    }
                };
            }
#endif
        }

        void OnEnable()
        {
#if ENABLE_INPUT_SYSTEM
            if (TargetetAction != null)
            {
                TargetetAction.Enable();
            }
#endif
        }

        void OnDisable()
        {
#if ENABLE_INPUT_SYSTEM
            if (TargetetAction != null)
            {
                TargetetAction.Disable();
            }
#endif
        }

        void Update()
        {
            if ((triggerOption == TriggerOption.Interaction_Distance_Vector3 || triggerOption == TriggerOption.Interaction_Distance_Transform) && !hasTriggered)
            {
                Vector3 targetPosition = triggerOption == TriggerOption.Interaction_Distance_Vector3
                    ? TargetVector3
                    : (TargetTransform != null ? TargetTransform.position : transform.position);
                if (Vector3.Distance(transform.position, targetPosition) <= Radius)
                {
                    TriggerDialog();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (triggerOption == TriggerOption.Events_OnEnter && !hasTriggered)
            {
                TriggerDialog();
            }

            /// Used when a tag is needed 
            if (triggerOption == TriggerOption.Events_OnEnter_Tag && other.CompareTag(ObjectTag) && !hasTriggered)
            {
                TriggerDialog();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerOption == TriggerOption.Events_OnExit && !hasTriggered)
            {
                TriggerDialog();
            }

            // Used when a tag is needed 
            if (triggerOption == TriggerOption.Events_OnExit_Tag && other.CompareTag(ObjectTag) && !hasTriggered)
            {
                TriggerDialog();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggerOption == TriggerOption.Events_OnEnter && !hasTriggered)
            {
                TriggerDialog();
            }

            // Used when a tag is needed 
            if (triggerOption == TriggerOption.Events_OnEnter_Tag && !hasTriggered && other.CompareTag(ObjectTag))
            {
                TriggerDialog();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (triggerOption == TriggerOption.Events_OnExit && !hasTriggered)
            {
                TriggerDialog();
            }

            // Used when a tag is needed 
            if (triggerOption == TriggerOption.Events_OnExit_Tag && !hasTriggered && other.CompareTag(ObjectTag))
            {
                TriggerDialog();
            }
        }

        private void TriggerDialog()
        {
            Debug.Log("Dialog Triggered: " + gameObject.name);

            if (DialogFile != null && dialogElement != null)
            {
                if (DialogueGraphManager.instance == null || DialogueGraphManager.instance.IsDialogPlaying)
                {
                    return;
                }

                triggers += 1;
                _ = DialogueGraphManager.instance.Play(DialogFile, dialogElement);
            }
            else
            {
                Debug.LogWarning("No DialogFile assigned.");
            }
            if (MaxTrigger != -1 && triggers >= MaxTrigger)
            {
                hasTriggered = true;
            }
        }
    }
}