using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill)
    {
        // ����: SkillMovement ������Ʈ �߰� �� �ʱ�ȭ
        SkillMovementData movementData = skill.Data.GetData<SkillMovementData>();
        if (movementData != null)
        {
            var movementComponent = skill.gameObject.AddComponent<SkillMovement>();
            movementComponent.Init();
        }

        // ����: SkillDamage ������Ʈ �߰� �� �ʱ�ȭ
        SkillDamageData damageData = skill.Data.GetData<SkillDamageData>();
        if (damageData != null)
        {
            // SkillDamage�� ó���� ������Ʈ �߰� ����
            // ��: skill.gameObject.AddComponent<DamageComponent>().Init();
        }
        //Spear���� Component�� Data�� �������� Init()

        // �߰����� ��ų ������Ʈ �ʱ�ȭ ����
        // ...


    }
}