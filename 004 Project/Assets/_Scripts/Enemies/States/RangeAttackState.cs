using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState rangeStateData;

    protected GameObject projectile;
    protected Projectile projectileScript;
    Transform attackPosition;
    Transform projectileParent;

    private AnimationToAttackCheck rangeAttackCheck;

    public RangeAttackState(Entity etity, MonsterStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.rangeStateData = stateData;
        this.attackPosition = attackPosition;
        this.projectileParent = GameObject.Find("Arrows").transform;

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(rangeStateData.projectile, attackPosition.position, attackPosition.rotation, projectileParent);
        projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.FireProjectile(rangeStateData.projectileSpeed, rangeStateData.projectileTravelDistance, rangeStateData.projectileDamage);
        rangeAttackCheck = projectileScript.GetComponent<AnimationToAttackCheck>();
        rangeAttackCheck.OnPlayerHit += HandleAttack;
        projectileScript.OnDeleteAction += DeleteAction;

    }

    public void DeleteAction()
    {
        rangeAttackCheck.OnPlayerHit -= HandleAttack;
    }
}
