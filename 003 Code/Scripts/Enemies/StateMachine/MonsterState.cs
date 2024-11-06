using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState
{
    protected Core core;
    protected MonsterStateMachine stateMachine;
    protected Entity entity;
    protected bool isExitingState;
    public float startTime { get; protected set; }
    protected string animBoolName;

    public MonsterState(Entity entity, MonsterStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = entity.Core;
    }


    public virtual void Enter()
    {
        DoChecks();
        entity.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        entity.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }
    public virtual void DoChecks()
    {

    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
}
