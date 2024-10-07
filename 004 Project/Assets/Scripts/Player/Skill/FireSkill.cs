using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : ConcreteSkill
{
    public FireSkillEnterState FireSkillEnterState { get; private set; }
    public FireSkillExitState FireSkillExitState { get; private set; }

    public FireAttackState FireAttackState { get; private set; }
    public override void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default)
    {
        base.Initialize(skill, prefab, prefabParent, playerTransform, prefabOffset);
        FireSkillEnterState = new FireSkillEnterState(this, skill, stateMachine, SkillStateName.Enter);
        FireSkillExitState = new FireSkillExitState(this, skill, stateMachine, SkillStateName.Exit);
        FireAttackState = new FireAttackState(this, skill, stateMachine, SkillStateName.Attack);
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Initialize(FireSkillEnterState);

    }
}

public class FireSkillEnterState : SkillEnterState
{
    private FireSkill fireSkill;
    public FireSkillEnterState(FireSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.fireSkill = concreteSkill;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ChangeState(fireSkill.FireAttackState);

    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class FireSkillExitState : SkillExitState
{
    private FireSkill fireSkill;

    public FireSkillExitState(FireSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        fireSkill = concreteSkill;
    }
    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class FireAttackState : SkillState
{
    public FireAttackState(FireSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
 
    }
    public override void Enter()
    {
        base.Enter();


        
    }

    public override void Exit()
    {
        base.Exit();

    }
    public override void LogicUpdate()
    {
        if (!isExitingState)
        {
            skillMovement.HandleStopMovementX();
        }
    }

}