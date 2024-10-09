using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill)
    {
        // 예시: SkillMovement 컴포넌트 추가 및 초기화
        SkillMovementData movementData = skill.Data.GetData<SkillMovementData>();
        if (movementData != null)
        {
            skill.gameObject.GetOrAddComponent<SkillMovement>().Init();
        }

        SkillDamageData damageData = skill.Data.GetData<SkillDamageData>();
        if (damageData != null)
        {
            skill.gameObject.GetOrAddComponent<SkillDamage>().Init();
        }


        //  SkillFireData fireData = skill.Data.GetData<SkillIceData>();
        //    if (fireData != null)
        //    {
        //        skill.gameObject.GetOrAddComponent<SkillIce>().Init();
        //    }

        // 추가적인 스킬 컴포넌트 초기화 로직
        // ...


    }
}