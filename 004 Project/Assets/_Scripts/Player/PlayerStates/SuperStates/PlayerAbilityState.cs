using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement);}
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    private Movement movement;
    private CollisionSenses collisionSenses;

    private bool isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {

    }
    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
        {
            isGrounded = collisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        //AnimationFinishTrigger가 실행되면 넘어가는 로직.
        if(isAbilityDone)
        {
            // x축의 속도도 받아와서 Idle을 조정하기.
            if(isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            } else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public Movement GetMovement()
    {
        return Movement;
    }
}
