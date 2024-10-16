using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public MonsterState currentState { get; private set; }

    public void Initialize(MonsterState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeState(MonsterState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
