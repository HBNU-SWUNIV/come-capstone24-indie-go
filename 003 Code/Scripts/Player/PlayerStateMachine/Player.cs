using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerDataCollectName
{
    public const string AttackAttempt = "AttackAttempt";
    public const string AttackSuccess = "AttackSuccess";
    public const string DefenceAttempt = "DefenceAttempt";
    public const string DefenceSuccess = "DefenceSuccess";
    public const string ParryAttempt = "ParryAttempt";
    public const string ParrySuccess = "ParrySuccess";
    public const string DashAttempt = "DashAttempt";
    public const string DashSuccess = "DashSuccess";
    public const string DashFailure = "DashFailure";
    public const string RunAttempt = "RunAttempt";
    public const string RunSuccess = "RunSuccess";

}
public static class PlayerAnimStatesName
{
    public const string Idle = "Idle";
    public const string Move = "Move";
    public const string Jump = "jump";
    public const string Land = "Land";
    public const string InAir = "InAir";
    public const string Action = "Action";
    public const string Dash = "Dash";
    public const string Hit = "Hit";
    public const string WallSlide = "WallSlide";
    public const string WallGrab = "WallGrab";
    public const string WallClimb = "WallClimb";
    public const string LedgeClimbState = "LedgeClimbState";
    public const string CrouchIdle = "CrouchIdle";
    public const string CrouchMove = "CrouchMove";

}
public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    // 각종 STATE
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerShieldState ShieldState { get; private set; }
    public PlayerSkillState SkillState { get; private set; }
    public PlayerHitState HitState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    #region Components
    public Core Core { get; private set; }
    protected Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }

    private Movement movement;
    private PlayerStats stats;

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    //public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    //test
    #endregion

    public Skill skill { get; private set; }

    private Weapon primaryWeapon;
    private Weapon shieldWeapon;
    
    private Vector2 workspace;

    Transform effectParticles;
    Transform elementParticles;

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        //GameObject.Find말고 자식으로 서치하는것을 찾기.
        primaryWeapon = GameObject.Find("Sword").GetComponent<Weapon>();
        shieldWeapon = GameObject.Find("Shield").GetComponent<Weapon>();
        skill = GameObject.Find("Skill1").GetComponent<Skill>();
        stats = GetComponentInChildren<PlayerStats>();

        skill.SetCore(Core);

        playerData = new PlayerData();
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, PlayerAnimStatesName.Idle);
        MoveState = new PlayerMoveState(this, StateMachine, playerData, PlayerAnimStatesName.Move);
        JumpState = new PlayerJumpState(this, StateMachine, playerData, PlayerAnimStatesName.InAir); //jump 아님
        LandState = new PlayerLandState(this, StateMachine, playerData, PlayerAnimStatesName.Land);
        InAirState = new PlayerInAirState(this, StateMachine, playerData, PlayerAnimStatesName.InAir);
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, PlayerAnimStatesName.Action, primaryWeapon);
        //   SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, PlayerAnimStatesName.Attack);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, PlayerAnimStatesName.WallSlide);
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, PlayerAnimStatesName.WallGrab);
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, PlayerAnimStatesName.WallClimb);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, PlayerAnimStatesName.InAir);
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, PlayerAnimStatesName.LedgeClimbState);
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, PlayerAnimStatesName.CrouchIdle);
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, PlayerAnimStatesName.CrouchMove);
        DashState = new PlayerDashState(this, StateMachine, playerData, PlayerAnimStatesName.Dash);
        ShieldState = new PlayerShieldState(this, StateMachine, playerData, PlayerAnimStatesName.Action, shieldWeapon);
        SkillState = new PlayerSkillState(this, StateMachine, playerData, PlayerAnimStatesName.Action, skill);
        HitState = new PlayerHitState(this, StateMachine, playerData, PlayerAnimStatesName.InAir);

        primaryWeapon.InitializeAttackWeapon(PrimaryAttackState);
        shieldWeapon.InitializeShieldWeapon(ShieldState);

        effectParticles = transform.Find("Particles");
        elementParticles = transform.Find("Core/Element");
    }

    protected void OnEnable()
    {
        if(StateMachine.CurrentState != null)
            StateMachine.ChangeState(IdleState);

        if (effectParticles != null)
        {
            RemoveAllChildObjects(effectParticles);
        }
        if (elementParticles != null)
        {
            RemoveAllChildObjects(elementParticles);
        }
    }
    protected virtual void OnDisable()
    {
        Movement?.SetVelocityZero();
        GameManager.SharedCombatDataManager.IsPlayerNotKnockback = false;
    }

    protected void RemoveAllChildObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("child : " + child.name);
            Destroy(child.gameObject);
        }
    }


    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate(); //행동Comopnent관련

        if(StateMachine.isUpdating)
            StateMachine.CurrentState.LogicUpdate(); //상태State관련
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }

    // AnimationEvent
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

}
