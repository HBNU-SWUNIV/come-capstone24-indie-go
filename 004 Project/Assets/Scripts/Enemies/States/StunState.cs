using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : MonsterState
{

    D_StunState stateData;
    protected float stunTime = 0f;
    public StunState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
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
    public void SetStunTime(float time)
    {
        stunTime = time;
    }
}
