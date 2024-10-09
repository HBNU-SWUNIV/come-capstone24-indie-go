using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillNames
{
    public const string SpearSkill = "SpearSkill";
    public const string FireSkill = "FireSkill";
    public const string IceSkill = "IceSkill";
    public const string LandSkill = "LandSkill";
    public const string LightSkill = "LightSkill";
    // 필요한 다른 스킬 이름도 여기에 추가
}
public class SkillData
{
}

public class SkillDamageData : SkillData
{
    public float Damage;
    public Vector2 knockbackAngle;
    public float knockbackStrength;
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

public class SkillFireData : SkillData
{
    public float FireDistance;
}
public class SkillIceData : SkillData
{
  //  public float FireDistance;
}
public class SkillLandData : SkillData
{
  //  public float FireDistance;
}
public class SkillLightData : SkillData
{
  //  public float FireDistance;
}
