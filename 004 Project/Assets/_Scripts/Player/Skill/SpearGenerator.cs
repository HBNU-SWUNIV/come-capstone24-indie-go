using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearGenerator : SkillGenerator
{
    protected override void InitializeSkillComponents(Skill skill)
    {
        // Skill ������Ʈ �߰� �� �ʱ�ȭ
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

        //Spear���� Component�� Data�� �������� Init()
        SkillSpearData spearData = skill.Data.GetData<SkillSpearData>();
        if(spearData != null)
        {
             skill.gameObject.GetOrAddComponent<SkillSpear>().Init();
        }
        // �߰����� ��ų ������Ʈ �ʱ�ȭ ����
        // ...


    }
}
