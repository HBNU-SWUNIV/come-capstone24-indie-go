using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLight : SkillComponent<SkillLightData>
{
    public float GetLightThrowSpeed()
    {
        return currentSkillData.ThrowSpeed;
    }
    public float GetLightThrowDistance()
    {
        return currentSkillData.ThrowDistance;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

}
