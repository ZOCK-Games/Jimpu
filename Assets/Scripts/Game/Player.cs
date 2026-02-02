using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour, IDataPersitence

{
    public GameObject Player;
    public Animator PlayerAniamtor;
    public GameObject CollidersGameObjekt; // the TilemapContainer
    public float MoveSpeed = 3.5f;
    public float Jump_speed = 350;
    public bool CanMove = true;
    public int run_speed = 5;
    public Rigidbody2D rb;
    public List<Sprite> SkinSprite;
    private int SkinIndex;
    public GameObject BodyPartsContainer;

    public GarderobenScribt garderobenScribt;
    [Header("Can Player perform this inputs?")]
    public bool MovePlayerR;
    public bool MovePlayerL;
    public bool MovePlayerUP;
    public List<TilemapCollider2D> Grounds;
    [SerializeField] private List<Skins> PlayerSkins;
    public bool PlayerIsTouchingGround; // if the player is touching a ground tile collider
    [Header("Player Hold On to settings")]
    public GameObject PlayerInfoInteractionKey;

    private bool IsHoldingOn;
    public PolygonCollider2D playerCollider;
    public BoxCollider2D JumpCollider;
    private Rigidbody2D PlayerRb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    public HealthManagerPlayer healthManagerPlayer;
    public EnergyManager energyManager;
    public VibrateControllerManager vibrateController;
    [Header("Player Attacks")]
    public string CurrentAttack;
    private bool CanAttack;
    public float AttackRollStrength;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    private void Start()
    {
        playerCollider = Player.GetComponent<PolygonCollider2D>();
        PlayerRb = Player.GetComponent<Rigidbody2D>();

        PlayerInfoInteractionKey.SetActive(false);
        PlayerIsTouchingGround = false;
        CanAttack = true;
        UnityEngine.Debug.Log("Tutorial Game Has Startet");
        rb = Player.GetComponent<Rigidbody2D>();
        PlayerAniamtor.SetBool("Walk", false);

        for (int i = 0; i < CollidersGameObjekt.transform.childCount - 1; i++)
        {
            if (CollidersGameObjekt.transform.GetChild(i).gameObject.CompareTag("Ground"))
            {
                Grounds.Add(CollidersGameObjekt.transform.GetChild(i).gameObject.GetComponent<TilemapCollider2D>());
                Debug.LogError("Found Ground: " + CollidersGameObjekt.transform.GetChild(i).gameObject.name);
            }
            else if (Grounds == null)
            {
                Debug.LogWarning("NoGroundFound");
            }
            else
            {
                Debug.Log("An unexpected error accrued");
            }
        }
        CheckSkin();
    }


    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Vector3 move = new Vector3(moveInput.x, 0, 0);
            transform.Translate(move * MoveSpeed * Time.deltaTime);
        }


        /*/if (PlayerHealth == 0)              // Aktiviert Den Dead Screen
        SceneManager.LoadScene("Death");*/

        PlayerIsTouchingGround = false;

        foreach (var ground in Grounds)
        {
            if (ground != null && JumpCollider.IsTouching(ground))
            {
                PlayerIsTouchingGround = true;
                break;
            }
        }

        if (!inputActions.Player.Interact.IsPressed() && IsHoldingOn)
        {
            IsHoldingOn = false;
            CanMove = true;
            PlayerRb.constraints = RigidbodyConstraints2D.None;
            PlayerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(new Vector2(0, Jump_speed / 2));
            Debug.Log("Player is no longer holding on");

        }
        if (inputActions.Player.Jump.WasPerformedThisFrame() && PlayerIsTouchingGround && CanMove || MovePlayerUP && CanMove)
        {
            rb.AddForce(new Vector2(0, Jump_speed));
            PlayerAniamtor.SetTrigger("Jump");
        }

        ////
        /// Dodge Roll
        /// 

        if (inputActions.Player.DodgeRoll.WasPerformedThisFrame() && CanAttack && energyManager.EnergyAmount >= 25)
        {
            Debug.Log("Attack");
            float Direction = inputActions.Player.Move.ReadValue<Vector2>().x;
            CurrentAttack = "DodgeRoll";
            if (Direction > 0)
            {
                Debug.Log("Attack R");
                PlayerAniamtor.Play("PlayerDogeRoll");
                moveInput.x += AttackRollStrength;
                vibrateController.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            else
            {
                Debug.Log("Attack L");
                PlayerAniamtor.Play("PlayerDogeRoll");
                moveInput.x -= AttackRollStrength;
                vibrateController.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            StartCoroutine(AttackWait(1.5f,0.18f));
        }
        ////
        /// Hand Bumm
        /// 

        if (inputActions.Player.Attack.WasPerformedThisFrame() && CanAttack)
        {
            CurrentAttack = "HandBumm";
            PlayerAniamtor.Play("HandBumm");
            StartCoroutine(AttackWait(0.8f,0.1f));
        }
    }
    IEnumerator AttackWait(float ResetTime, float DelayTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(ResetTime);
        CurrentAttack = null;
        yield return new WaitForSeconds(DelayTime);
        CanAttack = true;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            Vector3 myPosition = transform.position;
            Vector3 otherPosition = collider.transform.position;
            Vector3 collisionDirection = myPosition - otherPosition;
            if (collisionDirection.x > 0)
            {
                StartCoroutine(HoldOnToWall("Right", collisionDirection));
            }
            if (collisionDirection.x < 0)
            {
                StartCoroutine(HoldOnToWall("Left", collisionDirection));
            }
        }
    }
    IEnumerator HoldOnToWall(string Direction, Vector3 Pos)
    {
        PlayerInfoInteractionKey.SetActive(true);
        float ElapsedTime = 0f;
        while (ElapsedTime < 0.8f)
        {
            if (inputActions.Player.Interact.WasPerformedThisFrame() && !PlayerIsTouchingGround)
            {
                Debug.Log($"Holding On to: {Pos}");
                Debug.DrawRay(Pos, Pos * 2, Color.red);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                CanMove = false;
                IsHoldingOn = true;
                break;
            }
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        PlayerInfoInteractionKey.SetActive(false);
    }
    public void CheckSkin()
    {
        BodyPartsContainer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Head;
        BodyPartsContainer.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].Body;
        BodyPartsContainer.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftArm;
        BodyPartsContainer.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightArm;
        BodyPartsContainer.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].LeftLeg;
        BodyPartsContainer.transform.GetChild(5).GetComponent<SpriteRenderer>().sprite = PlayerSkins[SkinIndex].RightLeg;
    }


    public void LoadData(SaveManager manager)
    {

        Player.transform.localPosition = manager.playerDataSO.PlayerPosition;
        SkinIndex = manager.playerDataSO.SkinIndex;
        CheckSkin();
        //Player.GetComponent<SpriteRenderer>().sprite = SkinSprite[SkinIndex];
        if (UnityEngine.ColorUtility.TryParseHtmlString("#" + manager.playerDataSO.colorHex, out Color colorHex))
            for (int i = 0; i < BodyPartsContainer.transform.childCount; i++)
            {
                BodyPartsContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorHex;
            }
        healthManagerPlayer.PlayerHealth = manager.playerDataSO.Health;

    }
    public void SaveData(SaveManager manager) 
    {
        manager.playerDataSO.PlayerPosition = this.Player.transform.localPosition;
        manager.playerDataSO.Health = healthManagerPlayer.PlayerHealth;
        manager.playerDataSO.SkinIndex = SkinIndex;
    }
}
