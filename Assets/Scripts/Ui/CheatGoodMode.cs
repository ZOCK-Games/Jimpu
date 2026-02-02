using Unity.Services.Lobbies.Models;
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
    }
    void Update()
    {
        if (IsActive)
        {
            playerControll.playerCollider.isTrigger = true;
            if (Input.GetKey(KeyCode.W))
            {
                playerControll.rb.AddForceY(5);
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerControll.rb.AddForceY(-5);
            }
        }
        else
        {
            playerControll.playerCollider.isTrigger = false;
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
        playerControll.rb.gravityScale = -0.01f;
        IsActive = true;
    }
    void Deactivate()
    {
        playerControll.rb.gravityScale = NormalGravity;
        IsActive = false;
    }
}
