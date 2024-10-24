using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : MonsterState
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_StunState stateData;

	protected bool isStunTimeOver;
	protected bool isGrounded;
	protected bool isMovementStopped;
	protected bool performCloseRangeAction;
	protected bool isPlayerInMinAgroRange;
	public bool stun;
	protected float freezeStunTime = 0f;
	protected float parryStunTime = 0f;
	
	
	public StunState(Entity etity, MonsterStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(etity, stateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();

		isGrounded = CollisionSenses.Ground;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
	}

	public override void Enter()
	{
		base.Enter();
		stun = false;
		isStunTimeOver = false;
		isMovementStopped = false;
		//Movement?.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!isExitingState)
		{
	/*		if (Time.time >= startTime + stunTime)
			{
				isStunTimeOver = true;
				SetStunTime(0f);
				Debug.Log("스턴종료");
			}*/

			if (freezeStunTime != 0f)
			{
				if (Time.time >= startTime + freezeStunTime)
				{
					isStunTimeOver = true;
					SetFreezeStunTime(0f);
				}
			}
			else if (parryStunTime != 0f && freezeStunTime == 0f)
            {
				if (Time.time >= startTime + parryStunTime)
				{
					isStunTimeOver = true;
					SetParryStunTime(0f);
				}
			}
			else
			{
				if (Time.time >= startTime + stateData.stunTime)
					isStunTimeOver = true;
			}

//			if (isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
//			{
	//			isMovementStopped = true;
	//			Movement?.SetVelocityX(0f);
	//		}

		}

	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public void SetParryStunTime(float time)
	{
		parryStunTime = time;
	}
	public void SetFreezeStunTime(float time)
    {
		freezeStunTime = time;
    }
}