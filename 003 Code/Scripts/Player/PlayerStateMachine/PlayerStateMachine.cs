using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    public bool isUpdating = true;
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        isUpdating = false;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
        isUpdating = true;
    }
}
