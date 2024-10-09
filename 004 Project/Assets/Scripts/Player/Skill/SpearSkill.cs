using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSkill : ConcreteSkill
{  
    public SpearSkillEnterState SpearSkillEnterState { get; private set; }
    public SpearSkillExitState SpearSkillExitState { get; private set; }
    public SkillHoldState SkillHoldState { get; private set; }
    public SkillFireState SkillFireState { get; private set; }
    public override void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default)
    {
        base.Initialize(skill, prefab, prefabParent, playerTransform, prefabOffset);
        SpearSkillEnterState = new SpearSkillEnterState(this, skill, stateMachine, SkillStateName.Enter);
        SpearSkillExitState = new SpearSkillExitState(this, skill, stateMachine, SkillStateName.Exit);
        SkillHoldState = new SkillHoldState(this, skill, stateMachine, SkillStateName.Hold);
        SkillFireState = new SkillFireState(this, skill, stateMachine, SkillStateName.Fire, prefab, prefabParent, playerTransform, prefabOffset);
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Initialize(SpearSkillEnterState);

    }
}

public class SpearSkillEnterState : SkillEnterState
{
    private SpearSkill spearSkill;
    public SpearSkillEnterState(SpearSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.spearSkill = concreteSkill;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

    }
    protected override void EventHandler()
    {
        base.EventHandler();
        skill.HoldSkill();
        if (skill.hold)
        {
            stateMachine.ChangeState(spearSkill.SkillHoldState);
        }
        else
        {
            stateMachine.ChangeState(spearSkill.SkillFireState);
        }
    }
}

public class SpearSkillExitState : SkillExitState
{
    private SpearSkill spearSkill;

    public SpearSkillExitState(SpearSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        spearSkill = concreteSkill;
    }
}

public class SkillHoldState : SkillState
{
    private SpearSkill spearSkill;

    public SkillHoldState(SpearSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.spearSkill = concreteSkill;

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
        skill.HoldSkill();
        if (!skill.hold)
        {
            stateMachine.ChangeState(spearSkill.SkillFireState);

        }
        if (!isExitingState)
        {
            skillMovement.HandleStopMovementX();
        }
    }
}

public class SkillFireState : SkillState
{
    private GameObject prefab;
    private Transform prefabParent;
    private Transform playerTransform;
    private Vector2 prefabOffset;
    public SkillFireState(SpearSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName, GameObject prefab, Transform prefabParent, Transform playerTransform, Vector2 prefabOffset) : base(concreteSkill, skill, stateMachine, animBoolName)
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

        GameObject arrow = GameManager.Resource.Instantiate(prefab, spawnPosition, Quaternion.identity, prefabParent); // 화살 생성

        if (arrow != null)
        {
            SkillDamage skillDamage = skill.GetComponent<SkillDamage>();
            if (skillDamage != null)
            {
                skillDamage.Initialize(arrow);
            }
            Vector2 arrowDirection = (facingDirection == 1) ? Vector2.right : Vector2.left;
            arrow.transform.right = Vector3.right * facingDirection;

            // 화살의 Rigidbody2D에 속도 설정
            Rigidbody2D arrowRigidbody = arrow.GetComponent<Rigidbody2D>();
            if (arrowRigidbody != null)
            {
                arrowRigidbody.velocity = arrowDirection * skillSpear.GetSpearThrowSpeed();
            }

            // 화살의 최대 이동 거리 설정
            Arrow arrowComponent = arrow.GetComponent<Arrow>();
            if (arrowComponent != null)
            {
                arrowComponent.SetThrowDistance(skillSpear.GetSpearThrowDistance());
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