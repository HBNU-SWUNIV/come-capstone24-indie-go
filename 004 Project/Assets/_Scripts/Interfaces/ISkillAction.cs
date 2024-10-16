using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillAction
{
    void Initialize(Skill skill, GameObject prefab = null, Transform prefabParent = null, Transform playerTransform = null, Vector2 prefabOffset = default(Vector2));
    void Enter();
    void Exit();
}