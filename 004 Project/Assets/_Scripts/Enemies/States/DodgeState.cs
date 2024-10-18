using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : MonsterState
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
	private Movement movement;
	private CollisionSenses collisionSenses;
	private Combat combat;
	protected D_DodgeState stateData;

	protected bool performCloseRangeAction;
	protected bool isPlayerInMaxAgroRange;
	protected bool isGrounded;
	protected bool isDodgeOver;

	public DodgeState(Entity etity, MonsterStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(etity, stateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();

		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
		isGrounded = CollisionSenses.Ground;
	}

	public override void Enter()
	{
		base.Enter();
		isDodgeOver = false;
		if (!Movement.CanSetVelocity)
			Combat.ResetKnockback();
		// 넉백 불가능하도록 설정
		entity.IsKnockbackable = false;
		Movement?.AttackForce(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection);
	}

	public override void Exit()
	{
		base.Exit();

		// 넉백 가능하도록 재설정
		entity.IsKnockbackable = true;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (Time.time >= startTime + stateData.dodgeTime && isGrounded)
		{
			isDodgeOver = true;
			if (GameManager.SharedCombatDataManager.IsMonsterNotKnockbacks.ContainsKey(entity))
			{
				GameManager.SharedCombatDataManager.IsMonsterNotKnockbacks.Remove(entity);
			}
		}
		if (entity.stunState.stun)
			stateMachine.ChangeState(entity.stunState);
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
