using System.Collections;
using UnityEngine;

public class EnemyStats : CharacterStats<EnemyStatsData>
{

 

    protected override void Start()
    {
        base.Start();
        //Element = Element.Land;
        InitializeMonsterStats();
        ChangeElement(Element.Land, landLevel);

        //animator = transform.root.GetComponent<Animator>();

    }
    
    private void OnEnable()
    {       
        
        // �÷��̾� Ÿ�Կ� ���� ���� ����
            StartCoroutine("AdjustStatsBasedOnPlayerType");
    }

    private IEnumerator AdjustStatsBasedOnPlayerType()
    {
        while(true)
        {
            if (OnsetStats)
                break;
            yield return null;
        }
        // ������ �⺻ ������ ����
        ResetStatsToBaseValues();
        GameManager.PlayerManager.DataAnalyze.AnalyzePlayerData(GameManager.PlayerManager.PlayerDataCollect.actionData);
        string playerType = GameManager.PlayerManager.DataAnalyze.playerType;
        Debug.Log($"GameManager.PlayerManager.DataAnalyze.playerType : {GameManager.PlayerManager.DataAnalyze.playerType}");
        switch (playerType)
        {
            case "High_parry":
            case "parry":
                // �÷��̾ �и��� ��ȣ�ϴ� ���, ���ʹ� �� ���������� �ൿ
                SetAdjustStatsAttackSpeed(1.2f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.1f);   // �̵� �ӵ� 10% ����
                break;

            case "High_dash":
            case "dash":
                // �÷��̾ ȸ�Ǹ� ��ȣ�ϴ� ���, ���ʹ� ���� �ɷ� ���
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 10% ����
                SetAdjustStatsMoveSpeed(1.4f);   // �̵� �ӵ� 30% ����
                break;

            case "High_run":
            case "run":
                // �÷��̾ ������ ��ȣ�ϴ� ���, ���ʹ� ���Ÿ� ������ �� ���� ���
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.2F);   // �̵� �ӵ� 20% ����
                break;

            default:
                //�ӽ�
                SetAdjustStatsAttackSpeed(0.8f); // ���� �ӵ� 20% ����
                SetAdjustStatsMoveSpeed(1.2F);   // �̵� �ӵ� 20% ����
                break;
        }
        Debug.Log($" playerType : {playerType} �� �ش��ϴ� ������ ����");
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
