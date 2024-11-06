using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;

    private void TriggerCheck()
    {
        attackState.TriggerCheck();
    }

    private void FinishCheck()
    {
        attackState.FinishCheck();
    }   


    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
