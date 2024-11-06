using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseAnimationToWeapon : MonoBehaviour
{
    public event Action OnAction;
    public event Action OnFinish;
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    public event Action OnTurnOffFlip;
    public event Action OnTurnOnFlip;
    public event Action OnNextState;

    private void AnimationFinishTrigger() => OnFinish.Invoke();

    private void AnimationActionTrigger() => OnAction.Invoke();

    private void AnimationStartMovementTrigger() => OnStartMovement.Invoke();

    private void AnimationStopMovementTrigger() => OnStopMovement.Invoke();
    private void AnimationTurnOffFlipTrigger() => OnTurnOffFlip.Invoke();
    private void AnimationTurnOnFlipTrigger() => OnTurnOnFlip.Invoke();
    private void AnimationOnNextState() => OnNextState.Invoke();
}
