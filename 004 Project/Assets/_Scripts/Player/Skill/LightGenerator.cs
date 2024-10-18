using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill, SkillDataEx data)
    {
        // Skill 컴포넌트 추가 및 초기화
        SkillMovementData movementData = data.GetData<SkillMovementData>();
        if (movementData != null)
        {
            skill.gameObject.GetOrAddComponent<SkillMovement>().Init();
        }

        SkillDamageData damageData = data.GetData<SkillDamageData>();
        if (damageData != null)
        {
            skill.gameObject.GetOrAddComponent<SkillDamage>().Init();
        }

        //Light만의 Component와 Data를 가져오고 Init()
        SkillLightData lightData = data.GetData<SkillLightData>();

        if(lightData != null)
        {
            SkillLight light = skill.gameObject.GetOrAddComponent<SkillLight>();
            light.Init(data);
        }
        // 추가적인 스킬 컴포넌트 초기화 로직
        // ...


    }
}
