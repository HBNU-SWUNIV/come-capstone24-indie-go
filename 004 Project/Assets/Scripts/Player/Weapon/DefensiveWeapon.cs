using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//받는피해 감소 및 넉백과 관련된 로직은 추후 만들 예정.
public static class ShieldStateName
{
    public const string Enter = "Enter";
    public const string Exit = "Exit";
    public const string Hold = "Hold";
    public const string Parry = "Parry";
}
public class DefensiveWeapon : Weapon
{
    [SerializeField] protected Shield_WeaponData weaponData;

    ShieldStateMachine stateMachine;

    public ShieldEnterState ShieldEnterState { get; private set; }
    public ShieldExitState ShieldExitState { get; private set; }
    public ShieldHoldState ShieldHoldState { get; private set; }
    public ShieldParryState ShieldParryState { get; private set; }
    public bool isDefending;


    private void Awake()
    {
        stateMachine = new ShieldStateMachine();
    }
    protected override void Start()
    {
        base.Start();
        //Start에서 초기화 하거나 Player에서 초기화
        ShieldEnterState = new ShieldEnterState(this, shieldState, stateMachine, weaponData, ShieldStateName.Enter);
        ShieldExitState = new ShieldExitState(this, shieldState,  stateMachine, weaponData, ShieldStateName.Exit);
        ShieldHoldState = new ShieldHoldState(this, shieldState, stateMachine, weaponData, ShieldStateName.Hold);
        ShieldParryState = new ShieldParryState(this, shieldState, stateMachine, weaponData, ShieldStateName.Parry);

        weaponAnimationToWeapon.OnAction += AnimationActionTrigger;
        weaponAnimationToWeapon.OnFinish += AnimationFinishTrigger;
        weaponAnimationToWeapon.OnNextState += AnimationNextStateTrigger;

        isDefending = false;
    }

    private void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }
    public override void EnterWeapon()
    {
        base.EnterWeapon();
        stateMachine.Initialize(ShieldEnterState);
        isDefending = true;


    }

    public override void ExitWeapon()
    {
        base.ExitWeapon();
    }

    protected void ChangeState()
    {
       /* 
        if (parryState.CheckShieldHoldInput())
            currentState = ShieldWeaponState.Hold;
        if (parryState.CheckShieldInput() == false)
            currentState = ShieldWeaponState.Exit;
       */
    }
  

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        stateMachine.CurrentState.AnimationTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.CurrentState.AnimationFinishTrigger();
    }
    
    public void AnimationNextStateTrigger()
    {
        ChangeState();
      //  EnterWeapon();
    }

    public void CheckShield(GameObject go, float attackDamage, Vector2 knockbackAngle, float knockbackStrength, int facingDirection)
    {
        isDefending = true;
        // 방패 상태 확인
        if (stateMachine.CurrentState is ShieldEnterState)
            stateMachine.ChangeState(ShieldParryState);

        stateMachine.CurrentState.HandleShieldCollision(go, attackDamage, knockbackAngle, knockbackStrength, facingDirection);
       // ShieldWeaponHitboxToWeapon.ResetAlreadyHit();
    }

    public override void HandleCollision(Collider2D collision)
    {
        isDefending = true;
    }
}

public class ShieldStateMachine
{
    public ShieldState CurrentState { get; private set; }
    public void Initialize(ShieldState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(ShieldState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }


}

public class ShieldState
{
    protected DefensiveWeapon weapon;
    protected ShieldStateMachine stateMachine;
    protected Shield_WeaponData weaponData;
    protected PlayerShieldState playerShieldState;
    protected string animBoolName;
    protected bool isExitingState;
    public ShieldState(DefensiveWeapon weapon, PlayerShieldState playerShieldState, ShieldStateMachine stateMachine, Shield_WeaponData weaponData, string animBoolName)
    {
        this.weapon = weapon;
        this.playerShieldState = playerShieldState;
        this.stateMachine = stateMachine;
        this.weaponData = weaponData;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        weapon.baseAnimator.SetBool(animBoolName, true);
        weapon.weaponAnimator.SetBool(animBoolName, true);
        isExitingState = false;
        //Debug.Log(this.ToString() + "상태 진입");
    }
    public virtual void Exit()
    {
        weapon.weaponAnimator.SetBool(animBoolName, false);
        weapon.baseAnimator.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger()
    {

    }

    public virtual void HandleShieldCollision(GameObject collision, float attackDamage, Vector2 knockbackAngle, float knockbackStrength, int facingDirection) { }

}
public class ShieldEnterState : ShieldState
{
    private bool isDone;

