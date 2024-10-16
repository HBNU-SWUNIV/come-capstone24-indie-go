using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName) : base(player, stateMachine, playerData, AnimBoolName)
    {
        amountOfJumpLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        Movement?.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        amountOfJumpLeft--;
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        return (amountOfJumpLeft > 0);
    }
    public void ResetAmountOfJumpsLeft() => amountOfJumpLeft = playerData.amountOfJumps;
    public void DecreaseAmountOfJumpsLeft() => amountOfJumpLeft--;
}
