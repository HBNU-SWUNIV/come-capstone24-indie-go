using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMovement : SkillComponent<SkillMovementData>
{
    private Movement coreMovement;

    private Movement CoreMovement =>
        coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

    private void HandleStartMovement()
    {
        if (currentSkillData != null)
        {
            CoreMovement.SetVelocityX(currentSkillData.Velocity);
            //CoreMovement.SetVelocity(currentSkillData.Velocity, currentSkillData.Direction, CoreMovement.FacingDirection);
        }
    }

    private void HandleStopMovement()
    {
        CoreMovement.SetVelocityZero();
    }
    public void HandleStopMovementX()
    {
        CoreMovement.SetVelocityX(0f);
    }
    public int GetFacingDirection()
    {
        return CoreMovement.FacingDirection;
    }
    protected override void Start()
    {
        base.Start();

        eventHandler.OnStartMovement += HandleStartMovement;
        eventHandler.OnStopMovement += HandleStopMovement;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        eventHandler.OnStartMovement -= HandleStartMovement;
        eventHandler.OnStopMovement -= HandleStopMovement;
    }
}