using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool dashInput;
    private bool shieldInput;
    private bool skillInput;
    private bool grabInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {

    }
    public override void DoChecks()
    {
        base.DoChecks();
        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
    }

    public override void Enter()
    {
        base.Enter();
        player.DashState.ResetCanDash();
        player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        shieldInput = player.InputHandler.ShieldInput;
        skillInput = player.InputHandler.SkillInput;

        //땅에 있다면 언제든지 실행 가능한 코드.
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if(jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if(shieldInput && !isTouchingCeiling)// & player.ShieldState.CanShield())
        {
            stateMachine.ChangeState(player.ShieldState);
        }
        else if(skillInput && player.SkillState.CanSkill() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SkillState);
        }
        else if(!isGrounded)
        {
            //player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (GameManager.SharedCombatDataManager.IsPlayerHit && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.HitState);
        }


    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
