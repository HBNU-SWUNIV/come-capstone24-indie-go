using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsData : CharacterStatsData
{
}


[Serializable]
public class EnemyStatsDataJson : ILoader<int, EnemyStatsData>
{
    public List<EnemyStatsEntry> enemyStats;

    [Serializable]
    public class EnemyStatsEntry
    {
        public int key;
        public EnemyStatsData value;
    }

    public Dictionary<int, EnemyStatsData> MakeDict()
    {
        Dictionary<int, EnemyStatsData> dict = new Dictionary<int, EnemyStatsData>();
        foreach (EnemyStatsEntry entry in enemyStats)
        {
            dict.Add(entry.key, entry.value);
        }
        return dict;
    }
}