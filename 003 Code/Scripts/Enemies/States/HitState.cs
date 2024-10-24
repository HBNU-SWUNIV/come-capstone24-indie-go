using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ÀÛ¼ºÁß..
/*
public class HitState : MonsterState
{
    protected D_HitState stateData;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;



    protected bool isPlayerInMinAgroRange;
    protected bool performCloseRangeAction;

    public HitState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, D_HitState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        entity.CheckPlayer().GetComponentInChildren<Movement>();
    }

    public override void Enter()
    {
        base.Enter();
        if(!isPlayerInMinAgroRange)
        {
            Movement?.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
*/