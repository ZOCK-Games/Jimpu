using UnityEngine;
using UnityEngine.UI;

public class CheatGoodMode : MonoBehaviour
{
    public Button GoodButton;
    public PlayerControll playerControll;
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
        NormalGravity = playerControll.rb.gravityScale;
        GoodButton.onClick.AddListener(ToggleMode);
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    void Update()
    {
        if (IsActive);
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0);
            playerControll.transform.position += move * 5 * Time.deltaTime;
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
        playerControll.playerCollider.isTrigger = true;
        playerControll.CanMove = false;
        playerControll.rb.bodyType = RigidbodyType2D.Kinematic;
        IsActive = true;
    }
    void Deactivate()
    {
        playerControll.CanMove = true;
        playerControll.rb.bodyType = RigidbodyType2D.Dynamic;
        playerControll.playerCollider.isTrigger = false;
        IsActive = false;
    }
}
