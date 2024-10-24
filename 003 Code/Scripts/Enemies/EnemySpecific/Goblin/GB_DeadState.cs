using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GB_DeadState : DeadState
{
    private Goblin enemy;

    public GB_DeadState(Entity etity, MonsterStateMachine stateMachine, string animBoolName, D_DeadState stateData, Goblin enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
