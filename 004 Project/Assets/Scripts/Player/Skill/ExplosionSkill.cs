using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill : ISkillAction
{
    private Skill skill;

    public void Initialize(Skill skill)
    {
        this.skill = skill;
    }

    public void Enter()
    {
        // ��ü���� ��ų ������ �����Ѵ�
        Debug.Log("explosion ����");
    }

    public void Exit()
    {
        // ��ü���� ��ų ������ �����Ѵ�
        Debug.Log("explosion ����");
    }
}
