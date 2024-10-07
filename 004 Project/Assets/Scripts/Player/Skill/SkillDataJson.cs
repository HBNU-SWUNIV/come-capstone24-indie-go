using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkillDataJson
{
    public SkillDamageData damageData;
    public SkillCooldownData cooldownData;
    public SkillMovementData movementData;
    public SkillSpearData spearData;
    public SkillExplosionData explosionData;
}

[Serializable]
public class SkillsJson : ILoader<string, SkillDataJson>
{
    public List<SkillEntry> skills;

    [Serializable]
    public class SkillEntry
    {
        public string key;
        public SkillDataJson value;
    }

    public Dictionary<string, SkillDataJson> MakeDict()
    {
        Dictionary<string, SkillDataJson> dict = new Dictionary<string, SkillDataJson>();
        foreach (SkillEntry entry in skills)
        {
            dict.Add(entry.key, entry.value);
        }
        return dict;
    }
}