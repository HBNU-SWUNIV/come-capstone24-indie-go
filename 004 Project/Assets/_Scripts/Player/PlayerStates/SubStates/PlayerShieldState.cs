using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldState : PlayerAbilityState
{
    private Weapon weapon;

    public bool ShieldInput { get; private set; }
    public bool ShieldHoldInput { get; private set; }

    
    public PlayerShieldState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string AnimBoolName, Weapon weapon) : base(player, stateMachine, playerData, AnimBoolName)
    {
        this.weapon = weapon;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        weapon.EnterWeapon();

    }

    public override void Exit()
    {
        base.Exit();
        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ShieldInput = player.InputHandler.ShieldInput;
        ShieldHoldInput = player.InputHandler.ShieldHoldInput;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
      //  weapon.InitializeWeapon(this);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }
    
}
