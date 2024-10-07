using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill)
    {
        // 예시: SkillMovement 컴포넌트 추가 및 초기화
        SkillMovementData movementData = skill.Data.GetData<SkillMovementData>();
        if (movementData != null)
        {
            var movementComponent = skill.gameObject.AddComponent<SkillMovement>();
            movementComponent.Init();
        }

        // 예시: SkillDamage 컴포넌트 추가 및 초기화
        SkillDamageData damageData = skill.Data.GetData<SkillDamageData>();
        if (damageData != null)
        {
            // SkillDamage를 처리할 컴포넌트 추가 로직
            // 예: skill.gameObject.AddComponent<DamageComponent>().Init();
        }
        //Spear만의 Component와 Data를 가져오고 Init()

        // 추가적인 스킬 컴포넌트 초기화 로직
        // ...


    }
}