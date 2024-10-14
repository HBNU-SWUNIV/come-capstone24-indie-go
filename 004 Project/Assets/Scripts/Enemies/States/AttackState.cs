using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonsterState
{


    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected Transform attackPosition;
    protected Collider2D collision;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;

    public AttackState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(entity, stateMachine, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.atsm.attackState = this;
        isAnimationFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(0f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {
    }
    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }

    public virtual void TriggerCheck()
    {

    }

    public virtual void FinishCheck()
    {

    }
    public virtual void HandleAttack(Collider2D collision)
    {
        this.collision = collision;
    }

    public void SetAattackCheck(AnimationToAttackCheck attackCheck)
    {
        attackCheck.InitializedAttackCheck(this);
    }
}
