using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{

    private bool canDash;
    private float lastDashTime;
    
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canDash = false;
        player.InputHandler.UseDashInput();
        GameManager.PlayerManager.PlayerDataCollect.RecordAction(PlayerDataCollectName.DashAttempt);
        if (GameManager.SharedCombatDataManager.IsPlayerWithinAttackRange)
        {
            GameManager.SharedCombatDataManager.SetPlayerDashing(true);
        }
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
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);

            Movement?.SetVelocityX(playerData.dashVelocity * Movement.FacingDirection);

            if(Time.time >= startTime + playerData.dashTime)
            {
                player.RB.drag = 0f;
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
            else if(!isAbilityDone)
            {

            }
            if (GameManager.SharedCombatDataManager.IsPlayerHit)
            {
                stateMachine.ChangeState(player.HitState);
            }

        }


    }



    public bool CheckIfCanDash() => canDash && Time.time >= lastDashTime + playerData.dashCoolDown;
    public void ResetCanDash() => canDash = true;
}
