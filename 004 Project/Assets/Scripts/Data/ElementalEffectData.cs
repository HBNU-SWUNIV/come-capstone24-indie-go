using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementalEffectLevelData
{
    public float Fire_dotDamage;
    public float Fire_duration;
    public int Fire_maxStacks;
    public float Ice_slowEffect;
    public float Ice_duration;
    public float Land_decreaseAttackSpeed;
    public float Land_increaseAttackDamage;
    public float Lightning_stunDuration;
}

[Serializable]
public class ElementalEffectData
{
    public ElementalEffectLevelData level1;
    public ElementalEffectLevelData level2;
    public ElementalEffectLevelData level3;
}

[Serializable]
public class ElementalEffectJson : ILoader<string, ElementalEffectData>
{
    public List<ElementalEffectEntry> effects;

    [Serializable]
    public class ElementalEffectEntry
    {
        public string key;
        public ElementalEffectData value;
    }

    public Dictionary<string, ElementalEffectData> MakeDict()
    {
        Dictionary<string, ElementalEffectData> dict = new Dictionary<string, ElementalEffectData>();
        foreach (ElementalEffectEntry entry in effects)
        {
            dict.Add(entry.key, entry.value);
        }
        return dict;
    }
}
