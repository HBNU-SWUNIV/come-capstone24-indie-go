using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<string, SkillDataJson> SkillDict { get; private set; } = new Dictionary<string, SkillDataJson>();
    public Dictionary<int, PlayerStatsData> PlayerStatsDict { get; private set; } = new Dictionary<int, PlayerStatsData>();
    public Dictionary<int, EnemyStatsData> EnemyStatsDict { get; private set; } = new Dictionary<int, EnemyStatsData>();
    public Dictionary<string, ElementalEffectData> ElementalEffectDict { get; private set; } = new Dictionary<string, ElementalEffectData>();

    public void Init()
    {
        SkillDict = LoadJson<SkillsJson, string, SkillDataJson>("SkillData").MakeDict();
        PlayerStatsDict = LoadJson<PlayerStatsDataJson, int, PlayerStatsData>("PlayerStatsData").MakeDict();
        EnemyStatsDict = LoadJson<EnemyStatsDataJson, int, EnemyStatsData>("EnemyStatsData").MakeDict();
        ElementalEffectDict = LoadJson<ElementalEffectJson, string, ElementalEffectData>("ElementalEffectData").MakeDict();

    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}