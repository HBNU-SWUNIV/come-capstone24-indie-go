using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillNames
{
    public const string SpearSkill = "SpearSkill";
    public const string ExplosionSkill = "ExplosionSkill";
    // �ʿ��� �ٸ� ��ų �̸��� ���⿡ �߰�
}
public class SkillData
{
}

public class SkillDamageData : SkillData
{
    public float Damage;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;
}

public class SkillCooldownData : SkillData
{
    public float Cooldown;
}

public class SkillMovementData : SkillData
{
    public Vector2 Direction;
    public float Velocity;
}

public class SkillSpearData : SkillData
{
    public float ThrowSpeed;
    public float ThrowDistance;
}

public class SkillExplosionData : SkillData
{
    public float ExplosionDistance;
}