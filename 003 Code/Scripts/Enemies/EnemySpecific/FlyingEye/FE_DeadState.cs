using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FE_DeadState : DeadState
{
    private FlyingEye enemy;

    public FE_DeadState(Entity etity, MonsterStateMachine stateMachine, string animBoolName, D_DeadState stateData, FlyingEye enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
