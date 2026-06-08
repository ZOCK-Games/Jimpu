using UnityEngine;
using UnityEngine.UI;

public class CheatGoodMode : MonoBehaviour
{
    public Button GoodButton;
    private bool IsActive;
    private float NormalGravity;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        IsActive = false;
        NormalGravity = playerControl.instance.rb.gravityScale;
        GoodButton.onClick.AddListener(ToggleMode);
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    void Update()
    {
        if (IsActive);
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0);
            playerControl.instance.transform.position += move * 5 * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void ToggleMode()
    {
        if (IsActive)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }
    void Activate()
    {
        playerControl.instance.playerCollider.isTrigger = true;
        playerControl.instance.CanMove = false;
        playerControl.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        IsActive = true;
    }
    void Deactivate()
    {
        playerControl.instance.CanMove = true;
        playerControl.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        playerControl.instance.playerCollider.isTrigger = false;
        IsActive = false;
    }
}
