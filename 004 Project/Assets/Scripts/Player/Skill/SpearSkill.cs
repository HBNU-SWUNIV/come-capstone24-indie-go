using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSkill : ISkillAction
{
    private Skill skill;

    public void Initialize(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        // ��ü���� ��ų ������ �����Ѵ�
        Debug.Log("spear ����");
    }

    public void Exit()
    {
        // ��ü���� ��ų ������ �����Ѵ�
        Debug.Log("spear ����");
    }
}
