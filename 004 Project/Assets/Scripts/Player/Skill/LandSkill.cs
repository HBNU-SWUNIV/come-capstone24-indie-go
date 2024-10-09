using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSkill : ConcreteSkill
{
    public LandSkillEnterState LandSkillEnterState { get; private set; }
    public LandSkillExitState LandSkillExitState { get; private set; }

    public LandAttackState LandAttackState { get; private set; }
    public override void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default)
    {
        base.Initialize(skill, prefab, prefabParent, playerTransform, prefabOffset);
        LandSkillEnterState = new LandSkillEnterState(this, skill, stateMachine, SkillStateName.Enter);
        LandSkillExitState = new LandSkillExitState(this, skill, stateMachine, SkillStateName.Exit);
        LandAttackState = new LandAttackState(this, skill, stateMachine, SkillStateName.Attack, prefab, prefabParent, playerTransform, prefabOffset);
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Initialize(LandSkillEnterState);

    }
}

public class LandSkillEnterState : SkillEnterState
{
    private LandSkill landSkill;
    public LandSkillEnterState(LandSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.landSkill = concreteSkill;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ChangeState(landSkill.LandAttackState);

    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class LandSkillExitState : SkillExitState
{
    private LandSkill landSkill;

    public LandSkillExitState(LandSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        landSkill = concreteSkill;
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

public class LandAttackState : SkillState
{
    private GameObject prefab;
    private Transform prefabParent;
    private Transform playerTransform;
    private Vector2 prefabOffset;
    public LandAttackState(LandSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName, GameObject prefab, Transform prefabParent, Transform playerTransform, Vector2 prefabOffset) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.prefab = prefab;
        this.prefabParent = prefabParent;
        this.playerTransform = playerTransform;
        this.prefabOffset = prefabOffset;
    }
    public override void Enter()
    {
        base.Enter();

        skill.EventHandler.OnStateFinish += EventHandler;

        int facingDirection = skillMovement.GetFacingDirection();

        Vector3 spawnPosition = playerTransform.position + (Vector3)prefabOffset * facingDirection;

        GameObject rock = GameManager.Resource.Instantiate(prefab, spawnPosition, Quaternion.identity, prefabParent);  // 

        if(rock != null)
        {
            SkillDamage skillDamage = skill.GetComponent<SkillDamage>();
            if(skillDamage != null)
            {
                skillDamage.Initialize(rock);
            }

        }
    }

    public override void Exit()
    {
        base.Exit();
        skill.EventHandler.OnStateFinish -= EventHandler;
    }
    public override void LogicUpdate()
    {
        if (!isExitingState)
        {
            skillMovement.HandleStopMovementX();
        }
    }

    private void EventHandler()
    {
        stateMachine.ChangeState(concreteSkill.SkillExitState);


    }

}