using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerControl : EntityManager, IDataPersitence

{
    public static playerControl instance { get; private set; }
    [Header("Player Settings")]
    public PlayerMovement PlayerMovement;
    public PlayerState PlayerState;
    [Space(0.5f)]
    public LayerMask FallLayerMask;

    // The Current Skin number
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
    public BoxCollider2D JumpCollider;
    public List<Collider2D> Grounds;
    public GameObject BodyPartsContainer;
    public List<Sprite> SkinSprite;
    [SerializeField] private List<Skins> PlayerSkins;
    public GameObject PlayerInfoInteractionKey;
    private Vector2 moveInput;
    [Space(1)]
    [Header("Scripts")]
    [Space(0.5f)]
    private InputSystem_Actions inputActions;

    public HealthManagerPlayer healthManagerPlayer;
    public EnergyManager energyManager;
    public void Init()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Enable(); // Activates Player Control for him self

        SceneInfoManager.OnSceneChanged += CheckScene;

    }

    private void OnDisable()
    {
        inputActions?.Player.Disable();
    }
    protected override void Start()
    {
        playerCollider = Player.GetComponent<PolygonCollider2D>();
        rb = Player.GetComponent<Rigidbody2D>();

        PlayerInfoInteractionKey.SetActive(false);
        PlayerState.PlayerIsTouchingGround = false;
        PlayerState.CanAttack = true;
        PlayerState.IsCheckingGround = false;
        PlayerAniamtor.SetBool("Walk", false);

        GameObject[] all = FindObjectsByType<GameObject>();
        foreach (var obj in all)
        {
            if (obj.layer == LayerMask.NameToLayer("Solid Object"))
            {
                Grounds.Add(obj.GetComponent<Collider2D>());
            }
        }
    }

    #region  Update Loop
    protected void Update()
    {
        // Checks if the player CanMove from EntityManager so Animations and Sounds Can be played
        if (CanMove)
        {
            if (Mathf.Abs(moveInput.x) > 0.05f && !PlayerState.IsPlayerAttacking) // Checks if The Player is moving or i the Player is Attacking
            {
                if (!AudioManager.instance.isPlaying("Walk")) // Checks if the sound is already playing
                {
                    AudioManager.instance.PlayAudio("Walk", transform, Vector2.zero, 0, true, 1);
                }
                PlayerAniamtor.SetBool("Walk", true);
            }
            else
            {
                PlayerAniamtor.SetBool("Walk", false);
            }
            Vector3 move = new Vector3(moveInput.x, 0, 0);
            transform.Translate(move * PlayerMovement.MoveSpeed * Time.deltaTime); // Sets the position where the player wants to move to
        }
        else
        {
            if (AudioManager.instance.isPlaying("Walk"))
            {
                AudioManager.instance.StopAudio("Walk");
            }
        }

        foreach (var ground in Grounds)
        {
            if (JumpCollider.IsTouching(ground))
            {
                PlayerState.PlayerIsTouchingGround = true;
                break;
            }
            else
            {
                PlayerState.PlayerIsTouchingGround = false;
            }
        }


        if (inputActions.Player.Jump.WasPerformedThisFrame() && PlayerState.PlayerIsTouchingGround && CanMove)
        {
            PlayerState.IsPlayerAttacking = true;
            AudioManager.instance.PlayAudio("jump", transform, Vector2.zero);
            rb.AddForce(new Vector2(0, PlayerMovement.JumpForce));
            if (PlayerState.AnimationsCanPlay)
            {
                PlayerAniamtor.SetTrigger("Jump");
            }
        }
        /// 
        /// Falling System
        /// 
        if (!PlayerState.PlayerIsTouchingGround && !PlayerState.IsCheckingGround)
        {
            PlayerState.IsCheckingGround = true;
            StartCoroutine(CheckFall());
        }


        ////
        /// Dodge Roll
        /// 
        #region  Dodge Roll
        if (inputActions.Player.DodgeRoll.WasPerformedThisFrame() && PlayerState.CanAttack && energyManager.EnergyAmount >= 25)
        {
            PlayerState.IsPlayerAttacking = true;
            Debug.Log("Attack");
            float Direction = inputActions.Player.Move.ReadValue<Vector2>().x;
            PlayerState.CurrentAttack = "DodgeRoll";
            AudioManager.instance.PlayAudio("Rolling", transform, Vector2.one / 1.5f, 1, true, 1);
            if (Direction > 0)
            {
                Debug.Log("Attack R");
                if (PlayerState.AnimationsCanPlay)
                {
                    PlayerAniamtor.Play("PlayerDogeRoll");
                }
                moveInput.x += PlayerMovement.AttackRollStrength;
                VibrateControllerManager.instance.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            else
            {
                Debug.Log("Attack L");
                if (PlayerState.AnimationsCanPlay)
                {
                    PlayerAniamtor.Play("PlayerDogeRoll");
                }
                moveInput.x -= PlayerMovement.AttackRollStrength;
                VibrateControllerManager.instance.VibrateController(0.2f, 0.1f, 2);
                StartCoroutine(energyManager.RemoveEnergy(-25));
            }
            StartCoroutine(AttackWait(1.5f, 0.18f));
        }
        #endregion
        ////
        /// Hand Bumm
        /// 
        #region  Hand Bumm
        if (inputActions.Player.Attack.WasPerformedThisFrame() && PlayerState.CanAttack)
        {
            PlayerState.IsPlayerAttacking = true;
            AudioManager.instance.PlayAudio("Player_Punch", transform, Vector2.one *1.5f, 1);
            PlayerState.CurrentAttack = "HandBumm";
            if (PlayerState.AnimationsCanPlay)
            {
                PlayerAniamtor.Play("HandBumm");
            }
            StartCoroutine(AttackWait(0.8f, 0.1f));
        }
        #endregion
        /// 
        /// Sitting System
        ///
        #region Sittings System
        if (inputActions.Player.Interact.WasPerformedThisFrame() && CanMove)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);

            foreach (Collider2D hit in colliders)
            {
                SeatSystem seatSystem = hit.GetComponentInParent<SeatSystem>() ?? hit.GetComponentInChildren<SeatSystem>();

                if (seatSystem != null)
                {
                    Transform targetPosition = seatSystem.transform;

                    foreach (Transform child in seatSystem.transform)
                    {
                        if (child.CompareTag("Seat"))
                        {
                            targetPosition = child;
                            break;
                        }
                    }
                    EntityManager playerEntity = GetComponent<EntityManager>();

                    Debug.Log($"Setze {gameObject.name} auf {targetPosition.name}");
                    seatSystem.Sit(this.transform, targetPosition, playerEntity);

                    break;
                }
            }
        }
        #endregion
    }
    #endregion

    private void CheckScene(SceneSettings sceneSetting)
    {
        if (sceneSetting.tag != SceneTags.Game)
        {
            BodyPartsContainer.SetActive(false);
            CanMove = false;
            canTakeDamage = false;
        }
        else
        {
            BodyPartsContainer.SetActive(true);
            CanMove = true;
            canTakeDamage = true;

            Grounds.Clear();
            GameObject[] all = FindObjectsByType<GameObject>();
            foreach (var obj in all)
            {
                if (obj.layer == LayerMask.NameToLayer("Solid Object"))
                {
                    Grounds.Add(obj.GetComponent<Collider2D>());
                }
            }
        }

    }
    IEnumerator AttackWait(float ResetTime, float DelayTime)
    {
        PlayerState.CanAttack = false;
        yield return new WaitForSeconds(ResetTime);
        PlayerState.CurrentAttack = null;
        PlayerState.IsPlayerAttacking = false;
        yield return new WaitForSeconds(DelayTime);
        PlayerState.CanAttack = true;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            Vector3 myPosition = transform.position;
            Vector3 otherPosition = collider.transform.position;
            Vector3 collisionDirection = myPosition - otherPosition;

        }
    }
    #region  Check Fall
    public IEnumerator CheckFall()
    {
        Debug.Log($"Fall Started");
        PlayerState.IsCheckingGround = true;
        float Highest = 0;
        while (!PlayerState.PlayerIsTouchingGround)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Player.transform.position, Vector2.down, 100f, FallLayerMask);

            if (hit2D.collider != null)
                if (hit2D.distance > Highest)
                {
                    Highest = hit2D.distance;
                }
            yield return null;
        }
        if (Highest > PlayerMovement.FallDamage)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y - 1, 0);
            ParticelManager.instance.SpawnParticle(position, "Particle System Falling", 0.6f);
            Debug.Log($"Fall: {Highest}");
        }
        PlayerState.IsCheckingGround = false;
    }
    #endregion
    
    #region  Save / Load
    public void LoadData(SaveManager manager)
    {
        Player.transform.localPosition = manager.dataSOs.playerDataSO.PlayerPosition != null ? manager.dataSOs.playerDataSO.PlayerPosition : Vector3.zero;
        healthManagerPlayer.PlayerHealth = manager.dataSOs.playerDataSO.Health! <= 0 ? manager.dataSOs.playerDataSO.Health : 3;
        if (!string.IsNullOrEmpty(manager.dataSOs.playerDataSO.LastGameScene))
        {
            SceneInfoManager.instance.LastGameScene = manager.dataSOs.playerDataSO.LastGameScene;
        }
    }
    public void SaveData(SaveManager manager)
    {
        manager.dataSOs.playerDataSO.PlayerPosition = this.Player.transform.localPosition;
        manager.dataSOs.playerDataSO.Health = healthManagerPlayer.PlayerHealth;
        manager.dataSOs.playerDataSO.LastGameScene = SceneInfoManager.instance.LastGameScene;
    }
    #endregion
}

