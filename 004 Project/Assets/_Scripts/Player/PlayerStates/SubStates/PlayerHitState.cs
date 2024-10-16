using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerAbilityState
{

    public PlayerHitState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.SharedCombatDataManager.SetPlayerHit(false);
        Movement?.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        {
            if(Time.time >= startTime + playerData.hitRecoveryTime)
            {
                isAbilityDone = true;
            }
        }
    }

    private void HandleAttack()
    {

    }
}
