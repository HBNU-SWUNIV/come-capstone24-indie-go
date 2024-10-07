using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill : ISkillAction
{
    private Skill skill;

    public void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default(Vector2))
    {
        this.skill = skill;
    }

    public void Enter()
    {
        // 구체적인 스킬 로직을 구현한다
        Debug.Log("explosion 진입");
    }

    public void Exit()
    {
        // 구체적인 스킬 로직을 구현한다
        Debug.Log("explosion 종료");
    }
}
