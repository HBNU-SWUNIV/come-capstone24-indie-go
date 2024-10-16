using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
   // AnimationToAttackCheck attackCheck;
    AnimationToPlayerDashCheck playerDashCheck;
    private AnimationToAttackCheck meleeAttackCheck;

    public MeleeAttackState(Entity entity, MonsterStateMachine stateMachine, string animBoolName, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        meleeAttackCheck = entity.transform.GetComponentInChildren<AnimationToAttackCheck>();

        playerDashCheck = entity.transform.GetComponentInChildren<AnimationToPlayerDashCheck>();
        enemyStats = entity.transform.GetComponentInChildren<EnemyStats>();
        this.stateData = stateData;
      //  attackCheck.OnPlayerHit += HandleAttack;
    }


    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        meleeAttackCheck.OnPlayerHit += HandleAttack;

    }

    public override void Exit()
    {
        base.Exit();
        meleeAttackCheck.OnPlayerHit -= HandleAttack;

    }

    public override void LogicUpdate()
    {
        //플레이어가 공격 범위에 있는지를 체크하고, 공격 trigger가 일어나기 전에 공격 범위 내를 벗어나면 카운트 증가
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        meleeAttackCheck.TriggerAttack();
    }
    public override void FinishAttack()
    {
        base.FinishAttack();

        meleeAttackCheck.FinishAttack();
    }

    public override void TriggerCheck()
    {
        base.TriggerCheck();

        playerDashCheck.TriggerCheck();
    }

    public override void FinishCheck()
    {
        base.FinishCheck();

        playerDashCheck.FinishCheck();
    }
    public override void HandleAttack(Collider2D collision)
    {
        base.HandleAttack(collision);
    }
}
