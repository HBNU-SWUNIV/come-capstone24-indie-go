using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{
    protected override void Start()
    {
        base.Start();
        //Element = Element.Land;
        ChangeElement(Element.Land, landLevel);

        //animator = transform.root.GetComponent<Animator>();

        InitializeMonsterStats();

    }

    private void InitializeMonsterStats()
    {
        //id = 1001; // ������ ���� ID ����
        SetStat();
    }

    protected override void SetStat()
    {
        // id�� �´� ������ �˻�
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
        Id = newId; // ID�� �����ϰ�, �ڵ����� ������ �缳����
    }
}