/// <summary>
/// The Player Movement Settings
/// </summary>
#region Player Movement Settings
[System.Serializable]
public class PlayerMovement
{
    [Tooltip("The Speed the player can move horizontal X axis")]
    [Min(0)] public float MoveSpeed = 3.5f;
    [Tooltip("The Amount The Player goes Up Y")]
    [Min(0)] public float JumpForce = 350;
    [Tooltip("The Speed the player can use when running should be higher than move speed")]
    [Min(0)] public int RunSpeed = 5;
    [Tooltip("The damage the Attack Roll dose to other NPCs")]
    [Min(0)] public float AttackRollDamage;
    [Tooltip("The force that moves the player in the direction")]
    [Min(0)] public float AttackRollStrength;
    [Tooltip("Hand bumm attack damage")]
    [Min(0)] public float HandBummAttackDamage;
    [Tooltip("The Damage the player takes when falling from to high")]
    [Min(0)] public float FallDamage = 6;
}
#endregion

#region  Player State
[System.Serializable]
public class PlayerState
{
    [Tooltip("Checks if the Player is Touching the current Ground that is selected")]
    public bool PlayerIsTouchingGround;
    [Tooltip("Checks if the Player is Currently in an attack")]
    public bool IsPlayerAttacking;
    [Tooltip("Checks if the script is already checking the ground")]
    public bool IsCheckingGround;
    [Tooltip("Checks if the Player is allowed to attack when set false he cant attack")]
    public bool CanAttack;
    [Tooltip("Checks if its allowed to Play animations")]
    public bool AnimationsCanPlay;
    [Header("The Current Attack as a string for checking the current attack")]
    public string CurrentAttack;
}
#endregion

#region  PlayerBodyParts
public class PlayerBodyParts
{
    public class Skins : ScriptableObject
    {
        public GameObject Head;
        public GameObject Body;
        public GameObject LeftArm;
        public GameObject RightArm;
        public GameObject LeftLeg;
        public GameObject RightLeg;
    }
}
#endregion