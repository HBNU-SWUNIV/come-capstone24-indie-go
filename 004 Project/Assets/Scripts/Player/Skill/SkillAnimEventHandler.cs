using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillAnimEventHandler : MonoBehaviour
{
    public event Action OnFinish;
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    public event Action OnSkillAction;

    private void AnimationFinishedTrigger() => OnFinish?.Invoke();
    private void StartMovementTrigger() => OnStartMovement?.Invoke();
    private void StopMovementTrigger() => OnStopMovement?.Invoke();
    private void AttackActionTrigger() => OnSkillAction?.Invoke();
}
