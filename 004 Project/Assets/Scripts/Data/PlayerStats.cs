using UnityEngine;

public class PlayerStats : CharacterStats<PlayerStatsData>
{
    [SerializeField] private int level;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            SetStat();
        }
    }

    protected override void Start()
    {
        base.Start();
        InitializePlayerStats();
    }

    private void InitializePlayerStats()
    {
        // 여기서 id와 level을 초기화
        id = 1; // 플레이어의 고유 ID 설정
        level = 1; // 초기 레벨 설정
        SetStat();
    }

    protected override void SetStat()
    {
        // id와 level에 맞는 데이터 검색
        foreach (var kvp in GameManager.Data.PlayerStatsDict)
        {
            var stats = kvp.Value;
            if (stats.id == id && stats.level == level)
            {
                SetStatsData(stats);
                return;
            }
        }
        Debug.LogError("Failed to load player stats for id: " + id + " and level: " + level);
    }

    public void LevelUp()
    {
        Level++;
    }
}
