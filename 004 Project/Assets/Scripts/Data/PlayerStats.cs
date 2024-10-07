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
        // ���⼭ id�� level�� �ʱ�ȭ
        id = 1; // �÷��̾��� ���� ID ����
        level = 1; // �ʱ� ���� ����
        SetStat();
    }

    protected override void SetStat()
    {
        // id�� level�� �´� ������ �˻�
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
