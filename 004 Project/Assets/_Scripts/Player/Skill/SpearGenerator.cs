using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill)
    {
        // Skill 컴포넌트 추가 및 초기화
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

        //Spear만의 Component와 Data를 가져오고 Init()
        SkillSpearData spearData = skill.Data.GetData<SkillSpearData>();
        if(spearData != null)
        {
             skill.gameObject.GetOrAddComponent<SkillSpear>().Init();
        }
        // 추가적인 스킬 컴포넌트 초기화 로직
        // ...


    }
}
