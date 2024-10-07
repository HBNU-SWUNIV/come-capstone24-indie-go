using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillStateName
{
    public const string Enter = "Enter";
    public const string Exit = "Exit";
    public const string Hold = "Hold";
    public const string Fire = "Fire";
}
public abstract class ConcreteSkill : ISkillAction
{
    protected Skill skill;
    public Animator baseAnim;
    public Animator weaponAnim;

    private GameObject prefab;
    private Transform prefabParent;
    private SkillStateMachine stateMachine;
    private Transform playerTransform;
    private Vector2 prefabOffset;
    public SkillEnterState SkillEnterState { get; private set; }
    public SkillExitState SkillExitState { get; private set; }
    public SkillHoldState SkillHoldState { get; private set; }
    public SkillFireState SkillFireState { get; private set; }
    public virtual void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default(Vector2))
    {
        this.skill = skill;
        baseAnim = skill.BaseGameObject.GetComponent<Animator>();
        weaponAnim = skill.WeaponGameObject.GetComponent<Animator>();
        stateMachine = new SkillStateMachine();
        skill.OnLogicUpdate += LogicUpdate;
        if (prefab != null)
            this.prefab = prefab;
        if (prefabParent != null)
            this.prefabParent = prefabParent;
        SkillEnterState = new SkillEnterState(this, skill, stateMachine, SkillStateName.Enter);
        SkillExitState = new SkillExitState(this, skill, stateMachine, SkillStateName.Exit);
        SkillHoldState = new SkillHoldState(this, skill, stateMachine, SkillStateName.Hold);
        SkillFireState = new SkillFireState(this, skill, stateMachine, SkillStateName.Fire, prefab, prefabParent, playerTransform, prefabOffset);

    }
    public virtual void Enter()
    {
        stateMachine.Initialize(SkillEnterState);
    }

    public virtual void Exit()
    {
    }
    private void LogicUpdate()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

}

public class SkillStateMachine
{
    public SkillState CurrentState { get; private set; }
    public void Initialize(SkillState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(SkillState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
public class SkillState
{

    protected ConcreteSkill concreteSkill;
    protected Skill skill;
    protected Animator baseAnim;
    protected Animator weaponAnim;
    protected string animBoolName;
    protected bool isExitingState;
    protected SkillStateMachine stateMachine;

    protected SkillMovement skillMovement;
    protected SkillDamage skillDamage;
    protected SkillSpear skillSpear;
    //기타 Component들.. 이 Component들은 상속받은 애가 구현

    public SkillState(ConcreteSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName)
    {
        this.concreteSkill = concreteSkill;
        this.skill = skill;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        baseAnim = concreteSkill.baseAnim;
        weaponAnim = concreteSkill.weaponAnim;

        skillMovement = skill.GetComponent<SkillMovement>();
        skillDamage = skill.GetComponent<SkillDamage>();
        skillSpear = skill.GetComponent<SkillSpear>();

    }
    public virtual void Enter()
    {
        baseAnim.SetBool(animBoolName, true);
        weaponAnim.SetBool(animBoolName, true);
        isExitingState = false;
       // Debug.Log(this.ToString() + "상태 진입");
    }
    public virtual void Exit()
    {
        //Debug.Log(this.ToString() + "상태 종료");
        baseAnim.SetBool(animBoolName, false);
        weaponAnim.SetBool(animBoolName, false);
        isExitingState = false;
    }
    public virtual void LogicUpdate()
    {

    }
}


public class SkillEnterState : SkillState
{
    private bool isDone;
    public SkillEnterState(ConcreteSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
    }

    //피격 시 애니메이션을 수행중 이었으면 애니메이션이 끝나기 전까지 이동이 계속 이루어지는 버그 수정해야함.
    public override void Enter()
    {
        base.Enter();
        isDone = false;
        skill.EventHandler.OnStateFinish += EventHandler;
    }

    public override void Exit()
    {
        base.Exit();
        skill.EventHandler.OnStateFinish -= EventHandler;

    }
    public override void LogicUpdate()
    {
        if(!isExitingState)
        {
            skillMovement.HandleStopMovementX();
        }
    }
    protected virtual void EventHandler()
    {
        skill.HoldSkill();
        if(skill.hold)
        {
            stateMachine.ChangeState(concreteSkill.SkillHoldState);
        }
        else
        {
            stateMachine.ChangeState(concreteSkill.SkillFireState);
        }
    }
}

public class SkillExitState : SkillState
{
    public SkillExitState(ConcreteSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //skill.EventHandler.OnFinish -= EventHandler;
        skill.EventHandler.OnFinish += EventHandler;

    }

    public override void Exit()
    {
        base.Exit();
        skill.EventHandler.OnFinish -= EventHandler;

    }
    public override void LogicUpdate()
    {
        if (!isExitingState)
        {
            skillMovement.HandleStopMovementX();
        }
        //애니메이션finishTrigger를 하면 종료됨.
    }

    private void EventHandler()
    {
        Exit();
    }
}
public class SkillHoldState : SkillState
{
    public SkillHoldState(ConcreteSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
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
        skill.HoldSkill();
        if (!skill.hold)
        {
            stateMachine.ChangeState(concreteSkill.SkillFireState);

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
    public SkillFireState(ConcreteSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName, GameObject prefab, Transform prefabParent, Transform playerTransform, Vector2 prefabOffset) : base(concreteSkill, skill, stateMachine, animBoolName)
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