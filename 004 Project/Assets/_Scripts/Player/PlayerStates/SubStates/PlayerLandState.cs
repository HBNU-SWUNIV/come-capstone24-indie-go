using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!isExitingState)
        {
            if(xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if(isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

}
