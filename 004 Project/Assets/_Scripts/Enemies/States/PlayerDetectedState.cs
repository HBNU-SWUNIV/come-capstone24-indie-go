using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : MonsterState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSeses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected D_PlayerDetectedState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool performMeleeAttackRangeAction;
    protected bool isDetectingLedge;


    public PlayerDetectedState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = CollisionSeses.LedgeVertical;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        performMeleeAttackRangeAction = entity.CheckPlayerInMeleeAttackRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMaxAgroRange)
        {
            int playerPosition = entity.GetPlayerRelativePosition();

            if (playerPosition != 0 && playerPosition != Movement.FacingDirection)
            {
                Movement?.Flip();
            }
        }
        Movement?.SetVelocityX(0.0f);

        if (Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
        if (entity.stunState.stun)
            stateMachine.ChangeState(entity.stunState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