    public ShieldEnterState(DefensiveWeapon weapon, PlayerShieldState playerShieldState, ShieldStateMachine stateMachine, Shield_WeaponData weaponData, string animBoolName) : base(weapon, playerShieldState, stateMachine, weaponData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isDone = false;
        playerShieldState.Movement?.SetVelocityZero();

        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.ParryAttempt);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isDone)
            {
                //if (패링한다면) 
                //    stateMachine.ChangeState(weapon.ShieldParryState);
                if (playerShieldState.ShieldHoldInput)
                    stateMachine.ChangeState(weapon.ShieldHoldState);
                else if (playerShieldState.ShieldInput == false)
                    stateMachine.ChangeState(weapon.ShieldExitState);

            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isDone = true;
    }

}

public class ShieldExitState : ShieldState
{
    private bool isDone;
    public ShieldExitState(DefensiveWeapon weapon, PlayerShieldState playerShieldState, ShieldStateMachine stateMachine, Shield_WeaponData weaponData, string animBoolName) : base(weapon, playerShieldState,stateMachine, weaponData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerShieldState.Movement?.SetVelocityX(0);
        weapon.isDefending = false;
        isDone = false;


    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            playerShieldState.Movement?.SetVelocityX(0);

            if (isDone)
            {
                playerShieldState.AnimationFinishTrigger();
            }
        }

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isDone = true;
    }

}

public class ShieldHoldState : ShieldState
{
    private float xInput;
    public ShieldHoldState(DefensiveWeapon weapon, PlayerShieldState playerShieldState, ShieldStateMachine stateMachine, Shield_WeaponData weaponData, string animBoolName) : base(weapon, playerShieldState, stateMachine, weaponData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DefenceAttempt);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            //이동 입력이 있을 경우에만. xinput != 0.. player를 public으로 할 순 없으니 보류
            //weapon.shieldState.Movement?.SetVelocityX(weaponData.holdMovementVelocity * weapon.shieldState.Movement.FacingDirection);

            playerShieldState.Movement?.SetVelocityX(0);
            if (playerShieldState.ShieldHoldInput == false)
            {
                stateMachine.ChangeState(weapon.ShieldExitState);
            }
            //else if (패링한다면) 
            //    stateMachine.ChangeState(weapon.ShieldParryState);
        }

    }

    public override void HandleShieldCollision(GameObject go, float attackDamage, Vector2 knockbackAngle, float knockbackStrength, int facingDirection)
    {
        // 홀드 상태일 때

        // 플레이어 데미지
        IDamageable playerDamageable = weapon.transform.root.GetComponentInChildren<IDamageable>();
        if (playerDamageable != null)
        {
         //   Debug.Log("홀드 피해 감소");
            playerDamageable.DamageWithShield(attackDamage); // 피해 감소
        }

        // 플레이어 넉백
        IKnockbackable playerKnockbackable = weapon.transform.root.GetComponentInChildren<IKnockbackable>();
        if (playerKnockbackable != null)
        {
            playerKnockbackable.KnockbackWithShield(knockbackAngle, knockbackStrength, facingDirection); // 넉백 감소
        }

        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DefenceSuccess);
    }
}

public class ShieldParryState : ShieldState
{

    private bool isDone;
    public ShieldParryState(DefensiveWeapon weapon, PlayerShieldState playerShieldState, ShieldStateMachine stateMachine, Shield_WeaponData weaponData, string animBoolName) : base(weapon, playerShieldState, stateMachine, weaponData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isDone = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isDone)
        {
            if (!isExitingState)
            {
                stateMachine.ChangeState(weapon.ShieldExitState);
            }
        }
    }

    public override void HandleShieldCollision(GameObject go, float attackDamage, Vector2 knockbackAngle, float knockbackStrength, int facingDirection)
    {
        base.HandleShieldCollision(go, attackDamage, knockbackAngle, knockbackStrength, facingDirection);

        // 패링 상태일 때
        // 적에게 피해 주기 로직 추가
        IDamageable damageable = go.GetComponentInChildren<IDamageable>();
        if (damageable != null)
        {
            //Debug.Log("패링");
            damageable.Damage(weaponData.parryDamage); // 패링 시 적에게 피해 줌
        }
        //적에게 넉백
        IKnockbackable knockbackable = go.GetComponentInChildren<IKnockbackable>();
        if (knockbackable != null)
        {
            knockbackable.Knockback(weaponData.knockbackAngle, weaponData.knockbackStrength, playerShieldState.Movement.FacingDirection);
        }
        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.ParrySuccess);

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isDone = true;

    }
}
