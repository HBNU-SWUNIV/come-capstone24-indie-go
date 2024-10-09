using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : ConcreteSkill
{
    public IceSkillEnterState IceSkillEnterState { get; private set; }
    public IceSkillExitState IceSkillExitState { get; private set; }

    public IceAttack1State IceAttack1State { get; private set; }
    public IceAttack2State IceAttack2State { get; private set; }
    public int CurrentAttackCounter { get => currentAttackCounter; set => currentAttackCounter = value >= 2 ? 0 : value; }
    private int currentAttackCounter;

    public override void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default)
    {
        base.Initialize(skill, prefab, prefabParent, playerTransform, prefabOffset);
        IceSkillEnterState = new IceSkillEnterState(this, skill, stateMachine, SkillStateName.Enter);
        IceSkillExitState = new IceSkillExitState(this, skill, stateMachine, SkillStateName.Exit);
        IceAttack1State = new IceAttack1State(this, skill, stateMachine, SkillStateName.Attack1);
        IceAttack2State = new IceAttack2State(this, skill, stateMachine, SkillStateName.Attack2);
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Initialize(IceSkillEnterState);
    }
}

public class IceSkillEnterState : SkillEnterState
{
    private IceSkill iceSkill;
    public IceSkillEnterState(IceSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        this.iceSkill = concreteSkill;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("iceSkill.CurrentAttackCounter : " + iceSkill.CurrentAttackCounter);
        if (iceSkill.CurrentAttackCounter == 0)
            stateMachine.ChangeState(iceSkill.IceAttack1State);
        else if (iceSkill.CurrentAttackCounter == 1)
            stateMachine.ChangeState(iceSkill.IceAttack2State);


    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class IceSkillExitState : SkillExitState
{
    private IceSkill iceSkill;

    public IceSkillExitState(IceSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        iceSkill = concreteSkill;
    }
    public override void Enter()
    {
        base.Enter();
        skill.EventHandler.AnimationFinishedTriggerFunc();
    }

    public override void Exit()
    {
        base.Exit();

    }
}

public class IceAttackState : SkillState
{
    protected IceSkill iceSkill;
    protected Coroutine checkAttackReInputCor;

    protected CoroutineHandler coroutineHandler;
    protected bool resetCounter;

    public IceAttackState(IceSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {
        iceSkill = concreteSkill;
        coroutineHandler = skill.GetComponentInParent<CoroutineHandler>();

    }
    public override void Enter()
    {
        base.Enter();
        skill.EventHandler.OnStateFinish += EventHandler;

        resetCounter = false;

    }

    public override void Exit()
    {
        base.Exit();
        skill.EventHandler.OnStateFinish -= EventHandler;

        if (!resetCounter)
        {
            iceSkill.CurrentAttackCounter += 1;
            Debug.Log("wmdrk");
        }
    }

    protected void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            coroutineHandler.StopCoroutine(checkAttackReInputCor);

        checkAttackReInputCor = coroutineHandler.StartManagedCoroutine(CheckAttackReInputCoroutine(reInputTime), ResetAttackCounter);
    }

    private IEnumerator CheckAttackReInputCoroutine(float reInputTime)
    {
        float currentTime = 0f;
        while (currentTime < reInputTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    private void ResetAttackCounter()
    {
        Debug.Log("ResetIceSkillCounter");
        resetCounter = true;
        iceSkill.CurrentAttackCounter =0;

    }

    // 공격 도중 피격 시, 애니메이션이 끝나기 직전까지 넉백당하는 문제 발생

    private void EventHandler()
    {
        stateMachine.ChangeState(iceSkill.IceSkillExitState);
    }
}

    //5초 이내로 c를 한번 더 누르면 2번째 스킬 발동
public class IceAttack1State : IceAttackState
{ 
    public IceAttack1State(IceSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        CheckAttackReInput(5f);
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

public class IceAttack2State : IceAttackState
{
    public IceAttack2State(IceSkill concreteSkill, Skill skill, SkillStateMachine stateMachine, string animBoolName) : base(concreteSkill, skill, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        GameManager.SharedCombatDataManager.IsPlayerNotKnockback = true;
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.SharedCombatDataManager.IsPlayerNotKnockback = false;
    }

}