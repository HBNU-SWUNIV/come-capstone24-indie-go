using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{
    protected override void Start()
    {
        base.Start();
        //Element = Element.Land;
        ChangeElement(Element.Land, fireLevel);

        //animator = transform.root.GetComponent<Animator>();

        InitializeMonsterStats();

    }

    private void InitializeMonsterStats()
    {
        id = 1001; // 몬스터의 고유 ID 설정
        SetStat();
    }

    protected override void SetStat()
    {
        // id에 맞는 데이터 검색
        if (GameManager.Data.EnemyStatsDict.TryGetValue(id, out var stats))
        {
            SetStatsData(stats);
        }
        else
        {
            Debug.LogError("Failed to load monster stats for id: " + id);
        }
    }

    public void SetMonsterId(int newId)
    {
        Id = newId; // ID를 설정하고, 자동으로 스탯이 재설정됨
    }
}
