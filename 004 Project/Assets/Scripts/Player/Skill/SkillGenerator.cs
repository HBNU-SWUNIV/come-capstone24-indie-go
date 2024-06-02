using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillGenerator
{
    public GameObject skillPrefab;

    public virtual void InitializeSkill(Skill skill, SkillDataEx data, GameObject collisionTarget = null)
    {
        skill.SetData(data);

        // �ʿ��� ��� CollisionHandler�� �������� �Ҵ�.
        if (collisionTarget != null)
        {
            InitializeCollisionHandler(collisionTarget);
        }
        else
        {
            //�ӽ�
            InitializeCollisionHandler(skill.gameObject);
        }

        // �ʿ��� ��ų ������Ʈ �߰� �� �ʱ�ȭ
        InitializeSkillComponents(skill);

       

    }

    // �� ��ų���� �ʿ��� ������Ʈ �߰� �� �ʱ�ȭ   
    protected abstract void InitializeSkillComponents(Skill skill);

    private void InitializeCollisionHandler(GameObject target)
    {
        var collisionHandler = target.GetOrAddComponent<CollisionHandler>();
    }
}