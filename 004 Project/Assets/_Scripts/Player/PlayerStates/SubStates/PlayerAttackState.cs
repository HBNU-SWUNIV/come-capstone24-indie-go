using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;
    private float velocityToSet;

    private bool setVelocity;
    private bool shouldCheckFlip;


    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName, Weapon weapon) : base(player, stateMachine, playerData, AnimBoolName)
    {
        this.weapon = weapon;
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;
        shouldCheckFlip = true;
        weapon.EnterWeapon();
    }
    public override void Exit()
    {
        base.Exit();
        weapon.ExitWeapon();
        Movement?.SetVelocityZero();
        setVelocity = false;
    }

    bool shieldInput;
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            shieldInput = player.InputHandler.ShieldInput;
            xInput = player.InputHandler.NormInputX;  //공중 공격을 따로 만들예정.
            if (shieldInput)
            {
                stateMachine.ChangeState(player.ShieldState);
            }
            if (shouldCheckFlip)
            {
                Movement?.CheckIfShouldFlip(xInput);
            }

            if (setVelocity)
            {
                Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
            }

            if(GameManager.SharedCombatDataManager.IsPlayerHit)
            {
                stateMachine.ChangeState(player.HitState);
            }

        }

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }


    //방향 전환을 한지 얼마 지나지 않았었다면 일어나지 않게 설정
    public void SetFilpCheck(bool value)
    {
        shouldCheckFlip = value;
    }


}
