using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour, IDataPersitence

{
    public static PlayerControll instance { get; private set; }
    [Header("Player Settings")]
    [Space(0.5f)]
    public float MoveSpeed = 3.5f;
    public float JumpSpeed = 350;
    public int RunSpeed = 5;
    [Header("The Setting for the player to get fall damage")]
    public float FallDamage = 6;
    public LayerMask FallLayerMask;
    public float AttackRollStrength;
    /// <summary>
    /// Bool for checking if
    /// Animations Can Play 
    /// Used for Blocking All Animations 
    /// </summary>
    public bool AnimationsCanPlay;
    [Header("The Current Attack as a string for checking the current attack")]
    public string CurrentAttack;
    /// <summary>
    /// The Damage that the 
    /// Attack Roll dose
    /// </summary>
    private int SkinIndex; // The Current Skin number
    public bool PlayerIsTouchingGround; // if the player is touching a ground tile collider
    private bool IsPlayerAttacking;
    public bool CanMove = true;
    public bool CanAttack;
    private bool IsCheckingGround;
    [Space(1)]
    [Header("Player Components")]
    [Space(0.5f)]
    [Tooltip("The Player Game Object")]
    public GameObject Player;
    [Tooltip("The Player Animator for body parts")]
    public Animator PlayerAniamtor;
    [Tooltip("The Player Rigidbody2D")]
    public Rigidbody2D rb;
    public PolygonCollider2D playerCollider;
    public GameObject CollidersGameObjekt;
    public BoxCollider2D JumpCollider;
    public List<TilemapCollider2D> Grounds;
    public GameObject BodyPartsContainer;
    public List<Sprite> SkinSprite;
    [SerializeField] private List<Skins> PlayerSkins;
    public GameObject PlayerInfoInteractionKey;
    private bool IsHoldingOn;
    private Vector2 moveInput;
    [Space(1)]
    [Header("Scripts")]
    [Space(0.5f)]
    private InputSystem_Actions inputActions;

    public HealthManagerPlayer healthManagerPlayer;
    public EnergyManager energyManager;
    public VibrateControllerManager vibrateController;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    { 
        inputActions.Player.Enable(); // Activates Player Control for him self
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
        rb = Player.GetComponent<Rigidbody2D>();

        PlayerInfoInteractionKey.SetActive(false);
        PlayerIsTouchingGround = false;
        CanAttack = true;
        IsCheckingGround = false;
        PlayerAniamtor.SetBool("Walk", false);

        for (int i = 0; i < CollidersGameObjekt.transform.childCount - 1; i++)
        {
            if (CollidersGameObjekt.transform.GetChild(i).gameObject.CompareTag("Ground"))
            {
                Grounds.Add(CollidersGameObjekt.transform.GetChild(i).gameObject.GetComponent<TilemapCollider2D>());
                Debug.Log("Found Ground: " + CollidersGameObjekt.transform.GetChild(i).gameObject.name);
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
    protected void Update()
    {
        if (CanMove)
        {
            if (Mathf.Abs(moveInput.x) > 0.1f && !IsPlayerAttacking) // Checks if The Player is moving or i the Player is Attacking
            {
                if (!AudioManager.instance.isPlaying("Walk")) // Checks if the sound is already playing
                {
                    AudioManager.instance.PlayAudio("Walk", transform, true, 1);
                }
                PlayerAniamtor.SetBool("Walk", true);
            }
            else
            {
                PlayerAniamtor.SetBool("Walk", false);
            }
            Vector3 move = new Vector3(moveInput.x, 0, 0);
            transform.Translate(move * MoveSpeed * Time.deltaTime); // Sets the position where the player wants to move to
        }
        else
        {
            if (AudioManager.instance.isPlaying("Walk"))
            {
                AudioManager.instance.StopAudio("Walk");
            }
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
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(new Vector2(0, JumpSpeed / 2));
            Debug.Log("Player is no longer holding on");

        }
        if (inputActions.Player.Jump.WasPerformedThisFrame() && PlayerIsTouchingGround && CanMove)
        {
            IsPlayerAttacking = true;
            AudioManager.instance.PlayAudio("jump", transform);
            rb.AddForce(new Vector2(0, JumpSpeed));
            if (AnimationsCanPlay)
            {
                PlayerAniamtor.SetTrigger("Jump");
            }
        }
        /// 
        /// Falling System
        /// 
        if (!PlayerIsTouchingGround && !IsCheckingGround)
        {
            IsCheckingGround = true;
            StartCoroutine(CheckFall());
        }


        ////
        /// Dodge Roll
        /// 

        if (inputActions.Player.DodgeRoll.WasPerformedThisFrame() && CanAttack && energyManager.EnergyAmount >= 25)
        {
            IsPlayerAttacking = true;
            Debug.Log("Attack");
            float Direction = inputActions.Player.Move.ReadValue<Vector2>().x;
            CurrentAttack = "DodgeRoll";
            AudioManager.instance.PlayAudio("Player_Punch", transform, true, 1);
            if (Direction > 0)
            {
                Debug.Log("Attack R");
                if (AnimationsCanPlay)
                {
                    PlayerAniamtor.Play("PlayerDogeRoll");
                }
                moveInput.x += AttackRollStrength;
                vibrateController.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            else
            {
                Debug.Log("Attack L");
                if (AnimationsCanPlay)
                {
                    PlayerAniamtor.Play("PlayerDogeRoll");
                }
                moveInput.x -= AttackRollStrength;
                vibrateController.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            StartCoroutine(AttackWait(1.5f, 0.18f));
        }
        ////
        /// Hand Bumm
        /// 

        if (inputActions.Player.Attack.WasPerformedThisFrame() && CanAttack)
        {
            IsPlayerAttacking = true;
            AudioManager.instance.PlayAudio("Player_Punch", transform, true, 1);
            CurrentAttack = "HandBumm";
            if (AnimationsCanPlay)
            {
                PlayerAniamtor.Play("HandBumm");
            }
            StartCoroutine(AttackWait(0.8f, 0.1f));
        }
    }
    IEnumerator AttackWait(float ResetTime, float DelayTime)
    {
        CanAttack = false;
        yield return new WaitForSeconds(ResetTime);
        CurrentAttack = null;
        IsPlayerAttacking = false;
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

    public IEnumerator CheckFall()
    {
        Debug.Log($"Fall Started");
        IsCheckingGround = true;
        float Highest = 0;
        while (!PlayerIsTouchingGround)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Player.transform.position, Vector2.down, 100f, FallLayerMask);

            if (hit2D.collider != null)
                if (hit2D.distance > Highest)
                {
                    Highest = hit2D.distance;
                }
            yield return null;
        }
        if (Highest > FallDamage)
        {
            Vector3 position = new Vector3(transform.position.x,transform.position.y - 1, 0);
            ParticelManager.instance.SpawnParticle(position, "Particle System Falling", 0.6f);
            Debug.Log($"Fall: {Highest}");
        }
        IsCheckingGround = false;
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

        Player.transform.localPosition = manager.dataSOs.playerDataSO.PlayerPosition != null ? manager.dataSOs.playerDataSO.PlayerPosition : Vector3.zero;
        SkinIndex = manager.dataSOs.playerDataSO.SkinIndex;
        CheckSkin();
        if (UnityEngine.ColorUtility.TryParseHtmlString("#" + manager.dataSOs.playerDataSO.colorHex, out Color colorHex))
            for (int i = 0; i < BodyPartsContainer.transform.childCount; i++)
            {
                BodyPartsContainer.transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorHex;
            }
        healthManagerPlayer.PlayerHealth = manager.dataSOs.playerDataSO.Health! <= 0 ? manager.dataSOs.playerDataSO.Health : 3;

    }
    public void SaveData(SaveManager manager)
    {
        manager.dataSOs.playerDataSO.PlayerPosition = this.Player.transform.localPosition;
        manager.dataSOs.playerDataSO.Health = healthManagerPlayer.PlayerHealth;
        manager.dataSOs.playerDataSO.SkinIndex = SkinIndex;
    }
}


