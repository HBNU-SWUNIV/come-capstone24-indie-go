using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillGenerator
{
    public GameObject skillPrefab;

    public virtual void InitializeSkill(Skill skill, SkillDataEx data, GameObject collisionTarget = null)
    {
        skill.SetData(data);

        // 필요한 경우 CollisionHandler를 동적으로 할당.
        if (collisionTarget != null)
        {
            InitializeCollisionHandler(collisionTarget);
        }
        else
        {
            //임시
            InitializeCollisionHandler(skill.gameObject);
        }

        // 필요한 스킬 컴포넌트 추가 및 초기화
        InitializeSkillComponents(skill);

       

    }

    // 각 스킬별로 필요한 컴포넌트 추가 및 초기화   
    protected abstract void InitializeSkillComponents(Skill skill);

    private void InitializeCollisionHandler(GameObject target)
    {
        var collisionHandler = target.GetOrAddComponent<CollisionHandler>();
    }
}